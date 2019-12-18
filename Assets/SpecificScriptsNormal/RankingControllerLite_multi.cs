using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RankingControllerLite_multi : Task {

	public Text playerNameText;
	public FGTable playerNameTable;

	public Texture[] playerFullBody;
	public Texture[] playerBust;

	public RawImage[] otherPlayerImage;
	public RawImage[] otherPlayerSeed;
	public Text[] otherPlayerText;
	public RawImage myPlayerImage;
	public Text myPlayerText;

	public RawImage BestPlayerSingleImage;
	public RawImage[] BestPlayersClusterImage;
	public Text BestPlayerScoreText;

	public UIFaderScript fader;

	public GameController_multi gameController;

	public GameObject rankingCanvas;

	int state = 0;

	public void setupPlayers() {

		for (int i = 0; i < otherPlayerSeed.Length; ++i) {
			if (i < (gameController.nPlayers - 1)) {
				otherPlayerImage [i].enabled = true;
				otherPlayerSeed [i].enabled = true;
				otherPlayerText [i].enabled = true;
			} else {
				otherPlayerImage [i].enabled = false;
				otherPlayerSeed [i].enabled = false;
				otherPlayerText [i].enabled = false;
			}
		}

		myPlayerImage.texture = playerFullBody [gameController.localPlayerN];
		myPlayerText.text = "" + gameController.playerList [gameController.localPlayerN].seeds;
		int index = 0;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if ((i != gameController.localPlayerN) && (gameController.playerPresent[i])) {
				otherPlayerImage [index].texture = playerFullBody [i];
				otherPlayerText [index].text = "" + gameController.playerList [i].seeds;
				++index;
			}
		}

		// extract max seeds
		int maxSeeds = 0;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (gameController.playerList [i].seeds > maxSeeds) {
				maxSeeds = gameController.playerList [i].seeds;
			}
		}

		BestPlayerScoreText.text = "" + maxSeeds;

		// find out number of players with this score
		List<int> sharedPlayers = new List<int> ();
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (gameController.playerList [i].seeds == maxSeeds) {
				sharedPlayers.Add (i);
			}
		}

		if (maxSeeds == 0) {
			BestPlayerScoreText.enabled = false;
			BestPlayerSingleImage.enabled = false;
			foreach (RawImage r in BestPlayersClusterImage) {
				r.enabled = false;
			}
		} else {
			BestPlayerScoreText.enabled = true;
			if (sharedPlayers.Count == 1) {
				
				BestPlayerSingleImage.texture = playerBust [sharedPlayers [0]];
				BestPlayerSingleImage.enabled = true;
				foreach (RawImage r in BestPlayersClusterImage) {
					r.enabled = false;
				}
			} else if (sharedPlayers.Count > 1) {
				BestPlayerSingleImage.enabled = false;
				index = 0;
				for (int i = 0; i < sharedPlayers.Count; ++i) {
					BestPlayersClusterImage [i].texture = playerBust [sharedPlayers [index++]];
					BestPlayersClusterImage [i].enabled = true;
				}
				for (int i = sharedPlayers.Count; i < BestPlayersClusterImage.Length; ++i) {
					BestPlayersClusterImage [i].enabled = false;
				}

			}
		}

	}

	public void startRankingActivity(Task w) {
		waiter = w;
		setupPlayers ();
		playerNameText.text = (string)playerNameTable.getElement (0, gameController.localPlayerN);
		w.isWaitingForTaskToComplete = true;
		fader.Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();
		rankingCanvas.SetActive (true);
		state = 0;
	}

	public void clickOnBackButton() {
		fader.fadeOutTask (this);
		state = 1; // waiting for fadeout
	}

	void Update() {
		if (state == 0) {

		}

		if (state == 1) {
			if (!isWaitingForTaskToComplete) {
				state = 0;
				rankingCanvas.SetActive (false);
				notifyFinishTask ();
			}
		}
	}

	void Start() {
		rankingCanvas.SetActive (false);
	}

}
	