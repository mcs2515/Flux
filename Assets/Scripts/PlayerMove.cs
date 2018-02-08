using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float speed;
    public float drag;
    public float jumpforce;
    private float gravity = 9.81f;
    private Vector3 moveDir = Vector3.zero;
    CharacterController controller;
    Vector3 acceleration = Vector3.zero;
    Vector3 norm_accel;

    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    public float shakeDetectionThreshold = 0f;
    public float jumpDetectionThreshold = 0f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;
    Vector3 deltaAcceleration;

    // Use this for initialization
    void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        //Debug.Log("start");
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }
	
	// Update is called once per frame
	void Update () {

        //accelerometer input is based on the rotation of the phone

        if (controller.isGrounded)
        {
            acceleration = Input.acceleration;
            lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
            deltaAcceleration = acceleration - lowPassValue;

            Debug.Log("delta_accel: " + deltaAcceleration.sqrMagnitude);
            //if moving
            if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            {
                moveDir = new Vector3(0, 0, 1f);
                moveDir = transform.TransformDirection(moveDir);
                moveDir *= speed;
            }
            else
            {
                Debug.Log("Slowing down");
                //slow down player
                if (moveDir.z > 0)
                {
                    moveDir.z -= drag;
                }
                else
                {
                    moveDir.z = 0;
                }
            }

            //if jumping
            if (deltaAcceleration.sqrMagnitude >= jumpDetectionThreshold)
            {
                moveDir = new Vector3(0, 1f, moveDir.z);
                moveDir = transform.TransformDirection(moveDir);
                moveDir.y *= jumpforce;

                //Debug.Log("z: " + moveDir.z);
            }
        }

        //always ground the person
        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);
    }
}
