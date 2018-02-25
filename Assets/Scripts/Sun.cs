using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    int distance = 490;
    GameObject player;

// Use this for initialization
void Start () {
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = (transform.position - player.transform.position).normalized * distance + player.transform.position;
    }
}
