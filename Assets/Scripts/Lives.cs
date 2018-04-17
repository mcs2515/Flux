using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour {

	public GameObject health1;
	public GameObject health2;
	public GameObject health3;
	GameObject obj_near;

	int lives;
	public CharacterController controller;
	public PlayerMove playerScript;
	public DetectObstacles obstacleScript;

	// Use this for initialization
	void Start () {
		lives = playerScript.Lives;
		health1.SetActive (false);
		health2.SetActive (false);
		health3.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		lives = playerScript.Lives;

		if (GameStateController.Instance.GetGameState () == GameState_e.GAME) {
			if (lives < 0) {
				GameStateController.Instance.ResultMenu();
			}
			checkLives ();
		}
	}

	void checkLives(){
		health1.SetActive (false);
		health2.SetActive (false);
		health3.SetActive (false);

		switch (playerScript.Lives) {
		case 0:
			health1.SetActive (true);
			break;
		case 1:
			health2.SetActive (true);
			health3.SetActive (true);
			break;
		case 2:
			health1.SetActive (true);
			health2.SetActive (true);
			health3.SetActive (true);
			break;
		}
	}
}
