using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SeedToPlayerController_multi : Task {

	bool clickLock = false;
	public GameObject rankingCanvas;
	public GameObject dossierCanvas;
	public GameObject guruCanvas;
	public GameObject notMyTurnGuruCanvas;

	public GameController_multi gameController;
	public GameObject canvas;
	public UIFaderScript fader;

	public int nSeedsToPlayers = 0;

	public int pl;

	float remainingTime;

	public CircpleDeploy[] YY;

	public int state = 0;

	public Text answer;

	public RawImage ansBg;

	bool answerShow;

	public void startSeedToPlayerActivity() {
		startSeedToPlayerActivity (this);
	}

	public void startSeedToPlayerActivity(Task w) {
		
		w.isWaitingForTaskToComplete = true;
		waiter = w;

		rankingCanvas.SetActive (false);
		dossierCanvas.SetActive (false);
		//guruCanvas.SetActive (false);
		notMyTurnGuruCanvas.SetActive (false);

		canvas.SetActive (true);
		clickLock = false;
		for (int i = 0; i < YY.Length; ++i) {
			YY [i].setNElements (gameController.nPlayers-1);
			YY [i].Start ();
			YY [i].reset ();
		}
		fader.Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();
		state = 1;

		answer.enabled = false;
		ansBg.enabled = false;
		answerShow = false;
	}

	// Use this for initialization
	void Start () {
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {

		}
		if (state == 1) {
			pl = 0;
			for (int i = 0; i < YY.Length; ++i) {
				if (gameController.playerPresent [i] && (gameController.localPlayerN != i)) {
					YY [i].setIndex (pl++);
					YY [i].extend ();
				}
				
			}
			state = 2;
		}
		if (state == 2) { // waiting for input

		}
		if (state == 3) {
			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f) {
				for (int i = 0; i < YY.Length; ++i)
					YY [i].retract ();
				remainingTime = 0.5f;
				state = 4;
			}
		}
		if (state == 4) {
			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f) {
				fader.fadeOutTask (this);
				state = 5;
			}
		}
		if (state == 5) {
			if (!isWaitingForTaskToComplete) {
				gameController.networkAgent.sendCommand (gameController.playerList[gameController.playerTurn].id, "addplayerseed");
				canvas.SetActive (false);
				notifyFinishTask ();
				state = 0;
			}
		}
	}


	public void clickOnYY(int p) {
		if (clickLock)
			return;
		clickLock = true;
		YY [p].retract ();
		state = 3;
		remainingTime = 0.5f;
		gameController.addNotification (Notification.CONSIGUESEMILLA, gameController.getPlayerName (p), "", "", gameController.getPlayerFemality(p));
		gameController.playerList [p].addSeeds (1);
		gameController.networkAgent.broadcast ("addseed:" + p + ":1:");

	}

	/* event callbacks */
	public void questionMarkClick() {
		if (answerShow) {
			answer.enabled = false;
			ansBg.enabled = false;
			answerShow = false;
		} else {
			answer.enabled = true;
			ansBg.enabled = true;
			answerShow = true;
		}
	}


}
	