using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {
	public Animator anim;
	private int counter;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		

		if (GameStateController.Instance.GetGameState () == GameState_e.START) {
			anim.Rebind ();
		}

		if(GameStateController.Instance.GetGameState () == GameState_e.GAME){
			CheckStart ();
		}
	}

	void CheckStart(){
		if (Vector3.Distance(GameObject.Find ("Player").transform.position, transform.position) < 110.0f) {
			anim.SetTrigger ("startAnim");
		}

	}
}
