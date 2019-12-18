using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GompaLiteController_mono : Task {

	public GameController_mono gameController;

	public UITextFader goBackToStart;
	public UIFaderScript fader;

	public void startGompaTask(Task w) {

		goBackToStart.Start ();
		goBackToStart.fadeOutImmediately ();
		w.isWaitingForTaskToComplete = true;
		waiter = w;

//		int immunityRemain = (gameController.playerList [gameController.localPlayerN].seeds) / 7 - gameController.playerList [gameController.localPlayerN].spentImmunity;
//		if (immunityRemain < 0)
//			immunityRemain = 0;
//		if (immunityRemain > 0) {
//			gameController.playerActivityController.obtainedImmunity = 1;
//			gameController.playerList [gameController.localPlayerN].spentImmunity++;
//			notifyFinishTask ();
//		} else {
//			gameController.playerActivityController.obtainedImmunity = 0;
//			fader.fadeInTask (this);
//			state = 1;
//		}

		fader.fadeInTask(this);
		state = 1;

	}

	// modelo de lonchas
	int state = 0;
	float remaining = 0;
	void Update() {
		if (state == 0) {

		}
		if (state == 1) {
			if (!isWaitingForTaskToComplete) {
				goBackToStart.fadeIn ();
				remaining = 5.0f;
				state = 2;
			}
		}
		if (state == 2) {
			remaining -= Time.deltaTime;
			if ((remaining < 0f) || Input.GetMouseButtonDown (0)) {
				fader.fadeOutTask (this);
				gameController.playerList[gameController.localPlayerN].addSeeds(1);

				gameController.addNotification (Notification.CONSIGUESEMILLA, gameController.getPlayerName (gameController.localPlayerN), "1", "", gameController.getPlayerFemality());
				state = 3;
			}
		}
		if (state == 3) {
			if (!isWaitingForTaskToComplete) {
				state = 0;
				notifyFinishTask ();
			}
		}
	}
}
