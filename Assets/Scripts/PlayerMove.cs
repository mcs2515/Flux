using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float speed;
    public float drag;
    public float jumpforce;
    private float gravity = 30f;
    private Vector3 moveDir = Vector3.zero;
    CharacterController controller;
    Vector3 acceleration;
    Vector3 norm_accel;

	// Use this for initialization
	void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        //Debug.Log("start");
    }
	
	// Update is called once per frame
	void Update () {

        //accelerometer input is based on the rotation of the phone

        if (controller.isGrounded)
        {
            acceleration = Input.acceleration;
            Debug.Log("Acceleration: "+ acceleration.sqrMagnitude);

            //Debug.Log("Mag: " + acceleration.sqrMagnitude);

            //jump
            //if (acceleration.sqrMagnitude >= 2f)
            //{
            //    moveDir = new Vector3(0, Input.acceleration.y, 0);
            //    moveDir = Vector3.ClampMagnitude(moveDir, 1);
            //    moveDir = transform.TransformDirection(moveDir);
            //    moveDir *= jumpforce;
            //}

            //run
            //if (acceleration.sqrMagnitude >= .2f)
            //{
            //    //mapping the y movement of phone to z movement of player
            //    moveDir = new Vector3(0, 0, Mathf.Clamp01(Input.acceleration.z));
            //    moveDir = Vector3.ClampMagnitude(moveDir, 1);
            //    moveDir = transform.TransformDirection(moveDir);
            //    moveDir *= (speed - (drag * Time.deltaTime));

            //    Debug.Log("MoveDirection: " + moveDir);
            //}
        }

        //ground the person
        moveDir.y -= gravity * Time.deltaTime;
        //slow down the player
        //moveDir.z -= drag * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);
    }
}
