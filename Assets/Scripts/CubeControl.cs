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
		Debug.Log (startTime + " assigned");
	}

	void moveBlock(){
		Debug.Log (startTime);
		float distCovered = 0;
		if (Time.time > startTime) {
			distCovered = (Time.time - startTime) * speed;
		}
		float fracJourney = distCovered / journeyLength;
		currentPos = Vector3.Lerp(startMarker, endMarker, fracJourney);
	}
}
