using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour {
	
	private int tileLength;
	public GameObject model;
	public Texture2D controllerImg;

	void Start() {
		tileLength = 40;
		SpawnTile ();
	}

	void Update() {
	}

	private void SpawnTile(){
		for (float i = 0; i < tileLength; i++) {
			for (float j = 0; j < tileLength; j++) {
				GameObject block;
				if (i == 0 && j == 0) {
					block = GameObject.Find ("singleCube");
					block.AddComponent<CubeControl> ();
				}
				else {
					block = Instantiate (model, transform.localPosition + transform.forward *i + transform.right*j,
						transform.rotation, this.transform) as GameObject;
				}

				//assign start time
				float startTime = controllerImg.GetPixel (Mathf.RoundToInt(i/tileLength*controllerImg.width), Mathf.RoundToInt(j/tileLength*controllerImg.height)).grayscale;
				block.GetComponent<CubeControl> ().assignStart (startTime);
			}
		}
	}
}
