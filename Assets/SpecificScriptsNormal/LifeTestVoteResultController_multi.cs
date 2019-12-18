using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ScalerAuxArray {
	public UIScaleFader[] scaler;
}

public class LifeTestVoteResultController_multi : Task {

	public ScalerAuxArray[] scalers;

	public GameController_multi gameController;

	public UIFaderScript fader;

	public UITextFader hasConseguidoScaler;
	public UITextFader semillasScaler;


	List<int> votesReceived;
	List<int> fromPlayer; // we should make a class or structure for this....

	public void startLifeTestVoteResultController_multi(Task w) {
		fader.Start ();
		fader.setFadeValue (1.0f);
		waiter = w;
		w.isWaitingForTaskToComplete = true;
		votesReceived = new List<int> ();
		fromPlayer = new List<int> ();
		foreach (ScalerAuxArray a in scalers) {
			foreach (UIScaleFader s in a.scaler) {
				s.Start ();
				s.scaleOutImmediately ();
			}
		}
		fader.fadeIn ();
		semillasScaler.Start ();
		hasConseguidoScaler.Start ();
		semillasScaler.fadeOutImmediately ();
		hasConseguidoScaler.fadeOutImmediately ();
		currentVoteOrb = 0;
		state = -1;
	}

	int currentVoteOrb = 0;

	int score;

	// Network callbacks
	public void receiveVote(int fp, int value) {
		Debug.Log ("Vote received: " + value);
		votesReceived.Add (value);
		fromPlayer.Add (fp);
		scalers [currentVoteOrb].scaler [value-1].scaleIn ();
		++currentVoteOrb;
		if (gameController.nPlayers == 3) {
			currentVoteOrb = 2;
		} 

		if (votesReceived.Count == (gameController.nPlayers - 1)) {
			float acc = 0f;
			// find out minimum score
			int minimum = 3;
			for (int i = 0; i < votesReceived.Count; ++i) {
				if (votesReceived [i] < minimum)
					minimum = votesReceived [i];
			}
			int uniqueness = 0;
			int uniqueIndex = 0;
			for (int i = 0; i < votesReceived.Count; ++i) {
				if (votesReceived [i] == minimum) {
					++uniqueness;
					uniqueIndex = fromPlayer[i];
				}
			}
			if ((uniqueness == 1) && (gameController.nPlayers > 2)) {
				gameController.networkAgent.sendCommand (uniqueIndex, "looseseed:");
				gameController.addNotification (Notification.PIERDESEMILLA, gameController.getPlayerName (uniqueIndex), 
					"", "", gameController.getPlayerFemality(uniqueIndex));
				gameController.playerList [uniqueIndex].addSeeds (-1);
				gameController.networkAgent.broadcast ("addseeds:" + uniqueIndex + ":-1:");
			}
			for (int i = 0; i < votesReceived.Count; ++i) {
				acc += votesReceived [i];
			}
			score = (int)(acc / ((float)votesReceived.Count));
			remainingTime = 1.0f;
			Debug.Log ("<color=blue>Score: " + score + "</color>");
			if (score > 1) {
				gameController.addNotification (Notification.CONSIGUESEMILLAS, gameController.getPlayerName (gameController.localPlayerN), 
					"" + score, "", gameController.getPlayerFemality());
				
			} else {
				gameController.addNotification (Notification.CONSIGUESEMILLA, gameController.getPlayerName (gameController.localPlayerN), 
					"1", "", gameController.getPlayerFemality());
			}
			gameController.playerList [gameController.localPlayerN].addSeeds (score);
			gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":" + score + ":");
			scalers [3].scaler [score-1].scaleIn ();
			state = 1;

		}
	}

	float remainingTime;
	int state = 0;
	void Update() {
		if (state == -1) {
			foreach (ScalerAuxArray a in scalers) {
				foreach (UIScaleFader s in a.scaler) {
					s.Start ();
					s.scaleOutImmediately ();
				}
			}
			state = 0;
		}
		if (state == 0) { // idling

		}
		if (state == 1) { // delay 1
			remainingTime -= Time.deltaTime;
			if (remainingTime < 0f) {
				remainingTime = 0.5f;
				hasConseguidoScaler.fadeIn ();
				state = 2;
			}
		}
		if (state == 2) { // delay 0.5
			remainingTime -= Time.deltaTime;
			if (remainingTime < 0f) {
				remainingTime = 5f;
				semillasScaler.fadeIn ();
				state = 3;
			}
		}
		if (state == 3) { // delay 5f;
			remainingTime -= Time.deltaTime;
			if (Input.GetMouseButtonDown (0)) {
				remainingTime = 0f;
			}
			if (remainingTime < 0f) {
				gameController.networkAgent.broadcast ("endlifetestvote:");
				fader.fadeOutTask (this);
				state = 4;
			}
		}
		if (state == 4) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				state = 0;
				notifyFinishTask ();
			}
		}
	}

}
	