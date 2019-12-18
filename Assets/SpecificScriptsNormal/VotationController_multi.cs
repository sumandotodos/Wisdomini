using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VotationController_multi : Task {

	public GameObject votationControllerCanvas;

	public UIFaderScript fader;

	int votationHostId;

	public UIScaleFader[] sphereScaler;

	public UITextFader explainFader;
	public UITextFader waitFader;

	public GameController_multi gameController;

	// Use this for initialization
	void Start () {
		
	}

	int state = 0;

	List<int> votesFromOthers;

	public void startVotationController_multi(Task w, int hostId) {
		fader.Start ();
		foreach (UIScaleFader s in sphereScaler) {
			s.Start ();
			s.scaleOutImmediately ();
		}
		votationHostId = hostId;
		waitFader.Start ();
		waitFader.fadeOutImmediately ();
		explainFader.Start ();
		explainFader.fadeOutImmediately ();
		voted = false;
		votationControllerCanvas.SetActive (true);
		fader.fadeIn ();
		StartCoroutine ("showThings_CoRo");
		waiter = w;
		state = 0;
		votesFromOthers = new List<int> ();
		nVotes = 0;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i)
			votesFromOthers.Add (0);
		w.isWaitingForTaskToComplete = true;
	}

	public IEnumerator showThings_CoRo() {
		explainFader.fadeIn ();
		yield return new WaitForSeconds (0.25f);
		for(int i = 0; i < 3; ++i) {
			sphereScaler [i].scaleIn ();
			yield return new WaitForSeconds (0.25f);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {

		}

		if (state == 1) {
			if (!isWaitingForTaskToComplete) {
				votationControllerCanvas.SetActive (false);
				notifyFinishTask ();
				state = 0;
			}
		}
	}

	bool voted = false;

	public void clickOnValue(int val) {

		if (voted == true)
			return;

		voted = true;

		for(int i = 0; i < 3; ++i) {
			if (i != (val - 1))
				sphereScaler [i].scaleOut ();
		}

		waitFader.fadeIn ();

		gameController.networkAgent.sendCommand (votationHostId, "lifetestvote:" + gameController.localPlayerN + ":" + val + ":");

	}

	// network callback

	int nVotes;

	public void networkClickOnValue(int playerWho, int value) {
		votesFromOthers [playerWho] = value;
		++nVotes;
		if (nVotes == (GameController_multi.MaxPlayers - 1)) {
			// extract lowest vote
			int minVal = 1000;
			int minIndex = -1;
			bool unique = true;
			for (int j = 0; j < GameController_multi.MaxPlayers; ++j) {
				if (votesFromOthers [j] != 0) {
					if (votesFromOthers [j] < minVal) {
						minVal = votesFromOthers [j];
						minIndex = j;
					}
					else if (votesFromOthers [j] == minVal) {
						unique = false;
						break;
					}
				}
			}
			if (unique) { // the player who has voted the lowest loses one seed
				gameController.networkAgent.sendCommand (minIndex, "addseeds:-1:");
			}
		}
	}

	public void networkFinishThis() {
		fader.fadeOutTask (this);
		state = 1;
	}
}


