using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshControl : MonoBehaviour {

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices, currentVertices;
	bool[] upwardBound;

	public Material gridTex;
	public Material glowTex;

	float speed;
	private Texture2D controllerImg;
	public Texture2D[] transIn;
	public Texture2D[] jumpNoise;
	private GameObject[] extras;
	private int controllerI;
	private float glowTexOffset;
	bool transitioning;
    Object[] transInObjs, jumpNoiseObjs;
    Vector3 hideFire;
	bool start = false;

	// Use this for initialization
	void Start ()
    {
        //assign all variables
        glowTexOffset = 0.016129f;

        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        currentVertices = new Vector3[originalVertices.Length];
        extras = new GameObject[Mathf.RoundToInt((float)originalVertices.Length)];

        transInObjs = Resources.LoadAll("jumpTransition/", typeof(Texture2D));
        jumpNoiseObjs = Resources.LoadAll("jumpNoise/", typeof(Texture2D));

        int j = 0;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            extras[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Destroy(extras[i].GetComponent<Collider>());

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(extras[i].transform);
            float sphereRad = 0.2f;
            sphere.transform.localScale = new Vector3(sphereRad, sphereRad, sphereRad);
            sphere.GetComponent<Renderer>().material = gridTex;
            Destroy(sphere.GetComponent<Collider>());

            extras[i].transform.SetParent(transform);
            extras[i].transform.Rotate(90, 0, 0);
            //extras [i].transform.localScale = new Vector3 (0, 0, 4f);
            //float sphereRad = 0.03f;
            float extraRad = 0.1f;
            extras[i].transform.localScale = new Vector3(extraRad, extraRad, extraRad);

            extras[i].GetComponent<Renderer>().material = glowTex;
            j++;

            //float texScale = 0.5f;
            //extras [i].GetComponent<Renderer> ().material.SetTextureScale ("_MainTex", new Vector2(1f, 0.1f));
        }

        Restart();
    }
	
	// Update is called once per frame
	void Update () {
		if (start) {
            if (GameStateController.Instance.GetGameState() == GameState_e.GAME) {

                int j = 0;
			    for (int i = 0; i < extras.Length; i++) {
					extras [i].transform.position = transform.position + transform.localScale.x * currentVertices [j] + new Vector3 (0, -2f, 0);
					extras [i].GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (glowTexOffset, 0));
					j++;
                    //glowTexOffset = (glowTexOffset - 0.07f) % 1;
                }
				if (transitioning) {
                    for (int i = 0; i < displacedVertices.Length; i++)
                    {
                        UpdateVertexColor(transIn, i);
                        extras[i].transform.position += hideFire;
                        extras[i].transform.Find("Sphere").transform.localPosition = new Vector3(0f, 0 - hideFire.y, -2.7f);
                    }
					controllerI = (controllerI + 1);
                    if(hideFire.y < 0) {
                        hideFire += new Vector3(0, 0.12f, 0);
                    }
                    
				} else {
					for (int i = 0; i < displacedVertices.Length; i++) {
						UpdateVertexColor (jumpNoise, i);
					}
					controllerI = (controllerI + 1) % jumpNoise.Length;
				}
				deformingMesh.vertices = currentVertices;
				deformingMesh.RecalculateNormals ();

				if (controllerI == transIn.Length) {
					transitioning = false;
					controllerI = 0;
				}
			}
		}else {
			CheckStart ();
            //start = true;
		}
	}

    void Restart()
    {
        transitioning = true;

        
        transIn = new Texture2D[transInObjs.Length];
        for (int i = 0; i < transInObjs.Length; i++)
        {
            transIn[i] = (Texture2D)transInObjs[i];
        }
        jumpNoise = new Texture2D[jumpNoiseObjs.Length];
        for (int i = 0; i < jumpNoiseObjs.Length; i++)
        {
            jumpNoise[i] = (Texture2D)jumpNoiseObjs[i];
        }
        controllerI = 0;


        int j = 0;
        for (int i = 0; i < originalVertices.Length; i++)
        {
            extras[i].transform.position = transform.position + originalVertices[j] + hideFire;
            extras[i].transform.Find("Sphere").transform.localPosition += new Vector3(0, 0, -2.7f);

        }

        hideFire = new Vector3(0f, -6f, 0f);

    }

	void UpdateVertexColor(Texture2D[] controllerImgs, int i){
		float tileL = Mathf.Round(Mathf.Sqrt (originalVertices.Length/2));
		float tileW = tileL*2 - 1;
		controllerImg = controllerImgs [controllerI];
		//Debug.Log (controllerImg.width + " " + controllerImg.height);
		int x = Mathf.RoundToInt ((float)i % tileW / (tileW - 1) * controllerImg.width);
		int y = Mathf.RoundToInt (i / 20.909f / tileL * controllerImg.height);
		float colorVal = controllerImg.GetPixel (x, y).grayscale;
		//Debug.Log(i + " x: " + x + " y: " + y + " " + controllerImg.GetPixel (x, y));
		currentVertices[i] = originalVertices[i] + new Vector3(0, 1*colorVal, 0);
	}

	void CheckStart(){
		if (Vector3.Distance(GameObject.Find ("Player").transform.position, transform.position) < 80.0f) {
			start = true;
		}
	}
}
