using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshControl : MonoBehaviour {

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices, currentVertices;
	float[] startTimes;
	bool[] upwardBound;
	float speed;
	private Texture2D controllerImg;
	public Texture2D[] controllerImgs;
	private int controllerI;

	// Use this for initialization
	void Start () {
		//assign all variables
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		currentVertices = new Vector3[originalVertices.Length];
		/*startTimes = new float[originalVertices.Length];
		upwardBound = new bool[originalVertices.Length];
		speed = 1.0f;*/

		//for (int i = 0; i < originalVertices.Length; i++) {
		//	displacedVertices[i] = originalVertices[i] + new Vector3(0, 1.0f, 0);
		//	if (originalVertices [i].y == 0) {
		//		displacedVertices [i] = originalVertices [i];
		//	}
		//
		//	float tileLength = Mathf.Sqrt (originalVertices.Length);
		//	offset start times for each vertice
		//	startTimes[i] = controllerImg.GetPixel (Mathf.RoundToInt((float)i%tileLength/tileLength*controllerImg.width), Mathf.RoundToInt(Mathf.Floor((float)i/tileLength)/tileLength*controllerImg.height)).grayscale;
		//	upwardBound [i] = true;
		//}

		Object[] controllerImgObjs = Resources.LoadAll("Noise/", typeof(Texture2D));
		controllerImgs = new Texture2D[controllerImgObjs.Length];
		for (int i = 0; i < controllerImgObjs.Length; i++) {
			controllerImgs [i] = (Texture2D) controllerImgObjs [i];
		}
		controllerI = 0;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertexColor(i);
		}
		deformingMesh.vertices = currentVertices;
		deformingMesh.RecalculateNormals();
		controllerI = (controllerI + 1) % controllerImgs.Length;
	}

	/*void UpdateVertex (int i) {
		if (displacedVertices [i].y == 0) {
			currentVertices [i] = displacedVertices [i];
			return;
		}

		float distCovered = 0;
		float journeyLength = Vector3.Distance (originalVertices [i], displacedVertices [i]);
		float totalTime = (Vector3.Distance (originalVertices[i], displacedVertices[i]) / speed); 
		if (Time.time > startTimes[i]) {
			distCovered = (Time.time - startTimes[i] * speed);
		}
		float fracJourney = distCovered / journeyLength;
		if (upwardBound[i]) {
			currentVertices[i] = Vector3.Lerp (originalVertices[i], displacedVertices[i], fracJourney);
			if (Vector3.Distance(currentVertices[i], displacedVertices[i]) <= 0.1) {
				upwardBound[i] = false;
				startTimes[i] += totalTime;
			}
		} else {
			currentVertices[i] = Vector3.Lerp (displacedVertices[i],originalVertices[i], fracJourney);
			if (Vector3.Distance(currentVertices[i], originalVertices[i]) <= 0.1) {
				upwardBound[i] = true;
				startTimes[i] += totalTime;
			}
		}
	}*/

	void UpdateVertexColor(int i){
		float tileLength = Mathf.Sqrt (originalVertices.Length);
		controllerImg = controllerImgs [controllerI];
		float colorVal = controllerImg.GetPixel (Mathf.RoundToInt ((float)i % tileLength / tileLength * controllerImg.width), Mathf.RoundToInt (Mathf.Floor ((float)i / tileLength) / tileLength * controllerImg.height)).grayscale;
		currentVertices[i] = originalVertices[i] + new Vector3(0, 5*colorVal, 0);
		Debug.Log (currentVertices [i]);
	}
}
