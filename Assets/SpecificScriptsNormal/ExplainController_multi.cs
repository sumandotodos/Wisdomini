using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplainController_multi : Task {

	public UIFaderScript fader;

	public GameObject colorScroll;
	public GameObject bwScroll;
	public GameObject explainCanvas;

	int state = 0;

	bool isTurnPlayer;

	public void startExplanation(Task w, bool isTurn) {
		explainCanvas.SetActive (true);
		fader.Start ();
		fader.setFadeValue(1.0f);
		waiter = w;
		w.isWaitingForTaskToComplete = true;
		colorScroll.SetActive (isTurn);
		bwScroll.SetActive (!isTurn);
		fader.fadeIn ();
		isTurnPlayer = isTurn;
		state = 1;
		remaining = 20.0f;
		dismiss = false;
	}

	float remaining;
	bool dismiss;

	// Update is called once per frame
	void Update () {
		if (state == 0) {

		}

		if (state == 1) {
			remaining -= Time.deltaTime;
			if ((Input.GetMouseButtonDown (0) || remaining < 0)) {
				state = 2;
				fader.fadeOutTask (this);
			} 
		}

		if (state == 2) {
			if (!isWaitingForTaskToComplete) {
				explainCanvas.SetActive (false);
				notifyFinishTask ();
				state = 0;
			}
		}
	}

	// network callback

	public void dismissExplain() {
		dismiss = true;
	}
}
	