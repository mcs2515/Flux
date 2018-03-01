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
	public Texture2D[] transIn;
	public Texture2D[] jumpNoise;
	private GameObject[] extras;
	private int controllerI;
	private float sphereTexOffset;
	bool transitioning;

	// Use this for initialization
	void Start () {
		//assign all variables
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		currentVertices = new Vector3[originalVertices.Length];
		extras = new GameObject[Mathf.RoundToInt((float)originalVertices.Length/4)];
		transitioning = true;
		sphereTexOffset = 0.016129f;

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

		Object[] transInObjs = Resources.LoadAll("jumpTransition/", typeof(Texture2D));
		Object[] jumpNoiseObjs = Resources.LoadAll("jumpNoise/", typeof(Texture2D));
		transIn = new Texture2D[transInObjs.Length];
		for (int i = 0; i < transInObjs.Length; i++) {
			transIn [i] = (Texture2D) transInObjs [i];
		}
		jumpNoise = new Texture2D[jumpNoiseObjs.Length];
		for (int i = 0; i < jumpNoiseObjs.Length; i++) {
			jumpNoise [i] = (Texture2D) jumpNoiseObjs [i];
		}
		controllerI = 0;


		int j = 0;
		for (int i = 0; i < originalVertices.Length; i++) {
			extras [i] = GameObject.CreatePrimitive(PrimitiveType.Plane);

			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.transform.SetParent (extras[i].transform);
			float sphereRad = 0.3f;
			sphere.transform.localScale = new Vector3 (sphereRad, sphereRad, sphereRad);
			sphere.GetComponent<Renderer> ().material = GameObject.Find("gridTexHolder").GetComponent<Renderer>().material;

			extras [i].transform.SetParent(transform);
			extras [i].transform.position = transform.position + originalVertices[j] + new Vector3(0,10,0);
			extras [i].transform.Rotate(-90,0,0);
			//float sphereRad = 0.03f;
			float extraRad = 0.1f;
			extras [i].transform.localScale = new Vector3 (extraRad, extraRad, extraRad);

			extras [i].GetComponent<Renderer> ().material = GameObject.Find("glowTexHolder").GetComponent<Renderer>().material;
			j += 4;

			float texScale = 0.5f;
			extras [i].GetComponent<Renderer> ().material.SetTextureScale ("_MainTex", new Vector2(1f, 0.1f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transitioning) {
			for (int i = 0; i < displacedVertices.Length; i++) {
				UpdateVertexColor (transIn, i);
			}
			controllerI = (controllerI + 1);
		} else {
			for (int i = 0; i < displacedVertices.Length; i++) {
				UpdateVertexColor (jumpNoise, i);
			}
			controllerI = (controllerI + 1) % jumpNoise.Length;
		}
		deformingMesh.vertices = currentVertices;
		deformingMesh.RecalculateNormals();

		if (controllerI == transIn.Length) {
			transitioning = false;
			controllerI = 0;
		}

		int j = 0;
		for (int i = 0; i < extras.Length; i++) {
			extras [i].transform.position = transform.position + transform.localScale.x*currentVertices [j];
			extras [i].GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2(sphereTexOffset,0));
			j += 4;
			sphereTexOffset = (sphereTexOffset - 0.032258f) % 1;
		}
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

	void UpdateVertexColor(Texture2D[] controllerImgs, int i){
		float tileLength = Mathf.Sqrt (originalVertices.Length);
		controllerImg = controllerImgs [controllerI];
		float colorVal = controllerImg.GetPixel (Mathf.RoundToInt ((float)i % tileLength / tileLength * controllerImg.width), Mathf.RoundToInt (Mathf.Floor ((float)i / tileLength) / tileLength * controllerImg.height)).grayscale;
		currentVertices[i] = originalVertices[i] + new Vector3(0, 1*colorVal, 0);
	}
}
