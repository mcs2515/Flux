using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGifs : MonoBehaviour {

	public GameObject jump_sprite;
	public GameObject[] obstacles;
	GameObject obj_near;

	public CharacterController controller;
	// Use this for initialization
	void Start () {
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		jump_sprite.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (GameStateController.Instance.GetGameState () == GameState_e.GAME) {
			obj_near = FindNearestObj ();
			Display_Jump (obj_near);
		} else {
			jump_sprite.SetActive (false);
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
}
