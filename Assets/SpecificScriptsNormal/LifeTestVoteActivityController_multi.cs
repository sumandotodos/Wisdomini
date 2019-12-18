using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeTestVoteActivityController_multi : Task {

	public GameObject lifeTestVoteCanvas;
	public GameObject lifeTestCanvas;
	public GameObject myTurnCanvas;
	public GameObject notMyTurnCanvas;
	public GameObject dossierCanvas;
	public GameObject rankingCanvas;
	public GameObject waitForConfirmationCanvas;
	public UIFaderScript fader;
	public GameController_multi gameController;
	public UIScaleFader lowestScoreScaler;
	public UIFaderScript[] voteHalo;

	public HeroController_multi heroController;

	public RawImage classImage;
	public Text classText;

	public RawImage indivImage;
	public Text indivText;

	bool buttonLock = false;

	int whichClass, whichIndiv;

	public void startLifeTestVote(int c, int i) {

		whichClass = c;
		whichIndiv = i;

		string cName, iName;
		Texture cTex, iTex;

		heroController.retrieveTextureAndNameFromClass (c, out cName, out cTex);
		heroController.retrieveTextureAndNameFromIndiv (c, i, out iName, out iTex);

		classImage.texture = cTex;
		classText.text = cName;

		indivImage.texture = iTex;
		indivText.text = iName;

		myTurnCanvas.SetActive (false);
		lifeTestCanvas.SetActive (false);
		notMyTurnCanvas.SetActive (false);
		dossierCanvas.SetActive (false);
		rankingCanvas.SetActive (false);
		lifeTestVoteCanvas.SetActive (true);
			


		lowestScoreScaler.Start ();
		foreach (UIFaderScript f in voteHalo) {
			f.Start ();
			f.setFadeValue (0.0f);
		}
		lowestScoreScaler.scaleOutImmediately ();
		fader.Start ();
		fader.fadeIn ();
		buttonLock = false;

	}

	public void endLifeTestVote() {
		fader.fadeOutTask (this);
		state = 2;
	}

	int state = 0;

	void Update() {
		if (state == 0) { // idling

		}

		if (state == 1) { // waiting for orbTouch

		}
		if (state == 2) {
			if (!isWaitingForTaskToComplete) {
				state = 0;
				lifeTestVoteCanvas.SetActive (false);
				//waitForConfirmationCanvas.SetActive (true);
			}
		}
	}

	public void touchOrb(int value) {
		if (buttonLock == true)
			return;
		buttonLock = true;
		voteHalo [value - 1].fadeOut ();
		gameController.networkAgent.sendCommand (gameController.playerTurn,
			"lifetestvote:" + gameController.localPlayerN + ":" + value + ":"); // I vote this shit
	}

	public void looseSeed() {
		lowestScoreScaler.scaleIn ();
	}



}
	