using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankingController_mono : Task {

	public Texture[] playerTex;
	public Texture[] humanPlayerTex;

	public Color[] playerColor;

	public Text maxGompasText;
	public Text maxSchoolText;
	public Text maxTrainingsText;
	public Text maxHeroesText;
	public Text maxCardsText;
	public Text maxCardClassesText;

	public RawImage pMaxSchools;
	public RawImage pMaxGompas;
	public RawImage pMaxInitiations;
	public RawImage pMaxInitiationClasses;
	public RawImage pMaxHeroes;
	public RawImage pMaxCreativity;
	public RawImage pMaxEmpathy;
	public RawImage pMaxTrainings;
	public RawImage[] maxSchoolCluster;
	public RawImage[] maxGompasCluster;
	public RawImage[] maxInitCluster;
	public RawImage[] maxInitClassCluster;
	public RawImage[] maxHeroesCluster;
	public RawImage[] maxCreatCluster;
	public RawImage[] maxEmpatCluster;
	public RawImage[] maxTrainingsCluster;
	public RawImage humanPlayer;

	public RawImage mostStarsPlayer;
	public Text mostStarsText;
	public RawImage[] maxStarsCluster;
	public Text myStarsText;

	public UIAutoFader bgFader;

	public CircpleDeploy explainPanel;

	public GameController_mono gameController;

	public UITextAutoFader textFader;

	public UIFaderScript fader;

	public RosettaWrapper rosettaWrap;
	public StringBank explainStrings;

	string nextExplainText;

	int state;

	int state2; // slot2

	// calculate bumis

	public void updateRanking() {

		// reset all cluster images
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			maxSchoolCluster [i].enabled = false;
			maxGompasCluster [i].enabled = false;
			maxInitCluster [i].enabled = false;
			maxInitClassCluster [i].enabled = false;
			maxHeroesCluster [i].enabled = false;
			maxCreatCluster [i].enabled = false;
			maxEmpatCluster [i].enabled = false;
			maxTrainingsCluster [i].enabled = false;
			maxStarsCluster [i].enabled = false;
		}

		gameController.calculateBumis ();




		// get player with the most INITIATIONS
		int maxInitiations = 0;

		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && (gameController.playerList [i].nInitiations > maxInitiations)) {
				maxInitiations = gameController.playerList [i].nInitiations;
			}
		}
		int activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].nInitiations == maxInitiations) {
				if (activatedPlayers == 0) {
					pMaxInitiations.enabled = true;
					pMaxInitiations.texture = playerTex [i];
					//maxCardsText.text = "" + gameController.playerList [i].nInitiations;
					maxCardsText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					pMaxInitiations.enabled = false;
					//maxCardsText.text = "";
					maxInitCluster [0].enabled = true;
					maxInitCluster [0].texture = pMaxInitiations.texture;
					maxInitCluster [activatedPlayers].enabled = true;
					maxInitCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				} else {
					maxInitCluster [activatedPlayers].enabled = true;
					maxInitCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				maxCardsText.text = maxInitiations.ToString ();
			}
		}





		// get player with the most TRAININGS
		int maxTrainings = 0;

		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].trainings > maxTrainings) {
				maxTrainings = gameController.playerList [i].trainings;
			}
		}
		activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].trainings == maxTrainings) {
				if (activatedPlayers == 0) {
					pMaxTrainings.enabled = true;
					pMaxTrainings.texture = playerTex [i];
					//maxTrainingsText.text = "" + gameController.playerList [i].trainings;
					maxTrainingsText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					pMaxTrainings.enabled = false;
					//maxTrainingsText.text = "";
					maxTrainingsCluster [0].enabled = true;
					maxTrainingsCluster [0].texture = pMaxTrainings.texture;
					maxTrainingsCluster [activatedPlayers].enabled = true;
					maxTrainingsCluster [activatedPlayers].texture = playerTex [i];

					++activatedPlayers;
				} else {
					maxInitCluster [activatedPlayers].enabled = true;
					maxInitCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				maxTrainingsText.text = maxTrainings.ToString ();
			}
		}




		// get player with the most HEROES
		int maxHeroes = 0;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].totalHeroes > maxHeroes) {
				maxHeroes = gameController.playerList [i].totalHeroes;
			}
		}
		activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].totalHeroes == maxHeroes) {
				if (activatedPlayers == 0) {
					pMaxHeroes.enabled = true;
					pMaxHeroes.texture = playerTex [i];
					//maxHeroesText.text = "" + gameController.playerList [i].totalHeroes;
					maxHeroesText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					pMaxHeroes.enabled = false;
					//maxHeroesText.text = "";
					// 4
					maxHeroesCluster [0].enabled = true;
					maxHeroesCluster [0].texture = pMaxHeroes.texture;
					maxHeroesCluster [activatedPlayers].enabled = true;
					maxHeroesCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				} else {
					// 2
					maxHeroesCluster [activatedPlayers].enabled = true;
					maxHeroesCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				maxHeroesText.text = maxHeroes.ToString ();
			}
		}


		// different initiation classes
		int maxInitiationDifferentClasses = 0;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].initiationsDifferentClasses > maxInitiationDifferentClasses) {
				maxInitiationDifferentClasses = gameController.playerList [i].initiationsDifferentClasses;
			}
		}
		activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].initiationsDifferentClasses == maxInitiationDifferentClasses) {
				if (activatedPlayers == 0) {
					pMaxInitiationClasses.enabled = true;
					pMaxInitiationClasses.texture = playerTex [i];
					//maxCardClassesText.text = "" + gameController.playerList [i].initiationsDifferentClasses;
					maxCardClassesText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					pMaxInitiationClasses.enabled = false;
					//maxCardClassesText.text = "";
					// 4
					maxInitClassCluster [0].enabled = true;
					maxInitClassCluster [0].texture = pMaxInitiationClasses.texture;
					maxInitClassCluster [activatedPlayers].enabled = true;
					maxInitClassCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				} else {
					// 2
					maxInitClassCluster [activatedPlayers].enabled = true;
					maxInitClassCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				maxCardClassesText.text = maxInitiationDifferentClasses.ToString ();
			}
		}

	


		int maxSchools = 0;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].nSchools > maxSchools) {
				maxSchools = gameController.playerList [i].nSchools;
			}
		}
		activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].nSchools == maxSchools) {
				if (activatedPlayers == 0) {
					pMaxSchools.enabled = true;
					pMaxSchools.texture = playerTex [i];
					//maxSchoolText.text = "" + gameController.playerList [i].nSchools;
					maxSchoolText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					pMaxSchools.enabled = false;
					//maxSchoolText.text = "";
					// 4
					maxSchoolCluster [0].enabled = true;
					maxSchoolCluster [0].texture = pMaxSchools.texture;
					maxSchoolCluster [activatedPlayers].enabled = true;
					maxSchoolCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				} else {
					// 2
					maxSchoolCluster [activatedPlayers].enabled = true;
					maxSchoolCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				maxSchoolText.text = maxSchools.ToString();
			}
		}


		int maxGompas = 0;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].nGompas > maxGompas) {
				maxGompas = gameController.playerList [i].nGompas;
			}
		}
		activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].nGompas == maxGompas) {
				if (activatedPlayers == 0) {
					pMaxGompas.enabled = true;
					pMaxGompas.texture = playerTex [i];
					//maxGompasText.text = "" + gameController.playerList [i].nGompas;
					maxGompasText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					pMaxGompas.enabled = false;
					// 4
					maxGompasCluster [0].enabled = true;
					maxGompasCluster [0].texture = pMaxGompas.texture;
					maxGompasCluster [activatedPlayers].enabled = true;
					maxGompasCluster [activatedPlayers].texture = playerTex [i];
					//maxGompasText.text = "";
					++activatedPlayers;
				} else {
					// 2
					maxGompasCluster [activatedPlayers].enabled = true;
					maxGompasCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				maxGompasText.text = maxGompas.ToString ();
			}
		}

//		int maxTrains = 0;
//		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
//			if (gameController.playerPresent[i] && gameController.playerList [i].trainings > maxTrains) {
//				maxTrains = gameController.playerList [i].trainings;
//			}
//		}
//		activatedPlayers = 0;
//		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
//			if (gameController.playerPresent[i] && gameController.playerList [i].trainings == maxTrains) {
//				if (activatedPlayers == 0) {
//					pMaxTrainings.enabled = true;
//					pMaxTrainings.texture = playerTex [i];
//					maxTrainingsText.color = playerColor [i];
//					++activatedPlayers;
//				} else if (activatedPlayers == 1) {
//					pMaxTrainings.enabled = false;
//					// 4
//					maxTrainingsCluster [0].enabled = true;
//					maxTrainingsCluster [0].texture = pMaxTrainings.texture;
//					maxTrainingsCluster [activatedPlayers].enabled = true;
//					maxTrainingsCluster [activatedPlayers].texture = playerTex [i];
//					++activatedPlayers;
//				} else {
//					// 2
//					maxTrainingsCluster [activatedPlayers].enabled = true;
//					maxTrainingsCluster [activatedPlayers].texture = playerTex [i];
//					++activatedPlayers;
//				}
//				maxTrainingsText.text = maxTrains.ToString ();
//			}
//		}

		int maxStars = 0;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].totalBumis > maxStars) {
				maxStars = gameController.playerList [i].totalBumis;
			}

		}
		activatedPlayers = 0;
		for(int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (gameController.playerPresent[i] && gameController.playerList [i].totalBumis == maxStars) {
				if (activatedPlayers == 0) {
					mostStarsPlayer.enabled = true;
					mostStarsPlayer.texture = playerTex [i];
					mostStarsText.color = playerColor [i];
					++activatedPlayers;
				} else if (activatedPlayers == 1) {
					mostStarsPlayer.enabled = false;
					// 4
					maxStarsCluster [0].enabled = true;
					maxStarsCluster [0].texture = mostStarsPlayer.texture;
					maxStarsCluster [activatedPlayers].enabled = true;
					maxStarsCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				} else {
					// 2
					maxStarsCluster [activatedPlayers].enabled = true;
					maxStarsCluster [activatedPlayers].texture = playerTex [i];
					++activatedPlayers;
				}
				mostStarsText.text = maxStars.ToString ();
			}
		}

		humanPlayer.texture = humanPlayerTex [gameController.localPlayerN];
		myStarsText.text = "" + gameController.playerList [gameController.localPlayerN].totalBumis;
	}


	public void reset() {
		state = 0;
		fader.fadeIn ();
		updateRanking ();
	}


	public void startRankingActivity(Task w) {

		w.isWaitingForTaskToComplete = true;
		textFader.Start ();
		textFader.reset ();
		bgFader.reset ();
		explainStrings.rosetta = rosettaWrap.rosetta;
		waiter = w;
		state2 = 0;
		fader.Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();
		reset ();

	}
	
	// Update is called once per frame
	void Update () {

		// update slot 2

		if (state2 == 0) {
			// idling
		}
		if (state2 == 1) { // waiting for text fadeout
			if (!isWaitingForTaskToComplete) {
				
				textFader.gameObject.GetComponent<Text> ().text = nextExplainText;
				textFader.fadein ();
				state2 = 0;
			}
		}



		// update slot 1
	
		if (state == 0) {

		}

		if (state == 10) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				state = 0;
				notifyFinishTask ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			clickOnBackButton ();
		}

	}

	public void clickOnLaurel() {
		bgFader.fadein ();
		bgFader.GetComponent<RawImage> ().raycastTarget = true;
		explainPanel.extend ();
	}

	public void clickOnBg() {
		explainPanel.retract ();
		bgFader.GetComponent<RawImage> ().raycastTarget = false;
		textFader.fadeout ();
		bgFader.fadeout ();
	}

	public void clickOnBackButton() {
		fader.fadeOutTask (this);
		state = 10; // waiting for fadeout
	}

	public void clickOnItem(int id) {
		nextExplainText = explainStrings.getString (id);
		textFader.fadeoutTask (this);
		state2 = 1;

	}
}
