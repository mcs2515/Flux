using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControllers : MonoBehaviour {

	static VideoControllers instance;

	//public VideoPlayer videoPlayer;
	public VideoPlayer countdown_player;
	public VideoPlayer jump_player;
	public VideoPlayer warning_player;

	// Use this for initialization
	void Start () {
		countdown_player.playOnAwake = false;
		countdown_player.source = VideoSource.VideoClip;

		/*jump_player.playOnAwake = false;
		jump_player.source = VideoSource.VideoClip;

		warning_player.playOnAwake = false;
		warning_player.source = VideoSource.VideoClip;*/

		countdown_player.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public void PlayClips(string clip){
		switch (clip) {
		case "countdown":
			countdown_player.Play ();
			break;
		case "jump":
			jump_player.Play ();
			break;
		case "warning":
			warning_player.Play ();
			break;
		case "":
		case "none":
		case null:
			countdown_player.Stop ();
			//jump_player.Stop ();
			//warning_player.Stop ();
			break;
		}
	}
}
