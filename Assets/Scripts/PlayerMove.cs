using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState_e{
	STANDBY,
	RUNNING,
	JUMP,
	MISS
}

public class PlayerMove : MonoBehaviour {

	PlayerState_e playerState = PlayerState_e.STANDBY;
	//PlayerState_e playerStateLast = this.playerState;
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
		playerState = PlayerState_e.STANDBY;
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

				if (seconds >= .5f) {
					GameStateController.Instance.Delay_Input = false;
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
			ChangePlayerState(PlayerState_e.RUNNING);
			acceleration = Input.acceleration;
			lowPassValue = Vector3.Lerp (lowPassValue, acceleration, lowPassFilterFactor);
			deltaAcceleration = acceleration - lowPassValue;


			drag = .6f;
			//Debug.Log("sqrt_delta_accel: " + deltaAcceleration.sqrMagnitude);
			//Debug.Log("accleration: " + acceleration);
			//Debug.Log("delta_accel: " + deltaAcceleration);

			//rotate character and its camera based on accelerometer
			//controller.transform.Rotate (0, -Input.gyro.rotationRateUnbiased.y * 1.5f, 0);

			//if moving
			if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold) {
				moveDir = new Vector3 (0f, 0f, 1f);
				moveDir *= speed;
			}

			//if jumping
			if (deltaAcceleration.sqrMagnitude >= jumpDetectionThreshold) {
				playerState = PlayerState_e.JUMP;
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
			playerState = PlayerState_e.STANDBY;
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
		playerState = newState;
		Debug.Log ("changed state to " + newState);
	}

	public void CheckPlayerState(){
		//if (this.playerState != this.playerStateLast) {
			//Debug.Log("player state changed");

//		}
	}
}
