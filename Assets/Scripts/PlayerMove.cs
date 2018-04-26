using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum PlayerState_e{
	NONE,
	STANDBY,
	RUNNING,
	JUMP,
	MISS
}

public class PlayerMove : MonoBehaviour {

	PlayerState_e playerState = PlayerState_e.STANDBY;
	PlayerState_e playerStateLast;
    CharacterController controller;
	GameObject player;
    Vector3 moveDir = Vector3.zero;
	int lives;

	Vector3 start_position = Vector3.zero;
	Vector3 start_rotation = Vector3.zero;

    public float speed;
    private float drag;
    public float jumpforce;
    float gravity = 9.5f;
	private float delay;

    Vector3 acceleration;
    Vector3 deltaAcceleration;
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    public float shakeDetectionThreshold = 0f;
    public float jumpDetectionThreshold = 0f;
    float lowPassFilterFactor;
    private Vector3 lowPassValue;
	private float timer;

    // Use this for initialization
    void Start () {
		playerState = PlayerState_e.NONE;
		playerStateLast = playerState;
		player = GameObject.Find ("Player");

		start_position = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z);
		start_rotation = new Vector3 (player.transform.eulerAngles.x, player.transform.eulerAngles.y, player.transform.eulerAngles.z);

        controller = gameObject.GetComponent<CharacterController>();    
		timer = 0f;
		delay = 0f;

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
		acceleration = Vector3.zero;
		lowPassValue = Vector3.zero;

        Input.gyro.enabled = true;
    }

	// Update is called once per frame
	void Update () {

		//play game
		if (GameStateController.Instance.GetGameState() == GameState_e.GAME) {
			//delay input if user is coming from pause or start menu
			if (GameStateController.Instance.Delay_Input) {
				delay += Time.deltaTime;
				float seconds = Mathf.Floor (delay);
				VideoControllers.Instance.PlayClips ("countdown");
				GameStateController.Instance.game_screen.SetActive (false);

				if (seconds >= 4f) {
					VideoControllers.Instance.PlayClips ("none");
					GameStateController.Instance.Delay_Input = false;
					GameStateController.Instance.game_screen.SetActive (true);
					delay = 0;
				}
			} 
			else {
				PlayerMovement ();
			}
		}
		//reset game
		else if (GameStateController.Instance.GetGameState() == GameState_e.START) {
			ResetPlayer ();
		}
	}

	void PlayerMovement(){
		//Debug.Log (drag);
		if (controller.isGrounded) {
			acceleration = Input.acceleration;
			lowPassValue = Vector3.Lerp (lowPassValue, acceleration, lowPassFilterFactor);
			deltaAcceleration = acceleration - lowPassValue;

			drag = .6f;

			//if moving
			if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold) {
				moveDir = new Vector3 (0f, 0f, 1f);
				moveDir *= speed;
				ChangePlayerState(PlayerState_e.RUNNING);
			}

			//if jumping
			if (deltaAcceleration.sqrMagnitude >= jumpDetectionThreshold) {
				ChangePlayerState (PlayerState_e.JUMP);
				moveDir = new Vector3 (0, 1f, moveDir.z * 1.5f);
				moveDir = transform.TransformDirection (moveDir);
				moveDir.y *= jumpforce;
			}
		} 
		else {
			drag = .3f;
		}

		//always ground the person
		moveDir.y -= gravity * Time.deltaTime;
		moveDir.x = 0;

		//slow down player
		if (moveDir.z > 0) {
			moveDir.z -= drag;
		} 
		else {
			moveDir.z = 0;
		}

		moveDir = transform.TransformDirection (moveDir);
		controller.Move (moveDir * Time.deltaTime);

		CheckPlayer ();
	}

	void CheckPlayer(){
		//if player does not move, pause game
		timer += Time.deltaTime;

		if ((deltaAcceleration.sqrMagnitude < shakeDetectionThreshold) && moveDir.z == 0) {
			if (timer >= 10) {
				//check again
				if (deltaAcceleration.sqrMagnitude < shakeDetectionThreshold) {
					//TODO: insert  warning text here
					timer = 0;
				}
			}
		} 
		else {
			timer = 0;
		}
	}

	void ResetPlayer(){
		ChangePlayerState(PlayerState_e.STANDBY);
		player.transform.position = start_position;
		player.transform.eulerAngles = start_rotation;
		lives = 2;
	}

	public int Lives{
		get{return lives;}
		set{lives=value;}
	}

	public void ChangePlayerState(PlayerState_e newState){
		if (newState != playerState) {
			
			playerStateLast = playerState;
			playerState = newState;

			StartCoroutine (UploadState (playerState.ToString ()));

			Debug.Log ("changed state to " + newState);
		}
	}

	public IEnumerator UploadState(string playerStateString){
		//byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
		UnityWebRequest www = UnityWebRequest.Get("http://serenity.ist.rit.edu/~amp4129/341/flux/scores.php?i=0&state=" + playerStateString);
		yield return www.Send();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
	}

	public IEnumerator UploadScore(string finalScore){
		//byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
		UnityWebRequest www = UnityWebRequest.Get("http://serenity.ist.rit.edu/~amp4129/341/flux/scores.php?i=2&score=" + finalScore);
		yield return www.Send();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
	}
}
