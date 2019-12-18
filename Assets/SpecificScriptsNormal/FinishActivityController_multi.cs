using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FinishActivityController_multi : Task {

	public GameController_multi gameController;

	public GameObject resultsSheet;
//	public Text volcanoScore;
//	public Text schoolScore;
//	public Text gompaScore;
//	public Text heroesScore;
//	public Text initiationScore;
//	public Text initClassesScore;
//	public Text creativityScore;
//	public Text empathyScore;
//	public Text bumisScore; // this is TRAININGS, not BUMIS
//	public Text grandTotalScore;
	public Text seedsScore;
	const float minSteps = 2.0f;
	const float maxSteps = 12.0f;

	public float h, x;
	const float hSpeed = 80.0f;
	float maxH;
	float playerH;
	float maxScore;
	public float antiscale;
	public float scale;

	public UIAnimatedPlayer[] player;

	public GameObject[] orderer;

	public RawImage[] fourYangs;

	public UIFaderScript fader;

	public UIGeneralFader mensajeFader;

	float offscreenY;

	List<int> playersOrderedByScoreIndexes;
	List<int> score;

	int state = -1;
	float timer = 0.0f;

	int curPlayer;

	bool showingSheet;

	Vector3 originalScale;

	public void startFinishTask(Task w) {

		mensajeFader.Start ();
		mensajeFader.fadeOutImmediately ();
		mensajeFader.fadeToOpaque ();

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		offscreenY = -Screen.height / 2.0f;

		//gameController.calculateBumis ();

		score = new List<int> ();

		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			
				score.Add (gameController.playerList [i].seeds);

		}

		playersOrderedByScoreIndexes = new List<int> ();

		fader.Start ();
		fader.fadeIn ();

		showingSheet = false;

		resultsSheet.SetActive (false);

		maxH = gameController.masterController.screenHeight * 0.6f;
		x = gameController.masterController.screenWidth / 2;
		state = -1;

		originalScale = Vector3.one; 
		// put all four players in starting position (offscreen)
		for (int i = 0; i < player.Length; ++i) {
			player [i].Start ();
			player [i].transform.position = new Vector3 (x, offscreenY, 0.0f);
		}

		for (int i = 0; i < fourYangs.Length; ++i) {
			fourYangs [i].enabled = gameController.playerPresent [i];
		}

		curPlayer = 0;
		h = -170.0f;
		//x = 0.0f;

		curPlayer = 0;

		// initialize some shit
		int tempMaxScore;
		maxScore = 0.0f;
		int maxI;


		// use naive ordering algorithm: 
		tempMaxScore = score[0];
		maxI = 0;
		for (int i = 1; i < score.Count; ++i) {
			if ((score [i] > tempMaxScore)) {
				tempMaxScore = score [i];
				maxI = i;

			}
		}
		maxScore = tempMaxScore;
		playersOrderedByScoreIndexes.Add(maxI);

		for (int j = 1; j < score.Count; ++j) 
		{
			// get maximum score
			tempMaxScore = -1;
			for (int i = 0; i < score.Count; ++i) {
				if ((score [i] >= tempMaxScore) && (!playersOrderedByScoreIndexes.Contains(i))) {
					tempMaxScore = score [i];
					maxI = i;
				}
			}
			playersOrderedByScoreIndexes.Add(maxI);
		}

		if (maxScore > 0)
			playerH = (score [playersOrderedByScoreIndexes [0]] / maxScore) * maxH;
		else
			playerH = 0.5f * maxH;

		// rearrange sprites for correct ordering by parenting them to 
		//  fixed-order elements in canvas  (orderer[i+1] is avobe orderer[i])
		for (int i = 0; i < score.Count; ++i) {
			player [playersOrderedByScoreIndexes [i]].transform.parent = orderer [i].transform;
		}
	}

	public void init() {


	}


	
	void Update () 
	{	
		if ((showingSheet == true) && Input.GetMouseButtonDown (0)) {
			resultsSheet.SetActive (false);
			showingSheet = false;
		}

		if (state == 0) { // idling

		} else if (state == 1) { // tell player to walk up stairs
			/*
			h += hSpeed * Time.deltaTime;
			player [playersOrderedByScoreIndexes[curPlayer]].transform.position = new Vector3 (x, h, 0);
			antiscale = 0.5f * h / maxH;

			scale = 1.0f - antiscale;
			player [playersOrderedByScoreIndexes[curPlayer]].transform.localScale = originalScale * scale;
			if (h > playerH) {
				state = 2;
				timer = 0.0f;
				player [playersOrderedByScoreIndexes[curPlayer]].isStanding = true;
			}*/
			x = gameController.masterController.screenWidth / 2 + 10.0f + Random.Range (-5.0f, 5.0f);
			player [playersOrderedByScoreIndexes [curPlayer]].autopilotWithParams (x, curPlayer * Screen.width * 0.05f, h, hSpeed, maxH, playerH);
			state = 2;

		} else if (state == 2) { // small delay
			timer += Time.deltaTime;
			if (timer > 2.0f) {
				++curPlayer;
				h = offscreenY;
				if (curPlayer < gameController.nPlayers) { // command next player
					if (maxScore > 0) {
						playerH = (score [playersOrderedByScoreIndexes [curPlayer]] / maxScore) * maxH;
					} else
						playerH = 0.5f * maxH;
					state = 1;
				} else
					state = 3;
			}
		} else if (state == 3) { // wait for touch



		}

		// 
		else if (state == 4) { // wait for final fadeout
			if (!isWaitingForTaskToComplete) {
				gameController.syncedPlayers++;
				gameController.networkAgent.broadcast ("sync:");
				state = 5;
			}
		} else if (state == 5) { // waiting for sync
			if (gameController.syncedPlayers == gameController.nPlayers) {

				// reset synced players right after sync
				gameController.syncedPlayers = 0;

				gameController.masterController.startActivity = "ResetGame";
				//gameController.masterController.startActivity = "Valoration";
				notifyFinishTask (); // return control to mastercontroller
			}
		} else if (state == 100) { // wait until all players know about volcano result
			if(gameController.playerList[gameController.localPlayerN].volcanoReady)
				state = 1;
		}

		if (state == -1) {
			if (Input.GetMouseButtonDown (0)) {
				mensajeFader.fadeToTransparent ();
				state = 100;
			}
		}
	}

	/* event callback */
	public void clickOnPlayer(int pl) 
	{
		resultsSheet.SetActive (true);
		showingSheet = true;
		seedsScore.text = "" + gameController.playerList [pl].seeds;

	}

	public void finishGameForGood() 
	{
		fader.fadeOutTask (this);
		state = 4;
	}
}
	