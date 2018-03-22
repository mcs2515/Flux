using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame(){
		SceneManager.LoadScene("Main"); //Game State
	}

	public void EndGame(){
		Application.Quit();
	}

	public void MainMenu(){
		SceneManager.LoadScene ("Main_UI");
	}

	public void Pause(){
	}

	public void Resume(){
	}
}
