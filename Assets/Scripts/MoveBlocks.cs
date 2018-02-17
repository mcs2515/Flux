using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlocks : MonoBehaviour {

	private Vector3 startMarker;
	private Vector3 endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	private int tileLength;
	public GameObject model;

	void Start() {
		tileLength = 5;
		startTime = Time.time;
		startMarker = this.transform.position;
		endMarker = new Vector3 (this.transform.position.x, this.transform.position.y+2.0F, this.transform.position.z);
		journeyLength = Vector3.Distance(startMarker, endMarker);
		SpawnTile ();
	}

	void Update() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
	}

	private void SpawnTile(){
		for (int i = 0; i < tileLength; i++) {
			for (int j = 0; j < tileLength; j++) {
				if (!(i == 0 && j == 0)) {
					GameObject block = Instantiate (model, transform.localPosition + transform.forward *i + transform.right*j, transform.rotation, this.transform) as GameObject;
				}
			}
		}
	}
}
