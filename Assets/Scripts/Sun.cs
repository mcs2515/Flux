using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    float distance_z = 490;
	float distance_x =  1.3f;
    GameObject player;
	public GameObject sun;

	//Vector3 sun_position = Vector3.zero;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
		sun.SetActive (true);
    }
	
	// Update is called once per frame
	void Update () {
		Vector3 current_position = transform.position;

		current_position.z = player.transform.position.z + distance_z;

		transform.position = current_position;
    }
}
