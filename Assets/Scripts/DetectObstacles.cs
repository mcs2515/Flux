using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacles : MonoBehaviour {

	public GameObject jump_sprite;
	public GameObject[] obstacles;
	public GameObject damage_screen;
	public Material skybox;
	//Color skyDayColor = new Color(.234f, .297331f, .328f, 1);
	Color skyDayColor = new Color(60f/255f, 76f/255f, 84f/255f, 1);
	Color skyDmgColor = new Color(189f/255f, 57f/255f, 60f/255f, 1);


	GameObject obj_near;
	public PlayerMove playerScript;
	float timer;
	int counter;

	public CharacterController controller;
	// Use this for initialization
	void Start () {
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

		skybox = RenderSettings.skybox;
		skybox.SetColor("_Tint", skyDayColor);
		RenderSettings.skybox = skybox;

		jump_sprite.SetActive (false);
		//damage_screen.SetActive (false);
		timer = 0;
		counter = 0;
	}

	// Update is called once per frame
	void Update () {
		if (GameStateController.Instance.GetGameState () == GameState_e.GAME) {
			obj_near = FindNearestObj ();
			Display_Jump (obj_near);

			//if colliding
			if (IsColliding (obj_near)) {

				timer += Time.deltaTime;

				if (counter == 0) {
					playerScript.Lives--;
					skybox.SetColor("_Tint", Color.Lerp(skyDayColor, skyDmgColor,  1f));
					RenderSettings.skybox = skybox;

					counter++;
				}

				/*if (timer >= .5f) {
					//damage_screen.SetActive (false);
					skybox.SetColor("_Tint", skyDayColor);
					RenderSettings.skybox = skybox;
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
				skybox.SetColor("_Tint", skyDayColor);
				RenderSettings.skybox = skybox;
				//damage_screen.SetActive (false);
			}
		} 
		else {
			jump_sprite.SetActive (false);
			skybox.SetColor("_Tint",skyDayColor);
			RenderSettings.skybox = skybox;
			//damage_screen.SetActive (false);
		}

		if (GameStateController.Instance.GetGameState () == GameState_e.START) {
			timer = 0;
			counter = 0;
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

	public void Display_Jump(GameObject obj){
		if (obj) {
			if ((controller.transform.position - obj.transform.position).magnitude <= 25f) {
				jump_sprite.SetActive (true);
			} 
			else {
				jump_sprite.SetActive (false);
			}
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
