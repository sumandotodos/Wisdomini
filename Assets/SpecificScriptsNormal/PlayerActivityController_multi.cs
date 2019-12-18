using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerActivityController_multi : Task {

	public GameController_multi gameController;
	public RankingController_multi rankingController;

	public GameObject myTurnCanvas;
	public GameObject notMyTurnCanvas;

	public WisButton_multi guruButton;

	public Text amountOfCoinsText;
	public Text amountOfSeedsText;

	public GameObject gompaActivityCanvas;
	public GameObject workActivityCanvas;
	public GameObject lifeTestActivity;
	public GameObject myTurnSchoolActivity;
	public GameObject notMyTurnSchoolActivity;
	public GameObject forestBuyActivity;
	public GameObject guruActivity;
	public GameObject notMyTurnGuruActivity;
	public WorkActivityController_multi workActivityController;
	public LifeTestActivityController_multi lifeTestActivityController;
	public VolcanoActivityController_multi volcanoActivityController;
	public GompaLiteController_multi gompaController;
	public SchoolActivityController_multi myTurnSchoolActivityController_multi;
	public NotMySchoolController_multi notMyTurnActivityController;
	public BuyForestActivityController_multi forestActivityController;
	public GuruActivityController_multi guruActivityController;
	public NotMyTurnGuruActivityController_multi notMyGuruActivityController_multi;
	public Text timesRemaining;


	public GameObject waitForConfirmation;

	public GameObject volcanoActivityCanvas;


	public RawImage playerFrameTop;
	public RawImage playerFrameBottom;
	public Texture[] playerFrameTopTexColor;
	public Texture[] playerFrameBottomTexColor;
	public WisButton_multi[] button;

	public AudioClip OKSound;
	public UIFaderScript myTurnFader;
	public UIFaderScript notMyTurnFader;

	public NotMyTurnController_multi notMyTurnController;


	public Text goldText;


	const int ButtonTest = 0;
	const int ButtonWork = 1;
	const int ButtonGompa = 2;
	const int ButtonSchool = 3;
	const int ButtonGuru = 4;
	const int ButtonVolcano = 5;
	const int ButtonBuild = 6;
	const int ButtonCoin = 7;

	public int volcanoResult = 0;

	int state = 0; // idle

	bool isMyTurn;

	//turn status
	int obtainedVictory = 0;
	int obtainedDefeat = 0;
	int obtainedYinYang = 0;
	[HideInInspector]
	public int obtainedImmunity = 0;
	[HideInInspector]
	public int obtainedMaru = 0;
	[HideInInspector]
	public int obtainedTick = 0;

	public void setButtonPressed(int but) {

		if (but == ButtonTest) {
			button [ButtonSchool].deactivate ();
			button [ButtonGuru].deactivate ();
			button [ButtonVolcano].deactivate ();
			button [ButtonCoin].deactivate ();
			button [ButtonGompa].deactivate ();
		} 
		else if (but == ButtonSchool) {
			button [ButtonTest].deactivate ();
			button [ButtonGuru].deactivate ();
			button [ButtonVolcano].deactivate ();
			button [ButtonCoin].deactivate ();
			button [ButtonGompa].deactivate ();

		} else if (but == ButtonBuild) {
			button [ButtonVolcano].deactivate ();
		}


		else if (but == ButtonGuru) {
			button [ButtonGompa].deactivate ();
			timesRemaining.color = Color.black;
			button [ButtonSchool].deactivate ();
			button [ButtonTest].deactivate ();
			button [ButtonWork].deactivate ();
			button [ButtonVolcano].deactivate ();
			button [ButtonCoin].deactivate ();
		} 

		else if (but == ButtonVolcano) {

			button [ButtonGompa].deactivate ();
			timesRemaining.color = Color.black;
			button [ButtonSchool].deactivate ();
			button [ButtonTest].deactivate ();
			button [ButtonWork].deactivate ();
			button [ButtonGuru].deactivate ();
			button [ButtonBuild].deactivate ();
			button [ButtonCoin].deactivate ();
		} 

		else if (but == ButtonWork) {

			button [ButtonGompa].deactivate ();
			timesRemaining.color = Color.black;
			button [ButtonVolcano].deactivate ();

		} 

		else if (but == ButtonCoin) {

			button [ButtonGompa].deactivate ();
			timesRemaining.color = Color.black;
			button [ButtonSchool].deactivate ();
			button [ButtonTest].deactivate ();
			button [ButtonWork].deactivate ();
			button [ButtonGuru].deactivate ();
			button [ButtonVolcano].deactivate ();

		} 

		else if (but == ButtonGompa) { // Ahora es barco

			button [ButtonGuru].deactivate ();
			button [ButtonVolcano].deactivate ();
			button [ButtonWork].deactivate ();
			button [ButtonTest].deactivate ();
			button [ButtonSchool].deactivate ();
			button [ButtonCoin].deactivate ();
			if (timesRemaining.color == Color.red)
				timesRemaining.color = Color.black;
			else
			timesRemaining.color = Color.red;

		}

	}

	public void startPanel() {

		// start all activities
		gameController.masterController.playSound(OKSound);
		myTurnFader.fadeOutTask (this);
		state = 1; // 'my turn'

	}

	public void clearPanel() {
		button [ButtonGompa].deactivate ();
		timesRemaining.color = Color.black;
		button [ButtonSchool].deactivate ();
		button [ButtonTest].deactivate ();
		button [ButtonWork].deactivate ();
		button [ButtonVolcano].deactivate ();
		button [ButtonGuru].deactivate ();
		button [ButtonBuild].deactivate ();
		button [ButtonCoin].deactivate ();
	}

	// Use this for initialization
	void Start () {



	}

	public void initialize(bool rollback) {

		guruButton.going = (gameController.playerList [gameController.localPlayerN].mainWisdom == -1);

		amountOfCoinsText.text = "x " + gameController.playerList [gameController.localPlayerN].gold;
		amountOfSeedsText.text = "x " + gameController.playerList [gameController.localPlayerN].seeds;

		myTurnFader.Start ();
		notMyTurnFader.Start ();

		notMyTurnController.reset ();

		// disable all other canvases/panels
		lifeTestActivity.SetActive (false);
		workActivityCanvas.SetActive (false);
		waitForConfirmation.SetActive (false);
		gompaActivityCanvas.SetActive (false);
		forestBuyActivity.SetActive (false);
		guruActivity.SetActive (false);
		notMyTurnGuruActivity.SetActive (false);
		timesRemaining.color = Color.black;
		int immunityRemain = (gameController.playerList [gameController.localPlayerN].seeds) / 7 - gameController.playerList [gameController.localPlayerN].spentImmunity;
		if (immunityRemain < 0)
			immunityRemain = 0;
		timesRemaining.text = "" + immunityRemain;

		// reset all merits
		obtainedVictory = 0;
		obtainedDefeat = 0;
		obtainedYinYang = 0;
		obtainedMaru = 0;
		obtainedTick = 0;
		obtainedImmunity = 0;

		// reset player turn confirmation votations
		for (int i = 0; i < gameController.PlayerTurnVotation.Count; ++i) {
			gameController.PlayerTurnVotation [i] = 0;
		}

		if (rollback)
			gameController.rollbackTurn ();
		else 
			gameController.nextTurn ();



		if (gameController.localPlayerN == gameController.playerTurn) { // if it is 'my' turn

			isMyTurn = true;

			// change frame to 'my' color
			//playerMaterial.SetColor ("_TintColor", gameController.colorFromPlayerN(gameController.localPlayerN));
			playerFrameTop.texture = playerFrameTopTexColor[gameController.localPlayerN];
			playerFrameBottom.texture = playerFrameBottomTexColor[gameController.localPlayerN];

			goldText.text = "" + gameController.playerList [gameController.localPlayerN].gold;

			myTurnCanvas.SetActive (true);
			notMyTurnCanvas.SetActive (false);

			clearPanel ();

		} 

		else { // not 'my' turn

			isMyTurn = false;

			myTurnCanvas.SetActive (false);
			notMyTurnCanvas.SetActive (true);
			notMyTurnController.init ();

		}

		workActivityCanvas.SetActive (false);
		volcanoActivityCanvas.SetActive (false);

		if (isMyTurn) {
			myTurnFader.fadeIn ();
		} else
			notMyTurnFader.fadeIn ();

	}

	public void startMainGameTask(Task w) {
		Debug.Log ("<color=blue>MainGameTask started</color>");
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		//gameController.resetGame ();
		initialize (false);
		gameController.gameState = 2; // actually playing
	}

	/* network callback. only not my turn's will receive this */
	public void startVolcano()
	{
		notMyTurnController.disableAll ();
		volcanoActivityCanvas.SetActive (true);
		volcanoActivityController.startVolcanoTask (this);
		state = 21;
	}
	
	void Update () 
	{	
		if (state == 0) { // not my turn's will sit here

		} 

		// chain-check all activated buttons and call the appropriate sub-Task
		else if (state == 1) { // waiting 'myTurn' for fadeout to finish
			if (!isWaitingForTaskToComplete) {

				if (button [ButtonWork].isActivated ()) { // see if button WORK is pressed

					myTurnCanvas.SetActive (false);
					workActivityCanvas.SetActive (true);
					workActivityController.startWorkActivityTask (this); // start work activity
			
					state = 2;

				} else
					state = 3; // check LIFE TEST
			}

		} else if (state == 2) { // waiting for work activity to finish
			if (!isWaitingForTaskToComplete) {
				workActivityCanvas.SetActive (false);
				state = 3;
			}
		} else if (state == 3) { // check LIFE TEST
			
			if (button [ButtonTest].isActivated ()) { // see if button TEST is pressed
				
				myTurnCanvas.SetActive (false);
				lifeTestActivity.SetActive (true);

				// choose situation
				int sabiduria, sabio, question;
				//lifeTestActivityController.selectSituation (out sabiduria, out sabio, out question);
				int n = lifeTestActivityController.selectSituationFromFGTable ();

				lifeTestActivityController.startLifeTestActivity (this, 0, 0, n, true); // start lifetest activity
				gameController.networkAgent.broadcast ("nonturnlifetest:" + 0 + ":" + 0 + ":" + n + ":");
				//button [ButtonWork].enabled = false;
				state = 4;

			} else
				state = 5; // check GOMPA
			
		} else if (state == 4) { // waiting for lifetest activity to finish
			if (!isWaitingForTaskToComplete) {
				lifeTestActivity.SetActive (false);
				if(lifeTestActivityController.result() == 1)
					obtainedVictory = 1;
				if (lifeTestActivityController.result () == -1)
					obtainedDefeat = 1;
				state = 5;
			}
		} else if (state == 5) { // check GOMPA

//			if (button [ButtonGompa].isActivated ()) { // see if button GOMPA is pressed
//
//				myTurnCanvas.SetActive (false);
//				gompaActivityCanvas.SetActive (true);
//				//Debug.Log ("Punto raro");
//				gompaController.startGompaTask (this); // start gompa activity
//				state = 6;
//
//			} else
				state = 7; // check SCHOOL

		} else if (state == 6) { // waiting for gompa activity to finish
			if (!isWaitingForTaskToComplete) {
				gompaActivityCanvas.SetActive (false);
				state = 7;
			}
		} else if (state == 7) { // check SCHOOL

			if (button [ButtonSchool].isActivated ()) { // see if button SCHOOL is pressed

				myTurnCanvas.SetActive (false);
				myTurnSchoolActivity.SetActive (true);
				myTurnSchoolActivityController_multi.startSchoolTask (this);

				state = 8;

			} else
				state = 9; // check BUILD

		} else if (state == 8) { // waiting for school activity to finish
			if (!isWaitingForTaskToComplete) {
				myTurnSchoolActivity.SetActive (false);

				state = 9;
			}
		} else if (state == 9) { // check BUILD

			if (button [ButtonBuild].isActivated ()) { // see if button BUILD is pressed

				myTurnCanvas.SetActive (false);
				forestBuyActivity.SetActive (true);
				forestActivityController.startBuyActivityTask (this);

				state = 10;

			} else
				state = 11; // check GURU

		} else if (state == 10) { // waiting for build activity to finish
			if (!isWaitingForTaskToComplete) {
				forestBuyActivity.SetActive (false);

				state = 11;
			}
		} else if (state == 11) { // check GURU

			if (button [ButtonGuru].isActivated ()) { // see if button GURU is pressed

				myTurnCanvas.SetActive (false);
				guruActivity.SetActive (true);
				guruActivityController.startGuruTask (this, true); 

				state = 12;

			} else
				state = 13; // check COIN

		} else if (state == 12) { // waiting for build activity to finish
			if (!isWaitingForTaskToComplete) {
				obtainedYinYang = guruActivityController.result ();
				guruActivity.SetActive (false);
				state = 13;
			}
		}

		else if (state == 13) { // check COIN

			if (button [ButtonGompa].isActivated ()) { // see if button COIN is pressed

				myTurnCanvas.SetActive (false);
				guruActivity.SetActive (true);
				guruActivityController.startGuruTask (this, false); // CAMBIADO

				state = 14;

			} else
				state = 20; // check VOLCANO

		} else if (state == 14) { // waiting for build activity to finish
			if (!isWaitingForTaskToComplete) {
				obtainedYinYang = guruActivityController.result ();
				guruActivity.SetActive (false);
				state = 20;
			}
		}

		//
		else if (state == 20) {
			if (button [ButtonVolcano].isActivated ()) { // see if button VOLCANO is pressed

				myTurnCanvas.SetActive (false);
				volcanoActivityCanvas.SetActive (true);
				volcanoActivityController.startVolcanoTask (this);
				gameController.playerList [gameController.localPlayerN].firstToVolcano = true;
				gameController.playerList [gameController.localPlayerN].volcanoReady = true;
				state = 21;
			} else
				state = 100; // go to end of turn
		}

		//
		else if (state == 21) { // waiting for volcano task to finish
			if (!isWaitingForTaskToComplete) 
			{
				if (volcanoResult == 1) { // finish player activity
					if (gameController.playerList [gameController.localPlayerN].firstToVolcano == true) 
					{
						gameController.networkAgent.broadcast ("firsttovolcano:" + gameController.localPlayerN + ":");

					}

					state = 22;
				} else if (volcanoResult == -1) { // go on with our lives

					gameController.playerList [gameController.localPlayerN].firstToVolcano = false; // nobody made it
					gameController.playerList [gameController.localPlayerN].volcanoReady = false;
					// to the volcano

					if (isMyTurn) 
					{
						state = 100;
					} else
						state = 0;
				}
				volcanoResult = 0; // reset volcano result (will be set by volcanoActivity)
			}
		} 

		//
		else if (state == 22) {
			if (gameController.playerList [gameController.localPlayerN].volcanoReady) {
				gameController.masterController.startActivity = "ShowResults";
				notifyFinishTask (); // return to mastercontroller
				state = 0;
			}
		}

		// ... at the end, we can call end of turn from here
		else if (state == 100) {

			if (gameController.playerTurn == gameController.localPlayerN) {
				waitForConfirmation.SetActive (true);
			}
			
			gameController.turnVotesNO = 0;
			gameController.turnVotesOK = 0;
			int test, work, school, gompa, guru, volcano, build, coin, monedo;
			test = work = school = gompa = guru = volcano = build = coin = monedo = 0;
			if (button [ButtonTest].isActivated ())
				test = 1;
			if (button [ButtonWork].isActivated ())
				work = 1;
			if (button [ButtonSchool].isActivated ())
				school = 1;
			if (button [ButtonGompa].isActivated ())
				gompa = 1;
			if (button [ButtonGuru].isActivated ())
				guru = 1;
			if (button [ButtonVolcano].isActivated ())
				volcano = 1;
			if (button [ButtonBuild].isActivated ())
				build = 1;
			if (button [ButtonCoin].isActivated ())
				coin = 1;
			// ask confirmation from the rest of users
			gameController.networkAgent.broadcast ("report:" + gameController.localPlayerN + ":" +
				test + ":" + work + ":" + school + ":" + gompa + ":" + guru + ":" + volcano + ":" + build + ":" + coin + ":" +
				obtainedVictory + ":" + obtainedYinYang + ":" + obtainedMaru + ":" + obtainedTick + ":" + obtainedDefeat + ":" + 
			    obtainedImmunity + ":");


			state = 101;
		} else if (state == 101) { // waiting for votes

			int turnVotesNO = 0, turnVotesYES = 0;
			for (int i = 0; i < gameController.PlayerTurnVotation.Count; ++i) {
				if (gameController.PlayerTurnVotation [i] == 1)
					++turnVotesYES;
				if (gameController.PlayerTurnVotation [i] == -1)
					++turnVotesNO;
			}

			if (turnVotesNO != 0)
				state = 103;
			if (turnVotesYES != 0)
				state = 102;

		} else if (state == 102) { // tell everybody to proceed to next turn

			// ... and everybody else
			gameController.networkAgent.broadcast ("nextturn:false");

			// advance turn, me...
			nextTurn (false);

		} else if (state == 103) {
			gameController.networkAgent.broadcast ("nextturn:true");
			nextTurn (true);
		}

	}

	public void nextTurn(bool rollb) {
		state = 0;
		initialize (rollb);
	}
}
	