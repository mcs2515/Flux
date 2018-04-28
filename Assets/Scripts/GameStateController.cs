using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GameState_e{
	START,
	COUNTDOWN,
	GAME,
	PAUSE,
	RESULT,
}

public class GameStateController : MonoBehaviour {

	static GameStateController instance;
	public GameObject pause_screen;
	public GameObject start_screen;
	public GameObject game_screen;
	public GameObject results_screen;
	public GameObject results_background;
	public GameObject canvas;
	public GameObject result_sprite;

	GameState_e gamestate;
	public bool delay_input;
		
	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		gamestate = GameState_e.START;
		delay_input = true;

		if (canvas) {
			canvas.SetActive (true);
		}

		StartMenu();
	}

	public void StartMenu(){
		//show only title screen
		gamestate = GameState_e.START;
		delay_input = true;

		CheckGameState(gamestate);
		GameObject.Find("Player").GetComponent<PlayerMove>().ChangePlayerState(PlayerState_e.STANDBY);
	}

	public void ResumeMenu(){
		//hide all ui screens and buttons
		gamestate = GameState_e.GAME;

		CheckGameState(gamestate);
	}

	public void PauseMenu(){
		//show only pause screen
		gamestate = GameState_e.PAUSE;

		CheckGameState(gamestate);
		GameObject.Find("Player").GetComponent<PlayerMove>().ChangePlayerState(PlayerState_e.STANDBY);
	}

	public void ResultMenu(){
		//show only pause screen
		gamestate = GameState_e.RESULT;

		CheckGameState(gamestate);

		//server stuff
		GameObject.Find("Player").GetComponent<PlayerMove>().ChangePlayerState(PlayerState_e.STANDBY);
		string resultTime = GameObject.Find ("GameTimer").GetComponent<Timer> ().GetTime ();
		StartCoroutine (UploadScore (resultTime) );
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

	public bool Delay_Input{
		get{ return delay_input; }
		set{ delay_input = value; }
	}

	void CheckGameState(GameState_e state){
		pause_screen.SetActive(false);
		results_screen.SetActive(false);
		start_screen.SetActive(false);
		game_screen.SetActive (false);
		results_background.SetActive (false);
		result_sprite.SetActive (false);

		switch (state) {
			case GameState_e.START:
				start_screen.SetActive(true);
				break;
			case GameState_e.GAME:
				game_screen.SetActive (true);
				break;
			case GameState_e.PAUSE:
				pause_screen.SetActive (true);
				break;
			case GameState_e.RESULT:
				result_sprite.SetActive (true);
				results_screen.SetActive(true);
				results_background.SetActive (true);
				break;
		default:
			break;
		}
	}

	public IEnumerator UploadScore(string time){
		//byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
		UnityWebRequest www = UnityWebRequest.Get("http://serenity.ist.rit.edu/~amp4129/341/flux/scores.php?i=0&score=" + time);
		yield return www.Send();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Upload complete!");
		}
	}
}
