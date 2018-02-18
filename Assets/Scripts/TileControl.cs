using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour {
	
	private int tileLength;
	public GameObject model;

	void Start() {
		tileLength = 5;
		//SpawnTile ();
		for (int i = 0; i < tileLength; i++) {
			for (int j = 0; j < tileLength; j++) {
				GameObject block;
				if (i == 0 && j == 0) {
					GameObject.Find ("singleCube").AddComponent<CubeControl> ();
				}
				else {
					block = Instantiate (model, transform.localPosition + transform.forward *i + transform.right*j,
						transform.rotation, this.transform) as GameObject;
					
					block.GetComponent<CubeControl> ().assignStart (((float)j)*0.2F);
				}
			}
		}
	}

	void Update() {
	}

	private void SpawnTile(){
		
	}
}
