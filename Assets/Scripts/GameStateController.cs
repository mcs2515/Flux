﻿using System.Collections;
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

	public bool reset;
	public bool paused;

	public GameState_e gamestate;
		
	// Use this for initialization
	void Start () {
		reset = true;
		paused = true;

		gamestate = GameState_e.START;
		CheckGameState(gamestate);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void StartMenu(){
		//show only title screen
		reset = true;
		paused = true;
		gamestate = GameState_e.START;

		CheckGameState(gamestate);
		Debug.Log ("start screen");
	}

	public void ResumeMenu(){
		//hide all ui screens and buttons
		reset = false;
		paused = false;
		gamestate = GameState_e.GAME;

		CheckGameState(gamestate);
		Debug.Log ("resumed");
	}

	public void PauseMenu(){
		//show only pause screen
		paused = true;
		gamestate = GameState_e.PAUSE;

		CheckGameState(gamestate);
		Debug.Log ("paused");
	}

	public void ResultMenu(){
		//show only pause screen
		paused = true;
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

	void CheckGameState(GameState_e state){
		pause_screen.SetActive(false);
		results_screen.SetActive(false);
		start_screen.SetActive(false);

		switch (state) {
		case GameState_e.START:
			start_screen.SetActive(true);
			break;
		case GameState_e.GAME:
			paused = false;
			break;
		case GameState_e.PAUSE:
			paused = true;
			pause_screen.SetActive (true);
			break;
		case GameState_e.RESULT:
			results_screen.SetActive(true);
			break;
		default:
			break;
		}
	}
}
