using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterController_kidsmono : MasterController {

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

	//public TitlesController_monoNormal titlesController;
	public TitlesController_kidsmono titlesController;

	public PlayerActivityController_mono playerActivityController;
	public FinishActivityController_mono finishActivityController;

	public GameController_mono gameController;


	public ValorationController_mono valorationController;

	public GameObject valorationActivity;
	public GameObject volcanoActivity;
	public GameObject titlesActivity;
	public GameObject MainGame;
	public GameObject finishActivity;



	bool DEBUG = false;
	// these are for debug purposes only
	public WorkActivityController_mono workActivityController;
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

		//titlesController.titlesGoTask (this);
		state0 = 0;
		substate0 = 0;
		timer0 = 0.0f;

		showingService = false;
		servicePanel.scaleOutImmediately ();

		MainGame.SetActive (false);
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
					
					titlesActivity.SetActive (true);
					titlesController.titlesGoTask (this); // launch titles
				}

				if (startActivity.Equals ("MainGame")) {
					//scanYinYangActivity.SetActive (false);

					globalFader.setFadeValue (0f);
					MainGame.SetActive (true);
					gameController.localPlayerN = 0;
					playerActivityController.startMainGameTask (this);

				}
				if (startActivity.Equals ("ShowResults")) {
					screenWidth = Screen.width;
					screenHeight = Screen.height;
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

					//networkAgent.disconnect ();
					gameController.resetGame ();
					MainGame.SetActive (false);
					finishActivity.SetActive (false);

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
