﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectObstacles : MonoBehaviour {

	public GameObject jump_sprite;
	public GameObject warning_sprite;

	public GameObject[] obstacles;
	public GameObject damage_screen;
	public Material skybox;
	//Color skyDayColor = new Color(.234f, .297331f, .328f, 1);
	Color skyDayColor = new Color(91f/255f, 89f/255f, 91f/255f, 1);
	Color skyDmgColor = new Color(197f/255f, 23f/255f, 30f/255f, 1);

	Color32 lives_white = new Color32 (255,255,255,128);
	Color32 lives_red = new Color32 (239, 65, 65, 255);

	GameObject obj_near;
	public PlayerMove playerScript;
	float timer;
	int counter;
	float skyLerp;

	public CharacterController controller;
	public Lives lives;

	// Use this for initialization
	void Start () {
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

		skybox = RenderSettings.skybox;
		skybox.SetColor("_Tint", skyDayColor);
		RenderSettings.skybox = skybox;

		jump_sprite.SetActive (false);
		warning_sprite.SetActive (false);
		//damage_screen.SetActive (false);
		timer = 0;
		counter = 0;
		skyLerp = 0;
	}

	// Update is called once per frame
	void Update () {
		
		if (GameStateController.Instance.GetGameState () == GameState_e.GAME) {
			obj_near = FindNearestObj ();
			Display_Sprites (obj_near);

			//if colliding
			if (IsColliding (obj_near)) {

				timer += Time.deltaTime;
				skyLerp += Time.deltaTime/2f;

				lives.health1.GetComponent<Image>().color =  lives_red;
				lives.health2.GetComponent<Image>().color =  lives_red;
				lives.health3.GetComponent<Image>().color =  lives_red;


				if (skyLerp > 1f) {
					skyLerp = 1f;
				}
					
				//damage_screen.SetActive (true);
				skybox.SetColor("_Tint", Color.Lerp(skyDayColor,skyDmgColor, skyLerp));
				RenderSettings.skybox = skybox;

				if (counter == 0) {
					playerScript.Lives--;
					counter++;
					GameObject.Find ("Player").GetComponent<PlayerMove> ().ChangePlayerState (PlayerState_e.MISS);
				}

				/*if (timer >= .5f) {
					//damage_screen.SetActive (false);
				}*/

				if (timer >= 3.0f) {
					counter = 0;
					timer = 0;
				}
			} 
			//if not currently colliding
			else {
				counter = 0;
				timer = 0;
				skyLerp -= Time.deltaTime;

				if (skyLerp < 0) {
					skyLerp = 0;
				}

				lives.health1.GetComponent<Image>().color =  lives_white;
				lives.health2.GetComponent<Image>().color =  lives_white;
				lives.health3.GetComponent<Image> ().color = lives_white;

				skybox.SetColor("_Tint", Color.Lerp(skyDayColor, skyDmgColor,skyLerp));
				RenderSettings.skybox = skybox;
				//damage_screen.SetActive (false);
			}
		} 
		else {
			jump_sprite.SetActive (false);
			warning_sprite.SetActive (false);
			skybox.SetColor("_Tint",skyDayColor);
			RenderSettings.skybox = skybox;
			//damage_screen.SetActive (false);
		}

		if (GameStateController.Instance.GetGameState () == GameState_e.START) {
			timer = 0;
			counter = 0;
			skyLerp = 0;
			GameObject.Find ("Player").GetComponent<PlayerMove> ().ChangePlayerState (PlayerState_e.STANDBY);
		}
	}

	public GameObject FindNearestObj(){
		foreach(GameObject obj in obstacles) {
			if ((controller.transform.position - obj.transform.position).magnitude <= 30f) {
				return obj;
			}
		}
		return null;
	}

	public void Display_Sprites(GameObject obj){
		if (obj) {
			if ((controller.transform.position - obj.transform.position).magnitude <= 25f) {
				jump_sprite.SetActive (true);
			} 
			else {
				jump_sprite.SetActive (false);
			}
		}

		if (playerScript.playerState == PlayerState_e.STOPED) {
			warning_sprite.SetActive (true);
			jump_sprite.SetActive (false);
		}

		if (playerScript.playerState == PlayerState_e.RUNNING) {
			warning_sprite.SetActive (false);
		}
	}

	public GameObject GetNearestObj(){
		return obj_near;
	}

	public bool IsColliding(GameObject obj){
		if (obj) {
			if ((controller.transform.position - obj.transform.position).magnitude <= 5f) {
				return true;
			} 
		}
		return false;
	}
}
