using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreServer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Upload ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator Upload(){
		//byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
		UnityWebRequest www = UnityWebRequest.Get("http://serenity.ist.rit.edu/~amp4129/341/flux/scores.php?i=2&score=321");
		yield return www.Send();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Upload complete!");
		}
	}
}
