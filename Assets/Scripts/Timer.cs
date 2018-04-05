using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

	public TextMeshProUGUI timerText;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (GameStateController.Instance.GetGameState () == GameState_e.GAME) {
			var minutes = (int)(Time.time / 60f);
			var seconds = (int)(Time.time % 60f);
			string time = minutes.ToString ("00") + ":" + seconds.ToString ("00");
			timerText.SetText (time);
		}
	}
}
