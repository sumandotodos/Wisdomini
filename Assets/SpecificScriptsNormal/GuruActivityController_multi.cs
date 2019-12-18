using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GuruActivityController_multi : Task {

	public GameObject normalAspa;
	public GameObject normalOK;
	public GameObject barcoOK;

	public UITip barcoTip;
	public GameController_multi gameController;
	public SeedToPlayerController_multi seedToPlayerController;
	public GameObject QuestionScroll;

	public UITextAutoFader missingLabelFader;
	public UITextAutoFader meaningLabelFader;
	public UITextAutoFader answerLabelFader;

	public UIPlanet[] planets;
	public GameObject planetsCenter;
	public GameObject canvas1;
	public GameObject canvas2;

	public RawImage whiteOrb;
	public RawImage purpleOrb;
	public RawImage orangeOrb;
	public RawImage yellowOrb;
	public RawImage redOrb;
	public RawImage blueOrb;
	public RawImage greenOrb;
	public RawImage brownOrb;

	public RawImage warriorIcon;
	public RawImage explorerIcon;
	public RawImage wizardIcon;

	public Texture badassUniverseBG;

	public RawImage backgrImage;
	public RawImage backgrBoatImage;

	public RawImage guruImage;

	public FGTable type1Table;
	public FGTable type2Table;
	public FGTable type3Table;
	public RosettaWrapper rosettaWrap;

	public Text questionText;

	public float multiplier = 1.0f;
	const float multiplierSpeed = 6.0f;

	public UIFaderScript fader1;
	public UIFaderScript fader2;

	public RawImage okButton;
	public RawImage notOKButton;

	const float shortDelay = 0.75f;


	int typeOfTest;

	public int state = 0;

	float timer = 0.0f;

	int clickedOnColor;
	bool clickOnW;

	bool guruAct;

	bool clickOnHeroLock = false;
	bool clickOnOrbLock = false;
	bool clickOnYYLock = false;

	[HideInInspector]
	public int activityResult;

	public void pickPlanet() {

		fader2.fadeSpeed = 0.35f;
		fader2.fadeColor = Color.white;
		fader2.fadeOutTask (this);
		state = 1;

	}

	public int result() {
		return activityResult;
	}

	public void init(bool isGuruNotCoin) {

		meaningLabelFader.Start ();
		meaningLabelFader.reset ();
		missingLabelFader.Start ();
		missingLabelFader.reset ();
		answerLabelFader.Start ();
		answerLabelFader.reset ();
		notOKButton.enabled = false;
		okButton.enabled = false;
		QuestionScroll.SetActive (false);
		//planetsCenter.SetActive (true);
		for (int i = 0; i < planets.Length; ++i) {
			planets [i].active = true;
			planets [i].speedMultiplier = 1.0f;
		}

		if (guruAct) { 
			canvas2.SetActive (false);
			canvas1.SetActive (true);
			guruImage.enabled = true;
			normalAspa.SetActive (true);
			normalOK.SetActive (true);
			barcoOK.SetActive (false);
			state = 0;
			timer = 0.0f;
		} else {
			canvas1.SetActive (false);
			canvas2.SetActive (true);
			guruImage.enabled = false;
			barcoTip.show ();
			normalAspa.SetActive (false);
			normalOK.SetActive (false);
			barcoOK.SetActive (true);
			state = 2;
			timer = 2f;
		}



		bool noOrbWasExtended = true;


		bool hasYellowWisdom = gameController.guruPickedYellow;
		bool hasOrangeWisdom = gameController.guruPickedOrange;
		bool hasRedWisdom = gameController.guruPickedRed;
		bool hasPurpleWisdom = gameController.guruPickedPurple;
		bool hasGreenWisdom = gameController.guruPickedGreen;
		bool hasBlueWisdom = gameController.guruPickedBlue;
		bool hasBrownWisdom = gameController.guruPickedBrown;

//		if (((gameController.warriorOwner == -1) || (gameController.explorerOwner == -1) || (gameController.wizardOwner == -1)) 
//			&& (!gameController.playerList[gameController.localPlayerN].hasSecondaryWisdoms)) {
//			noOrbWasExtended = false;
//			whiteOrb.GetComponent<CircpleDeploy> ().extend ();
//		}
		if (!hasYellowWisdom) {
			noOrbWasExtended = false;
			yellowOrb.GetComponent<CircpleDeploy> ().extend ();
		}
		if (!hasPurpleWisdom) {
			noOrbWasExtended = false;
			purpleOrb.GetComponent<CircpleDeploy> ().extend ();
		}
		if (!hasOrangeWisdom) {
			noOrbWasExtended = false;
			orangeOrb.GetComponent<CircpleDeploy> ().extend ();
		}
		if (!hasRedWisdom) {
			noOrbWasExtended = false;
			redOrb.GetComponent<CircpleDeploy> ().extend ();
		}
		if (!hasGreenWisdom) {
			noOrbWasExtended = false;
			greenOrb.GetComponent<CircpleDeploy> ().extend ();
		}
		if (!hasBlueWisdom) {
			noOrbWasExtended = false;
			blueOrb.GetComponent<CircpleDeploy> ().extend ();
		}
		if (!hasBrownWisdom) {
			noOrbWasExtended = false;
			brownOrb.GetComponent<CircpleDeploy> ().extend ();
		}
//		if (!gameController.playerPresent [GameController_multi.YELLOW] && !hasYellowWisdom) {
//			noOrbWasExtended = false;
//			yellowOrb.GetComponent<CircpleDeploy> ().extend ();
//		}
//		if (!gameController.playerPresent [GameController_multi.GREEN] && !hasOrangeWisdom) {
//			noOrbWasExtended = false;
//			orangeOrb.GetComponent<CircpleDeploy> ().extend ();
//		}
//		if (!gameController.playerPresent [GameController_multi.BROWN] && !hasRedWisdom) {
//			noOrbWasExtended = false;
//			redOrb.GetComponent<CircpleDeploy> ().extend ();
//		}
//		if (!gameController.playerPresent [GameController_multi.BLUE] && !hasPurpleWisdom) {
//			noOrbWasExtended = false;
//			purpleOrb.GetComponent<CircpleDeploy> ().extend ();
//		}

		if (noOrbWasExtended && isGuruNotCoin) { // nothing to do
			notifyFinishTask ();
			MusicController.fadeOut ();
			state = 0;
		}

		fader1.fadeIn ();
		fader2.fadeIn ();

		clickOnHeroLock = false;
		clickOnOrbLock = false;
		clickOnYYLock = false;

		MusicController.playTrack (0);
		MusicController.fadeIn ();

	}

	public void startGuruTask(Task w, bool _guruActivity) { // AQUI VA EL BARCO
		guruAct = _guruActivity;
		planetsCenter.SetActive (_guruActivity);
		backgrBoatImage.enabled = !_guruActivity;
		waiter = w;
		fader1.fadeColor = Color.black;
		w.isWaitingForTaskToComplete = true;
		multiplier = 1.0f;
		init (_guruActivity);
	}
	
	void Update () {
	
		if (state == 0) {

		}

		if (state == 1) {

			multiplier += multiplierSpeed * Time.deltaTime;
			for (int i = 0; i < planets.Length; ++i) {
				planets [i].speedMultiplier = multiplier;
			}

			if (!isWaitingForTaskToComplete) {
				state = 2;
				timer = 0.0f;
				for (int i = 0; i < planets.Length; ++i) {
					planets [i].active = false;
				}
				planetsCenter.SetActive (false);
			}
		}

		if (state == 2) { // a small delay...
			timer += Time.deltaTime;
			if (timer > 1.0f) {
				timer = 0.0f;
				state = 3;
				backgrImage.enabled = false;
				guruImage.enabled = false;
				fader2.fadeSpeed = 1.1f;
				fader2.fadeIn ();
				if (guruAct)
					typeOfTest = Random.Range (0, 2) * 2; // entendemos que 2 es completar palabro  1 es interpretar   0 es contestar
				else {
					typeOfTest = 1;
				}
			}
		}

		if (state == 3) {

			int r = 0;
			string test = "";
			if (typeOfTest == 0) {
				r = type1Table.getNextRowIndex();
				test = (string)type1Table.getElement (0, r);
				gameController.seedToPlayerController.answer.text = (string)type1Table.getElement(1, r);
				answerLabelFader.fadein ();
			}
			if (typeOfTest == 1) {
				r = type2Table.getNextRowIndex();
				test = (string)type2Table.getElement (0, r);
				gameController.seedToPlayerController.answer.text = (string)type2Table.getElement (1, r);
				meaningLabelFader.fadein ();
			}
			if (typeOfTest == 2) {
				r = type3Table.getNextRowIndex();
				test = (string)type3Table.getElement (0, r);
				gameController.seedToPlayerController.answer.text = (string)type3Table.getElement (1, r);
				missingLabelFader.fadein ();
			}
			QuestionScroll.SetActive (true);
			questionText.text = test;
			gameController.networkAgent.broadcast ("gurutest:" + typeOfTest + ":" + r + ":");
			state = 12;

			notOKButton.enabled = true;
			okButton.enabled = true;

		}

		if (state == 40) { // clicked on white, waiting for fadeToWhite to end;
			if (!isWaitingForTaskToComplete) {
				fader1.fadeColor = Color.white;
				fader1.setFadeValue (1.0f);
				canvas2.SetActive (false);
				canvas1.SetActive (true);
				fader1.fadeInTask (this);
				state = 4;
			}
		}

		if (state == 4) { // clicked on white, waiting for orbs to shrink...
			if (!isWaitingForTaskToComplete) {
				timer = 0.0f;
				state = 8;
				// if all secondary heroes have been taken, fadeout
//				if ((gameController.warriorOwner != -1) &&
//				    (gameController.explorerOwner != -1) &&
//				    (gameController.wizardOwner != -1)) {
//					fader1.fadeOutTask (this);
//					state = 8; // fadeout
//				}
//				// OR if we have already taken a secondary hero, fadeout
//				else if ((gameController.warriorOwner == gameController.localPlayerN) ||
//				         (gameController.explorerOwner == gameController.localPlayerN) ||
//					(gameController.wizardOwner == gameController.localPlayerN)) {
//					fader1.fadeOutTask (this);
//					state = 8;  // fadeout
//
//				} else {
//					// notification
//					gameController.addNotification (Notification.TRIPLESABIDURIA, gameController.getPlayerName (gameController.localPlayerN), "", "");
//					gameController.playerList [gameController.localPlayerN].addSecondaryWisdoms ();
//					//gameController.playerList [gameController.localPlayerN].hasSecondaryWisdoms = true;
//					gameController.networkAgent.broadcast ("secondarywisdoms:" + gameController.localPlayerN + ":");
//				}
				gameController.playerList[gameController.localPlayerN].addSeeds(1);
				gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":" + 1 + ":"); // global
				gameController.addNotification (Notification.CONSIGUESEMILLA, gameController.getPlayerName (gameController.localPlayerN), "1", "", gameController.getPlayerFemality());
			}
		}

		if (state == 5) { // small delay
			timer += Time.deltaTime;
			if (timer > shortDelay) {
				timer = 0.0f;
				state = 6;
			}
		}

		if (state == 6) { // extend heroes
			warriorIcon.GetComponent<CircpleDeploy> ().extend ();
			explorerIcon.GetComponent<CircpleDeploy> ().extend ();
			wizardIcon.GetComponent<CircpleDeploy> ().extend ();
			if (gameController.warriorOwner != -1) {
				warriorIcon.color = new Color (1, 1, 1, 0.25f);
			} else
				warriorIcon.color = new Color (1, 1, 1, 1);
			if (gameController.explorerOwner != -1) {
				explorerIcon.color = new Color (1, 1, 1, 0.25f);
			} else
				explorerIcon.color = new Color (1, 1, 1, 1);
			if (gameController.wizardOwner != -1) {
				wizardIcon.color = new Color (1, 1, 1, 0.25f);
			} else
				wizardIcon.color = new Color (1, 1, 1, 1);
			state = 7;
		}

		if (state == 7) { // wait for user input

		}

		if(state == 8) { // waiting for fadeout, and return to parent task
			//if (!isWaitingForTaskToComplete) {
				
				if (guruAct) {
					notifyFinishTask ();
					MusicController.fadeOut ();
					state = 0;
				} else {
					gameController.networkAgent.broadcast ("seedtoplayer:");
					seedToPlayerController.startSeedToPlayerActivity (this);
					canvas1.SetActive (false);
					canvas2.SetActive (false);
					state = 88;
				}

			//}
		}

		if (state == 88) {
			if (!isWaitingForTaskToComplete) {
				state = 89;
			}
		}

		if (state == 89) {
			if (seedToPlayerController.nSeedsToPlayers == gameController.nPlayers) {
				seedToPlayerController.nSeedsToPlayers = 0;
				notifyFinishTask ();
				MusicController.fadeOut ();
				state = 0;
			}
		}

		if (state == 9) { // waiting for hero wheel to retract
			if (!isWaitingForTaskToComplete) {
				fader1.fadeOutTask (this);
				state = 8;
			}
		}

		if (state == 10) { // waiting for orbs to shrink, and then, canvas2
			if (!isWaitingForTaskToComplete) {
				fader1.fadeOutTask (this);
				state = 11; // antes 11
			}
		}

		if (state == 11) {
			if (!isWaitingForTaskToComplete) {
				canvas2.SetActive (true);
				canvas1.SetActive (false);
				guruImage.enabled = true;
				planetsCenter.SetActive(true);
				okButton.enabled = false;
				notOKButton.enabled = false;
				//fader2.setFadeValue (0.0f);
				fader2.fadeIn ();
				for (int i = 0; i < planets.Length; ++i) {
					planets [i].active = true;
					planets [i].speedMultiplier = 1.0f;
				}
				questionText.text = "";

//				if (clickOnW) {
//					state = 4;
//				}
//				else{
					state = 0; // wait for user to pick a planet
				//}
			}
		}
	}


	/* event callbacks */
	public void clickOnWhite() {

		if (clickOnOrbLock)
			return;
		clickOnOrbLock = true;

		//state = 4;
		clickOnW = true;
		state = 40;
		whiteOrb.GetComponent<CircpleDeploy> ().retractTask (this);
		redOrb.GetComponent<CircpleDeploy> ().retract ();
		orangeOrb.GetComponent<CircpleDeploy> ().retract ();
		purpleOrb.GetComponent<CircpleDeploy> ().retract ();
		yellowOrb.GetComponent<CircpleDeploy> ().retract ();
		clickedOnColor = GameController_multi.WHITE;

	}



	public void clickOn(int c) {
		if (clickOnOrbLock)
			return;
		clickOnOrbLock = true;

		clickOnW = false;
		state = 10;
		//whiteOrb.GetComponent<CircpleDeploy> ().retractTask (this);
		redOrb.GetComponent<CircpleDeploy> ().retractTask (this);
		orangeOrb.GetComponent<CircpleDeploy> ().retract ();
		purpleOrb.GetComponent<CircpleDeploy> ().retract ();
		yellowOrb.GetComponent<CircpleDeploy> ().retract ();
		brownOrb.GetComponent<CircpleDeploy> ().retract ();
		blueOrb.GetComponent<CircpleDeploy> ().retract ();
		greenOrb.GetComponent<CircpleDeploy> ().retract ();
		switch (c) {
		case 0: 
			clickedOnColor = GameController_multi.RED;
			break;
		case 1:
			clickedOnColor = GameController_multi.YELLOW;
			break;
		case 2:
			clickedOnColor = GameController_multi.BLUE;
			break;
		case 3:
			clickedOnColor = GameController_multi.GREEN;
			break;
		case 4:
			clickedOnColor = GameController_multi.ORANGE;
			break;
		case 5:
			clickedOnColor = GameController_multi.PURPLE;
			break;
		case 6:
			clickedOnColor = GameController_multi.BROWN;
			break;
		}
	}



	public void clickOnWarrior() {

		if (clickOnHeroLock)
			return;
		clickOnHeroLock = true;

		if (gameController.warriorOwner != -1)
			return;
		state = 9; // wait for retract

		gameController.warriorOwner = gameController.localPlayerN;
		gameController.networkAgent.broadcast ("setheroowner:0:" + gameController.localPlayerN + ":");

		warriorIcon.GetComponent<CircpleDeploy> ().retractTask (this);
		explorerIcon.GetComponent<CircpleDeploy> ().retract ();
		wizardIcon.GetComponent<CircpleDeploy> ().retract ();



		gameController.addNotification (Notification.TIENE5GUERREROS, gameController.getPlayerName (gameController.localPlayerN), "", "", gameController.getPlayerFemality());
		gameController.playerList [gameController.localPlayerN].addHero (GameController_multi.WARRIOR, 5);
		gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + GameController_multi.WARRIOR + ":5:");
	}

	public void clickOnExplorer() {

		if (clickOnHeroLock)
			return;
		clickOnHeroLock = true;

		if (gameController.explorerOwner != -1)
			return;
		state = 9; // wait for retract

		gameController.explorerOwner = gameController.localPlayerN;
		gameController.networkAgent.broadcast ("setheroowner:1:" + gameController.localPlayerN + ":");

		warriorIcon.GetComponent<CircpleDeploy> ().retractTask (this);
		explorerIcon.GetComponent<CircpleDeploy> ().retract ();
		wizardIcon.GetComponent<CircpleDeploy> ().retract ();
		gameController.addNotification (Notification.TIENE3EXPLORADORAS, gameController.getPlayerName (gameController.localPlayerN), "", "", gameController.getPlayerFemality());
		gameController.playerList [gameController.localPlayerN].addHero (GameController_multi.EXPLORER, 3);
		gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + GameController_multi.EXPLORER + ":3:");
	}

	public void clickOnWizard() {

		if (clickOnHeroLock)
			return;
		clickOnHeroLock = true;

		if (gameController.wizardOwner != -1)
			return;
		state = 9; // wait for retract

		gameController.wizardOwner = gameController.localPlayerN;
		gameController.networkAgent.broadcast ("setheroowner:2:" + gameController.localPlayerN + ":");

		warriorIcon.GetComponent<CircpleDeploy> ().retractTask (this);
		explorerIcon.GetComponent<CircpleDeploy> ().retract ();
		wizardIcon.GetComponent<CircpleDeploy> ().retract ();
		gameController.addNotification (Notification.TIENE4MAGAS, gameController.getPlayerName (gameController.localPlayerN), "", "", gameController.getPlayerFemality());
		gameController.playerList [gameController.localPlayerN].addHero (GameController_multi.WIZARD, 4);
		gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + GameController_multi.WIZARD + ":4:");
	}


	public void clickOnYinYang() {

		if (clickOnYYLock)
			return;
		clickOnYYLock = true;

		int Energy = 0;
		if (guruAct) {
			if (!clickOnW) {
			
				switch (clickedOnColor) {
				case GameController_multi.RED: // principal 5
					Energy = GameController_multi.WARRIOR;
					break;
				case GameController_multi.ORANGE: // principal 7
					Energy = GameController_multi.MASTER;
					break;
				case GameController_multi.YELLOW: // secund 3
					Energy = GameController_multi.EXPLORER;
					break;
				case GameController_multi.PURPLE: // secund 4
					Energy = GameController_multi.WIZARD;
					break;
				case GameController_multi.GREEN:
					Energy = GameController_multi.SAGE;
					break;
				case GameController_multi.BROWN:
					Energy = GameController_multi.YOGI;
					break;
				case GameController_multi.BLUE:
					Energy = GameController_multi.PHILOSOPHER;
					break;
				}

				gameController.playerList[gameController.localPlayerN].addSeeds(1);
				gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":" + 1 + ":"); // global
				gameController.addNotification (Notification.CONSIGUEUNASOLASEMILLA, gameController.getPlayerName (gameController.localPlayerN), 
					"", "", gameController.getPlayerFemality());
				
				gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), 
					gameController.getClassNameLos (Energy), "", gameController.getPlayerFemality());


				switch (clickedOnColor) {
				case GameController_multi.RED: // principal 5
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 5);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":5:");
					break;
				case GameController_multi.ORANGE: // principal 7
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 7);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":7:");
					break;
				case GameController_multi.YELLOW: // secund 3
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 3);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":3:");
					break;
				case GameController_multi.PURPLE: // secund 4
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 4);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":4:");
					break;
				case GameController_multi.GREEN: // 7
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 4);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":4:");
					break;
				case GameController_multi.BROWN: // 7
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 4);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":4:");
					break;
				case GameController_multi.BLUE: // 7
					//gameController.addNotification (Notification.REPRESENTANTEDE, gameController.getPlayerName (gameController.localPlayerN), gameController.getClassNameLos (Energy), "");
					gameController.playerList [gameController.localPlayerN].addWisdom (Energy);
					gameController.playerList [gameController.localPlayerN].addHero (Energy, 4);
					gameController.networkAgent.broadcast ("addhero:" + gameController.localPlayerN + ":" + Energy + ":4:");
					break;
				}



				// configurar como "energía principal" para que quede desconectado el botón de prueba gurú en la
				// pantalla principal
				gameController.playerList[gameController.localPlayerN].setMainWisdom(Energy);


				gameController.networkAgent.broadcast ("addwisdom:" + gameController.localPlayerN + ":" + Energy + ":");
				gameController.networkAgent.broadcast ("gurupicked:" + gameController.localPlayerN + ":" + clickedOnColor + ":");

				switch (clickedOnColor) {
				case GameController_multi.PURPLE:
					gameController.guruPickedPurple = true;
					break;
				case GameController_multi.YELLOW:
					gameController.guruPickedYellow = true;
					break;
				case GameController_multi.ORANGE:
					gameController.guruPickedOrange = true;
					break;
				case GameController_multi.RED:
					gameController.guruPickedRed = true;
					break;
				case GameController_multi.GREEN:
					gameController.guruPickedGreen = true;
					break;
				case GameController_multi.BROWN:
					gameController.guruPickedBrown = true;
					break;
				case GameController_multi.BLUE:
					gameController.guruPickedBlue = true;
					break;
				}
			
				state = 8;
			} else {
				state = 40;
			}
		} else {
			state = 8;
		}
		activityResult = 1;
		fader2.fadeOutTask (this);

	}

	public void clickOnArrow() {
		activityResult = 0;
		fader2.fadeOutTask (this);
		state = 8;
	}
}

