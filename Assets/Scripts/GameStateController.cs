using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState_e{
	START,
	GAME,
	PAUSE,
	RESULT,
}

public class GameStateController : MonoBehaviour {

	static GameStateController instance;
	public GameObject pause_screen;
	public GameObject start_screen;
	public GameObject results_screen;

	GameState_e gamestate;
	public GameState_e previous_state;
	public GameState_e next_state;
		
	// Use this for initialization
	void Start () {
		gamestate = GameState_e.START;
		previous_state = gamestate;
		CheckGameState(gamestate);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void StartMenu(){
		//show only title screen
		gamestate = GameState_e.START;

		CheckGameState(gamestate);
		Debug.Log ("start screen");
	}

	public void ResumeMenu(){
		//hide all ui screens and buttons
		gamestate = GameState_e.GAME;

		CheckGameState(gamestate);
		Debug.Log ("resumed");
	}

	public void PauseMenu(){
		//show only pause screen
		gamestate = GameState_e.PAUSE;

		CheckGameState(gamestate);
		Debug.Log ("paused");
	}

	public void ResultMenu(){
		//show only pause screen
		gamestate = GameState_e.RESULT;

		CheckGameState(gamestate);
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

	public GameState_e  GetGameState(){
		return gamestate;
	}

	void CheckGameState(GameState_e state){
		pause_screen.SetActive(false);
		results_screen.SetActive(false);
		start_screen.SetActive(false);
		next_state = state;

		switch (state) {
			case GameState_e.START:
				start_screen.SetActive(true);
				break;
			case GameState_e.GAME:
				break;
			case GameState_e.PAUSE:
				pause_screen.SetActive (true);
				break;
			case GameState_e.RESULT:
				results_screen.SetActive(true);
				break;
		default:
			break;

			if (previous_state != state) {
				previous_state = state;
			}
		}
	}
}
