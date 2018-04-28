using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimization : MonoBehaviour {
	GameObject player;
	GameObject[] obstacles;
	int passedObs = 0;
	public PlayerMove playerScript;

	// Use this for initialization
	void Start () {
		
		player = GameObject.Find ("Player");
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

		for (int i = 0; i < obstacles.Length; i++) {
			obstacles [i].GetComponent<Renderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*Debug.Log (playerScript.controller.transform.position + " " + obstacles [passedObs].transform.position);
		if (Mathf.Abs(player.transform.position.z - obstacles [passedObs].transform.position.z) < 130) {
			obstacles [passedObs].SetActive (true);
			passedObs++;
		}
		if ( passedObs > 1 && Mathf.Abs(player.transform.position.z - obstacles [passedObs - 1].transform.position.z) < 10) {
			obstacles [passedObs].SetActive (false);
		}*/
	}
}
