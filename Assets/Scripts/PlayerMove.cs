using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {


    CharacterController controller;
    Vector3 moveDir = Vector3.zero;
    Camera controllerCamera;

    public float speed;
    public float drag;
    public float jumpforce;
    float gravity = 9.81f;

    Vector3 acceleration;
    Vector3 deltaAcceleration;
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    public float shakeDetectionThreshold = 0f;
    public float jumpDetectionThreshold = 0f;
    public float duckDetectionThreshold = 0f;
    float lowPassFilterFactor;
    private Vector3 lowPassValue;
    private bool ducking = false;

    // Use this for initialization
    void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        acceleration = Vector3.zero;
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

        //if (controller.isGrounded)
        //{
        //    acceleration = Input.acceleration;
        //    lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        //    deltaAcceleration = acceleration - lowPassValue;

        //    //Debug.Log("sqrt_delta_accel: " + deltaAcceleration.sqrMagnitude);
        //    //Debug.Log("accleration: " + acceleration);
        //    Debug.Log("delta_accel: " + deltaAcceleration);

        //    if (!ducking)
        //    {
        //        //if moving
        //        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        //        {
        //            moveDir = new Vector3(0, 0, 1f);
        //            moveDir = transform.TransformDirection(moveDir);
        //            moveDir *= speed;
        //        }

        //        //if jumping
        //        if (deltaAcceleration.sqrMagnitude >= jumpDetectionThreshold)
        //        {
        //            moveDir = new Vector3(0, 1f, moveDir.z);
        //            Debug.Log("move z: " + moveDir.z);
        //            moveDir = transform.TransformDirection(moveDir);
        //            moveDir.y *= jumpforce;

        //            //Debug.Log("z: " + moveDir.z);
        //        }

        //        //if ducking
        //        else if (deltaAcceleration.sqrMagnitude <= duckDetectionThreshold)
        //        {
        //            //skrink player
        //            transform.localScale = new Vector3(1f, .5f, 1f);
        //            ducking = true;
        //        }
        //    }
        //    else
        //    {
        //        //if moving again
        //        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        //        {
        //            transform.localScale = new Vector3(1f, 1f, 1f);
        //            ducking = false;
        //        }
        //    }


        //    //rotate character and its camera based on accelerometer
        //    transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
        //    //controllerCamera.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);
        //}

        ////always ground the person
        //moveDir.y -= gravity * Time.deltaTime;
        //moveDir.x = 0;

        ////slow down player
        //if (moveDir.z > 0)
        //{
        //    moveDir.z -= drag;
        //    //moveDir.x -= drag;
        //}
        //else
        //{
        //    moveDir.z = 0;
        //    //moveDir.x = 0;
        //}

        //controller.Move(moveDir * Time.deltaTime);

        Vector3 dir = Vector3.zero;
        dir.x = -Input.acceleration.y;
        dir.z = Input.acceleration.x;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime;
        controller.Move(dir * speed);
    }
}
