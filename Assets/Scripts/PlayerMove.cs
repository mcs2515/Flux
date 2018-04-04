using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {


    CharacterController controller;
    Vector3 moveDir = Vector3.zero;
    Camera controllerCamera;

    public float speed;
    private float drag;
    public float jumpforce;
    float gravity = 9.5f;

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
        controller = gameObject.GetComponent<CharacterController>();
        acceleration = Vector3.zero;
		timer = 0f;
        //Debug.Log("start");
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        Input.gyro.enabled = true;
        controllerCamera = GameObject.Find("playerCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

		//accelerometer input is based on the rotation of the phone
		if (!GameStateController.Instance.paused) {
			Debug.Log (drag);
			if (controller.isGrounded) {
				acceleration = Input.acceleration;
				lowPassValue = Vector3.Lerp (lowPassValue, acceleration, lowPassFilterFactor);
				deltaAcceleration = acceleration - lowPassValue;

				drag = .6f;
				//Debug.Log("sqrt_delta_accel: " + deltaAcceleration.sqrMagnitude);
				//Debug.Log("accleration: " + acceleration);
				//Debug.Log("delta_accel: " + deltaAcceleration);

				//rotate character and its camera based on accelerometer
				controller.transform.Rotate (0, -Input.gyro.rotationRateUnbiased.y * 1.5f, 0);

				//if moving
				if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold) {
					moveDir = new Vector3 (0f, 0f, 1f);
					moveDir *= speed;
				}

				//if jumping
				if (deltaAcceleration.sqrMagnitude >= jumpDetectionThreshold) {
					moveDir = new Vector3 (0, 1f, moveDir.z * 1.5f);
					moveDir = transform.TransformDirection (moveDir);
					moveDir.y *= jumpforce;
				}
			} else {
				drag = .3f;
			}

			//always ground the person
			moveDir.y -= gravity * Time.deltaTime;
			moveDir.x = 0;

			//slow down player
			if (moveDir.z > 0) {
				moveDir.z -= drag;
				//moveDir.x -= drag;
			} else {
				moveDir.z = 0;
				//moveDir.x = 0;
			}

			moveDir = transform.TransformDirection (moveDir);
			controller.Move (moveDir * Time.deltaTime);

			//Debug.Log ("delta mag: " + deltaAcceleration.sqrMagnitude);
			checkPlayer ();

		}
	}

	void checkPlayer(){
		//if player does not move for 5 seconds, pause game
		timer += Time.deltaTime;
		//Debug.Log (timer);
		if ((deltaAcceleration.sqrMagnitude < shakeDetectionThreshold) && moveDir.z == 0) {
			if (timer >= 3 && Mathf.Floor (timer) % 3 == 0) {
				//check again
				if (deltaAcceleration.sqrMagnitude < shakeDetectionThreshold) {
					GameStateController.Instance.PauseMenu();
					timer = 0;
				}
			}

		} else {
			timer = 0;
		}
	}
}
