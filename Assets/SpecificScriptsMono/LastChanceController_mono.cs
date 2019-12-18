using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastChanceController_mono : Task {
	
	public GameController_mono gameController;
	public MasterController_mono masterController;
	public GameObject LastChanceCanvas;
	public LifeTestActivityController_mono lifeTestController;

	public AudioClip evilLaughClip;

	public UIScaleFader defeatSkullScaler;
	public UIScaleFader victoryScaler;

	public UIScaleFader yinYangScaler;

	public LifeTestActivityController_mono parentController;

	int result;

	public const int DEFEAT = 0;
	public const int REDUSED = 1;
	public const int GREENUSED = 2;

	public UIFaderScript fader;

	public const float SmallDelay = 0.25f;
	float remainingTime;

	public UIFaderScript [] orbFader;

	public CircpleDeploy[] playersDeploy;

	public UIScaleFader [] orbScaler;

	bool clickOnSphereLock = false;
	bool playerLock = false;

	public UITextFader mainInfo;
	public UITextFader savedInfo;
	public UITextFader defeatedInfo;
	public UITextFader chooseInfo;

	int state = 0;

	int index;

	// Use this for initialization
	void Start () {
		state = 0;
		LastChanceCanvas.SetActive (false);
	}

	public void startLastChance(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		mainInfo.Start ();
		defeatedInfo.Start ();
		savedInfo.Start ();
		chooseInfo.Start ();

		defeatSkullScaler.Start ();
		defeatSkullScaler.scaleOutImmediately ();
		victoryScaler.Start ();
		victoryScaler.scaleOutImmediately ();

		mainInfo.fadeInImmediately ();
		savedInfo.fadeOutImmediately ();
		defeatedInfo.fadeOutImmediately ();
		chooseInfo.fadeOutImmediately ();

		clickOnSphereLock = false;
		playerLock = false;
		LastChanceCanvas.SetActive (true);
		fader.Start ();
		yinYangScaler.Start ();
		yinYangScaler.scaleInImmediately ();
		fader.setFadeValue (1.0f);
		for (int i = 0; i < playersDeploy.Length; ++i) {
			playersDeploy [i].Start ();
			playersDeploy [i].reset ();
		}

		for (int i = 0; i < orbFader.Length; ++i) {
			if (!gameController.playerList [gameController.localPlayerN].orbUsed [i]) {
				orbFader [i].Start ();
				orbFader [i].setFadeValue (0.0f);
				orbScaler [i].Start ();
				orbScaler [i].scaleInImmediately ();
			}
		}
		state = 1000;
		remainingTime = SmallDelay;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == 0) { // stopped

		}

		if (state == 1000) {
			state = 1;
			return;
		}

		if (state == 1) {
			
			fader.fadeInTask (this);
			state = 2;
		}

		if (state == 2) {
			if (!isWaitingForTaskToComplete) {
				state = 3;
				index = 0;
				mainInfo.fadeIn ();
				remainingTime = SmallDelay;
			}
		}
		if (state == 3) {
			remainingTime -= Time.deltaTime;
			if (remainingTime < 0.0f) {
				if (!gameController.playerList [gameController.localPlayerN].orbUsed [index]) {
					//orbScaler [index].scaleIn ();
					orbFader[index].fadeOut();
				}
				++index;
				remainingTime = SmallDelay;
				if (index == orbFader.Length)
					state = 4;
			}
		}
		if (state == 4) { // awaiting orb press

		}
		if (state == 5) {
			remainingTime = 1.0f;
			state = 6;
		}
		if (state == 6) { // wait a little bit
			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f) {
				state = 7;
				savedInfo.fadeIn ();
			}
		}
		if (state == 7) { // take out main label
			mainInfo.fadeOutTask (this);
			state = 8;
		}
		if (state == 8) { // you have been saved!
			if (!isWaitingForTaskToComplete) {
				savedInfo.fadeIn ();
				remainingTime = 6.0f;
				state = 9;
			}
		}
		if (state == 9) {
			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f) {
				state = 100;
			}
			if (Input.GetMouseButtonDown (0))
				remainingTime = 0.0f;
		}

		if (state == 100) {
			fader.fadeOutTask (this);
			state = 101;
		}
		if (state == 101) {
			if (!isWaitingForTaskToComplete) {
				LastChanceCanvas.SetActive (false);
				notifyFinishTask ();
				state = 0;
			}
		}

		if (state == 200) { // showing skull
			mainInfo.fadeOut();
			chooseInfo.fadeOut ();
			defeatedInfo.fadeIn ();
			defeatSkullScaler.scaleIn ();
			masterController.playSound (evilLaughClip);
			state = 201;
			remainingTime = 5.0f;
		}
		if (state == 201) {
			remainingTime -= Time.deltaTime;
			if (Input.GetMouseButtonDown (0))
				remainingTime = 0.0f;
			if (remainingTime < 0.0f) {
				state = 100; // fadeout and finish
			}
		}

		if (state == 250) { // showing victory
			mainInfo.fadeOut();
			//defeatedInfo.fadeIn ();
			victoryScaler.scaleIn ();

			state = 251;
			remainingTime = 5.0f;
		}
		if (state == 251) {
			remainingTime -= Time.deltaTime;
			if (Input.GetMouseButtonDown (0))
				remainingTime = 0.0f;
			if (remainingTime < 0.0f) {
				state = 100; // fadeout and finish
			}
		}


		if (state == 300) {
			mainInfo.fadeOut ();
			chooseInfo.fadeIn ();
			for (int i = 0; i < orbFader.Length; ++i)
				orbFader [i].fadeIn ();
			yinYangScaler.scaleOut ();
			List<int> available = new List<int>();
			for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
				if (gameController.playerPresent [i] && (i != gameController.localPlayerN)) {
					available.Add (i);
				}
			}
			state = 303;
			int randomP = Random.Range (0, available.Count);

//			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
//				playersDeploy [i].setNElements (gameController.nPlayers - 1);
//				if (gameController.playerPresent [i] && (i != gameController.localPlayerN)) {
//					playersDeploy [i].extend ();
//				}
//			}
//			remainingTime = 5.5f;
//			state = 301;
		}
//		if (state == 301) { //wait
//
//		}
//		if (state == 302) {
//			remainingTime -= Time.deltaTime;
//			if (remainingTime <= 0.0f) {
//				for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
//					if (gameController.playerPresent [i] && (i != gameController.localPlayerN)) {
//						playersDeploy [i].retract ();
//					}
//				}
//			}
//			state = 303;
//		}
		if (state == 303) {
			// wait for victory or defeat!!
		}		
	}


	// UI callbacks
	public void clickOnSphere(int id) {
		if (clickOnSphereLock)
			return;
		if (id < 2) { // red sphere
			mainInfo.fadeOut();
			parentController.lastChanceResult = REDUSED;
			state = 5;
		} else { // green sphere
			mainInfo.fadeOut();
			parentController.lastChanceResult = GREENUSED;
			state = 300;
		}
		gameController.playerList [gameController.localPlayerN].orbUsed [id] = true;
		orbScaler [id].scaleOut ();
		clickOnSphereLock = true;
	}

	// UI callbacks
	public void clickOnYinYang() {
		if (clickOnSphereLock)
			return;
		clickOnSphereLock = true;
		parentController.lastChanceResult = DEFEAT;
		gameController.clearGold(gameController.localPlayerN);

		gameController.addNotification (Notification.PIERDEOROS, gameController.getPlayerName (gameController.localPlayerN),
			"", "", gameController.getPlayerFemality());
		state = 200; // showing skull

	}

	public void clickOnPlayer(int p) {
		if (playerLock)
			return;
		playerLock = true;
		playersDeploy [p].retract();

		remainingTime = 2.0f;
		state = 302;
	}

	// network callbacks
	public void summonRes(int r) {
		if (r == 1) { // victory
			state = 250;
		} else { // defeat
			state = 200;
			gameController.clearGold(gameController.localPlayerN);

			gameController.addNotification (Notification.PIERDEOROS, gameController.getPlayerName (gameController.localPlayerN),
				"", "", gameController.getPlayerFemality());
		}
	}
}
