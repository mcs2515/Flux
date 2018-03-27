using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

	static GameStateController instance;
	public bool paused;
	public GameObject pause_screen;
	public GameObject start_screen;
	public GameObject results_screen;
	public bool reset;


	// Use this for initialization
	void Start () {
		reset = true;
		paused = true;

		StartGame();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Resume(){
		//hide all ui screens and buttons
		reset = false;
		paused = false;

		pause_screen.SetActive(false);
		results_screen.SetActive(false);
		start_screen.SetActive(false);
		Debug.Log ("resumed");
	}

	public void Pause(){
		//show only pause screen
		paused = true;

		pause_screen.SetActive(true);
		results_screen.SetActive(false);
		start_screen.SetActive(false);
		Debug.Log ("paused");
	}

	public void End(){
		//show only pause screen
		paused = true;

		pause_screen.SetActive(false);
		results_screen.SetActive(true);
		start_screen.SetActive(false);
		Debug.Log ("paused");
	}

	public void StartGame(){
		//show only title screen
		reset = true;
		paused = true;

		pause_screen.SetActive(false);
		results_screen.SetActive (false);
		start_screen.SetActive(true);
		Debug.Log ("start screen");
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
