using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float speed = 10f;
    public float jumpforce = 50f;
    private float gravity = 30f;
    private Vector3 moveDir = Vector3.zero;
    CharacterController controller;
    Vector3 acceleration;

	// Use this for initialization
	void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        //Debug.Log("start");
    }
	
	// Update is called once per frame
	void Update () {
        if (controller.isGrounded)
        {
            acceleration = Input.acceleration;
            Debug.Log("Acceleration: "+acceleration);

            //Debug.Log("Mag: " + acceleration.sqrMagnitude);
            if (acceleration.sqrMagnitude >= 5f)
            {         
                moveDir = new Vector3(0, 0, -Input.acceleration.z);
                //Debug.Log(moveDir.x + "," + moveDir.y + "," + moveDir.z);

                //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDir = Vector3.ClampMagnitude(moveDir, 1);
                moveDir = transform.TransformDirection(moveDir);
                moveDir *= speed;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDir.y = jumpforce;
                }
            }

            //slow down if user stops moving
            //acceleration.y -= gravity * Time.deltaTime;
        }

        //ground the person
        moveDir.y -= gravity * Time.deltaTime;
        //Debug.Log("player.y = "+ moveDir.y);
        controller.Move(moveDir * Time.deltaTime);
    }
}
