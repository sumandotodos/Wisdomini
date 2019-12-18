using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSelectController_multi : Task {

	public GameController_multi gameController;
	public GameObject choosePlayerScene;
	public UIFaderScript fader;
	public UIScaleFader okButtonScaler;
	public MasterController masterController;

	public TextureToggle[] peanas;
	public TextureToggle[] reflejos;

	public Texture[] players;
	public RawImage playerRI;

	int thePlayerIWant;

	public void startPlayerSelectActivity(Task w) {
		state = 1;
		waiter = w;
		w.isWaitingForTaskToComplete = true;
	}

	public void assignPlayerToServicePanel() {
		playerRI.texture = players [thePlayerIWant];
		playerRI.enabled = true;
	}

	public void sendGrabPlayerCommand() {
		gameController.networkAgent.sendCommand (0,
			"grabplayer:" + thePlayerIWant +":"+ gameController.localUserLogin + ":");
	}

	public int state = 0;
	// Use this for initialization
	void Start () {

		okButtonScaler.Start ();
		okButtonScaler.scaleOutImmediately ();
		choosePlayerScene.SetActive (false);
		state = 0;

	}

	void Update() {

		if (state == 1) {
			choosePlayerScene.SetActive (true);
			fader.setFadeValue (1.0f);
			fader.fadeIn ();
			okButtonScaler.scaleIn ();
			state = 10; // waiting for OK button...
		}



//		execute (canvasHub.choosePlayerScene, "SetActive", true);
//		execute (canvasHub.choosePlayerCanvas, "SetActive", true);
//		execute (controllerHub.uiController, "fadeIn");


//		// when I press the OK button...
//		createSubprogram ("attemptGrabPlayer");
//		// tell the master I want that player, and wait for response
//		execute (this, "sendGrabPlayerCommand");


		if (state == 20) { // player granted
			okButtonScaler.scaleOut ();
			fader.fadeOutTask(this);
			state = 21;
		}
		if (state == 21) {
			if (!isWaitingForTaskToComplete) {
				choosePlayerScene.SetActive (false);
				gameController.masterController.startActivity = "MainGame";
				gameController.syncedPlayers++;
				masterController.enableWait ();

				gameController.networkAgent.broadcast ("sync:");
				assignPlayerToServicePanel();
				state = 22;
			}
		}
		if (state == 22) {
			if (gameController.syncedPlayers == gameController.nPlayers) {
				gameController.syncedPlayers = 0;
				masterController.disableWait ();
				notifyFinishTask ();
				state = 0;
			}
		}


//		waitForTask (controllerHub.uiController, "fadeOutTask", this);
//		execute (canvasHub.choosePlayerScene, "SetActive", false);
//		execute (canvasHub.choosePlayerCanvas, "SetActive", false);
//		execute (controllerHub.uiController, "startWait");
//		execute (controllerHub.networkController, "broadcast", FGNetworkManager.makeClientCommand ("sync"));
//		execute (controllerHub.gameController, "addSyncPlayers");
//		waitForCondition (true, "==", controllerHub.gameController, "playersAreSynced"); // sync players
//		execute (controllerHub.uiController, "endWait");
//		execute (this, "assignPlayerToServicePanel");
//		programNotifyFinish (); // back to mastercontroller



		if (state == 50) { // all players ready
			notifyFinishTask ();
			state = 0;
		}


	}
		

	public void doSomething() {

	}
	
	

	// UI callbacks
	public void buttonPress(int pl) {

		thePlayerIWant = pl;
		sendGrabPlayerCommand ();

	}



	// network callbacks
	public void takePlayer(int pl, int who) {

		Debug.Log ("<color=purple>" +who+" takes player " + pl + "</color>");
		
		gameController.playerList[pl].id = who;
		gameController.playerPresent [pl] = true;

		if (gameController.localUserLogin.Equals (who)) {
			gameController.localPlayerN = pl;
			state = 20;
		} else {
			disablePlayer (pl);
		}
	}

	public void dontTakePlayer() {
		Debug.Log ("<color=purple>Can't take player</color>");
		disablePlayer (thePlayerIWant);
	}

	public void disablePlayer(int pl) {
		peanas [pl].toggleTexture ();
		reflejos [pl].toggleTexture ();
	}

	public void allPlayersReady() {
		state = 50;
	}

}
