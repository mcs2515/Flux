using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

	static GameStateController instance;
	public bool paused;


	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		//check to see if player movement is 0
		//after 5 sec of no movement, bring up pause menu
		//if resume, go back to game
		//else bring them to results screen

		//if player reaches the end, bring them to the results screen
		//if end game and player selects restart,restart game
		//else bring them to main menu
	}

	public void Resume(){
		paused = false;
		Debug.Log ("resumed");
	}

	public void Pause(){
		paused = true;
		Debug.Log ("paused");
	}

	public static GameStateController Instance{
		get{
			if (instance == null) {
				instance = GameStateController.FindObjectOfType<GameStateController> ();
			}
			return GameStateController.instance; 
		}
		set{ GameStateController.instance = value;}
	}

	void Quit(){
	}
}
