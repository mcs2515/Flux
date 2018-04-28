using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectObstacles : MonoBehaviour {

	public GameObject jump_sprite;
	public GameObject warning_sprite;

	public GameObject[] tile_markers;
	public GameObject[] tile_group;
	public GameObject[] hide_markers;

	int tile_num;
	int tile_counter;
	int hide_num;
	int hide_counter;

	public GameObject[] obstacles;
	public GameObject damage_screen;
	public Material skybox;
	//Color skyDayColor = new Color(.234f, .297331f, .328f, 1);
	Color skyDayColor = new Color(91f/255f, 89f/255f, 91f/255f, 1);
	Color skyDmgColor = new Color(197f/255f, 23f/255f, 30f/255f, 1);

	Color32 lives_white = new Color32 (255,255,255,128);
	Color32 lives_red = new Color32 (239, 65, 65, 255);

	GameObject obj_near;
	GameObject marker_near;
	GameObject hide_near;

	public PlayerMove playerScript;
	float timer;
	int counter;
	float skyLerp;

	public CharacterController controller;
	public Lives lives;

	// Use this for initialization
	void Start () {
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		tile_markers = GameObject.FindGameObjectsWithTag("marker");
		hide_markers = GameObject.FindGameObjectsWithTag("hide");

		tile_num = 0;
		hide_counter = 0;
		hide_num = 0;
		tile_counter = 0;
		DisplayTiles ();

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
			marker_near = FindNearestMarker ();
			hide_near = FindNearestHideMarker ();
			Display_Sprites (obj_near);

			Debug.Log ("tile num: " + tile_num);
			if (IsCollidingWithMarker (marker_near)) {
				if (tile_counter == 0) {
					tile_num++;
					tile_counter++;
					DisplayTiles ();
				}
			} else {
				tile_counter = 0;
			}

			Debug.Log ("hide num: " + hide_num);
			if (IsCollidingWithMarker (hide_near)) {
				if (hide_counter == 0) {
					hide_num++;
					hide_counter++;
					HideTiles ();
				}
			} else {
				hide_counter = 0;
			}

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
			tile_num = 0;
			hide_num = 0;
			DisplayTiles ();
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

	public GameObject FindNearestMarker(){
		foreach(GameObject marker in tile_markers) {
			if ((controller.transform.position - marker.transform.position).magnitude <= 30f) {
				return marker;
			}
		}
		return null;
	}

	public GameObject FindNearestHideMarker(){
		foreach(GameObject marker in hide_markers) {
			if ((controller.transform.position - marker.transform.position).magnitude <= 30f) {
				return marker;
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

	public bool IsCollidingWithMarker(GameObject obj){
		if (obj) {
			if ((controller.transform.position - obj.transform.position).magnitude <= 10f) {
				return true;
			} 
		}
		return false;
	}

	public void DisplayTiles(){
		switch (tile_num) {
		case 0:
			tile_group [0].SetActive (true);
			tile_group [1].SetActive (false);
			tile_group [2].SetActive (false);
			break;
		case 1:
			tile_group [1].SetActive (true);
			break;
		case 2:
			tile_group [2].SetActive (true);

			break;
		}
	}

	public void HideTiles(){
		switch (hide_num) {
		case 1:
			tile_group [0].SetActive (false);
			break;
		case 2:
			tile_group [1].SetActive (false);
			break;
		}
	}
}
