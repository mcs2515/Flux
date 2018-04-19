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
		/*Vector3 position = (transform.position - player.transform.position).normalized * distance + player.transform.position;

		//change the x and z axis, keep y the same
		sun_position = new Vector3 (position.x, transform.position.y, position.z);
		transform.position = sun_position;*/
		Vector3 current_position = transform.position;

		current_position.z = player.transform.position.z + distance_z;
		//current_position.x = player.transform.position.x + distance_x;

		transform.position = current_position;
    }
}
