using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour {

	public float speed = 2.0F;
	private float startTime = 0;
	private Vector3 startMarker;
	private Vector3 endMarker;
	private float journeyLength;
	private Vector3 currentPos;
	bool upwards = true;

	// Use this for initialization
	void Start () {
		startMarker = this.transform.position;
		endMarker = new Vector3 (this.transform.position.x, this.transform.position.y+2.0F, this.transform.position.z);
		journeyLength = Vector3.Distance(startMarker, endMarker);
		currentPos = startMarker;
	}
	
	// Update is called once per frame
	void Update () {
		moveBlock();
		transform.position = currentPos;
	}

	public void assignStart(float t){
		startTime = t;
	}

	private void moveBlock(){
		float distCovered = 0;
		float totalTime = (Vector3.Distance (startMarker, endMarker) / speed); 
		if (Time.time > startTime) {
			distCovered = (Time.time - startTime * speed);
		}
		float fracJourney = distCovered / journeyLength;
		if (upwards) {
			currentPos = Vector3.Lerp (startMarker, endMarker, fracJourney);
			if (Vector3.Distance(currentPos, endMarker) <= 0.1) {
				upwards = false;
				startTime += totalTime;
			}
		} else {
			currentPos = Vector3.Lerp (endMarker,startMarker, fracJourney);
			if (Vector3.Distance(currentPos, startMarker) <= 0.1) {
				upwards = true;
				startTime += totalTime;
			}
		}
	}
}
