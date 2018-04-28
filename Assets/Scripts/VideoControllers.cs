using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControllers : MonoBehaviour {

	static VideoControllers instance;

	//public VideoPlayer videoPlayer;
	public VideoPlayer countdown_player;

	// Use this for initialization
	void Start () {
		//countdown_player.playOnAwake = false;
		countdown_player.source = VideoSource.VideoClip;
		countdown_player.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameStateController.Instance.GetGameState() == GameState_e.START){
			countdown_player.Prepare();
		}
	}

	public static VideoControllers Instance{
		get{
			if (instance == null) {
				instance = VideoControllers.FindObjectOfType<VideoControllers> ();
			}
			return VideoControllers.instance; 
		}
		set{ VideoControllers.instance = value;}
	}

	public void PlayCountdown(bool play){

		if (play) {
			countdown_player.Play ();
		} else {
			countdown_player.Stop ();
		}
	}
}
