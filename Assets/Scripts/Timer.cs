using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

	public TextMeshProUGUI timerText;
	public TextMeshProUGUI resultTimer;
	int minutes;
	int seconds;
	float timer;

	// Use this for initialization
	void Start () {
		minutes = 0;
		seconds = 0;
		timer = 0f;
	}

	// Update is called once per frame
	void Update () {
		if (GameStateController.Instance.GetGameState () == GameState_e.GAME) {
			timer += Time.deltaTime;

		} else if (GameStateController.Instance.GetGameState () == GameState_e.START) {
			timer = 0;
		}

		minutes = (int)(timer / 60f);
		seconds = (int)(timer % 60f);

		string time = minutes.ToString ("00") + ":" + seconds.ToString ("00");
		timerText.SetText (time);
		resultTimer.SetText (time);

		if (GameStateController.Instance.GetGameState () == GameState_e.RESULT) {
			sendTimerToURL ();
		}
	}

	void sendTimerToURL(){
		//send timer to the url
	}
}
