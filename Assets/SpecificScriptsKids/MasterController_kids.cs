using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MasterController_kids : MasterController {


	public UIFaderScript globalFader;

	/* references */

	public GameObject debugPanelMBO;

	public RawImage playerImage;
	public Texture[] playerTexture;

	public GameObject upgradeCanvas;
	public UIScaleFader upgradeNoticeScaler;

	const float maxDoubleTapDelay = 0.25f;
	float doubleTapElapsedTime = 0;
	public GameObject blackScreenOfDeath;

	bool showingService = false;
	public UIScaleFader servicePanel;

	public UIFaderScript gearFader;
	public UIDelayFader gearDelay;

	public AudioManager aManager;

	public TitlesController_kids titlesController;
	//public NewGameActivityController newGameController_multi;
	//public JoinGameActivityController joinGameController_multi;
	//public PlayerScanner yinYangScannerController;
	public PlayerSelectController_multi playerSelectController;
	public PlayerActivityController_multi playerActivityController;
	public FinishActivityController_multi finishActivityController;
	public ContinueGameController continueGameController_multi;
	public GameController_multi gameController;
	public FGBetterNetworkAgent networkAgent;

	public ValorationController_multi valorationController;

	public GameObject valorationActivity;
	public GameObject volcanoActivity;
	public GameObject titlesActivity;
	public GameObject newGameActivity;
	public GameObject joinGameActivity;
	public GameObject continueGameActivity;
	public GameObject scanYinYangActivity;
	public GameObject MainGame;
	public GameObject finishActivity;



	bool DEBUG = false;
	// these are for debug purposes only
	public WorkActivityController_multi workActivityController;
	public Canvas workActivityCanvas;
	public Canvas myTurnCanvas;
	public Canvas notMyTurnCanvas;







	/* state */

	public int state0;
	public int substate0;
	public float timer0;


	/* constants */

	const float InitialDelay = 0.1f;


	public override void playSound(AudioClip clip)  {
		aManager.playSound (clip);
	}

	public static bool mustShowHelp() {
		if (theInstance != null) {
			return theInstance.showHelpToggle.isOn;
		} else
			return false;
	}

	public void setRosettaLang(int l) {
		GameObject.Find ("RosettaWrapper").GetComponent<RosettaWrapper> ().rosetta.setTranslation (l);
		gameController.quickSaveInfo.currentTranslation = l;
		gameController.saveQuickSaveInfo ();
		toggleServicePanel ();
		playSound (changeLangAudioClip);
	}

	public void toggleCallback() {
		gameController.tipSaveData.showTips = showHelpToggle.isOn;
		gameController.saveTipSaveData ();
	}

	public override void enableWait() {
		waitThing.SetActive (true);
	}

	public override void disableWait() {
		waitThing.SetActive (false);
	}

	// Use this for initialization
	void Start () {

		theInstance = this;
		disableWait ();
		gameController.loadTipSaveData ();
		showHelpToggle.isOn = gameController.tipSaveData.showTips;

		debugPanelMBO.SetActive (true);

		blackScreenOfDeath.SetActive (false);

		playerImage.enabled = false;

		upgradeCanvas.SetActive (false);
		WWWForm myWWWForm = new WWWForm ();
		myWWWForm.AddField ("app", "Wis");
		WWW myWWW = new WWW ("https://apps.flygames.org" + "/getMinimumBuild.php", myWWWForm);
		while (!myWWW.isDone) { } // oh, no, don't!!
		if (!myWWW.text.Equals ("")) {
			int minimumBuild;
			int.TryParse (myWWW.text, out minimumBuild);
			if (minimumBuild > Utils.build) {
				upgradeCanvas.SetActive (true);
				upgradeNoticeScaler.Start ();
				upgradeNoticeScaler.scaleIn ();
			}
		}

		//titlesController.titlesGoTask (this);
		state0 = 0;
		substate0 = 0;
		timer0 = 0.0f;

		showingService = false;
		servicePanel.scaleOutImmediately ();

		MainGame.SetActive (false);
		newGameActivity.SetActive (false);
		joinGameActivity.SetActive (false);
		scanYinYangActivity.SetActive (false);
		titlesActivity.SetActive (true);

		Screen.sleepTimeout = SleepTimeout.NeverSleep;




	}

	public void toggleServicePanel() {

		if (showingService) {
			showingService = false;
			servicePanel.scaleOut ();
		} else {
			showingService = true;
			servicePanel.scaleIn ();
		}

		gearDelay.resetTimer ();
		gearDelay.going = !gearDelay.going;

	}

	public override void hardReset() {

		networkAgent.disconnect ();
		blackScreenOfDeath.SetActive (true);
		timer0 = 0.25f;
		state0 = 666;


		//yield return loadAll;

	}

	public void showGear() {
		gearFader.Start ();
		gearDelay.resetTimer ();
		gearDelay.going = true;
		gearFader.fadeOut ();
	}

	// Update is called once per frame
	void Update () {

		doubleTapElapsedTime += Time.deltaTime;
		if (Input.GetMouseButtonDown (0)) {
			if (doubleTapElapsedTime < maxDoubleTapDelay) {
				showGear ();
			}
			doubleTapElapsedTime = 0.0f;
		}

		if (state0 == 666) {
			timer0 -= Time.deltaTime;
			if (timer0 < 0.0f) {
				networkAgent.disconnect ();
				timer0 = 0.25f;
				state0 = 667;
			}
		}
		if (state0 == 667) {
			timer0 -= Time.deltaTime;
			if (timer0 < 0.0f) {
				SceneManager.LoadScene ("Scenes/Selector");
			}
		}

		if (state0 == 0) { // small initial delay
			timer0 += Time.deltaTime;
			if (timer0 > InitialDelay) {
				timer0 = 0.0f;
				state0 = 100;
				if (DEBUG) { // quick debug

					MainGame.SetActive (true);
					workActivityCanvas.enabled = false;
					myTurnCanvas.enabled = false;
					notMyTurnCanvas.enabled = true;
					//workActivityController.startWorkActivityTask (this);

				} else {
					state0 = 100;
					startActivity = "Titles";
				}
			}
		}



		else if (state0 == 100) { // waiting for activity to finish
			if (!isWaitingForTaskToComplete) {
				if (startActivity.Equals ("Titles")) {
					newGameActivity.SetActive (false);
					joinGameActivity.SetActive (false);
					titlesActivity.SetActive (true);
					titlesController.titlesGoTask (this); // launch titles
				}
				if (startActivity.Equals ("StartNewGame")) {
					titlesActivity.SetActive (false);
					newGameActivity.SetActive (true);
					//newGameController_multi.startNewGameTask (this);
				}
				if (startActivity.Equals ("JoinNewGame")) {
					titlesActivity.SetActive (false);
					joinGameActivity.SetActive (true);
					//joinGameController_multi.startJoinGameTask (this);
				}
				if (startActivity.Equals ("ContinueGame")) {
					titlesActivity.SetActive (false);
					continueGameActivity.SetActive (true);
					continueGameController_multi.startContinueGame (this);
				}
				if (startActivity.Equals ("ScanPlayers")) {
					joinGameActivity.SetActive (false);
					newGameActivity.SetActive (false);
					//					scanYinYangActivity.SetActive (true);
					//					yinYangScannerController.startGetPlayerTask (this);
					playerSelectController.startPlayerSelectActivity(this);
				}
				if (startActivity.Equals ("MainGame")) {
					//scanYinYangActivity.SetActive (false);
					continueGameActivity.SetActive (false);
					globalFader.setFadeValue (0f);
					MainGame.SetActive (true);
					playerActivityController.startMainGameTask (this);

				}
				if (startActivity.Equals ("ShowResults")) {
					screenWidth = Screen.width;
					screenHeight = Screen.height;
					scanYinYangActivity.SetActive (false);
					MainGame.SetActive (false);
					finishActivity.SetActive (true);
					finishActivityController.startFinishTask (this);
					gameController.resetQuickSaveInfo ();

				}
				if (startActivity.Equals ("Valoration")) {
					//screenWidth = Screen.width;
					//screenHeight = Screen.height;
					//scanYinYangActivity.SetActive (false);
					//MainGame.SetActive (false);
					//finishActivity.SetActive (true);
					//finishActivityController.startFinishTask (this);
					volcanoActivity.SetActive(false);
					finishActivity.SetActive(false);
					MainGame.SetActive (true);
					valorationActivity.SetActive(true);
					valorationController.startValorationTask(this);
				}
				if (startActivity.Equals ("ResetGame")) {
					state0 = 0;
					substate0 = 0;
					timer0 = 0.0f;
					//networkAgent.sendMessage ("end");
					//networkAgent.disconnect ();
					gameController.resetGame ();
					MainGame.SetActive (false);
					finishActivity.SetActive (false);
					newGameActivity.SetActive (false);
					joinGameActivity.SetActive (false);
					scanYinYangActivity.SetActive (false);
					titlesActivity.SetActive (true);
					hardReset (); // a tomar por culo todo
				}
			}

		}

	}

	public void resetHelp() {
		gameController.tipSaveData.dismissedTips = new List<string> ();
		gameController.saveTipSaveData ();
		toggleServicePanel ();
	}
}

