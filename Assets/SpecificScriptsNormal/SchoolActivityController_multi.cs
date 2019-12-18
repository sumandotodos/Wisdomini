using UnityEngine;
using System.Collections;


public class SchoolActivityController_multi : Task {

	public UITip chooseEnergyTip;
	public GameController_multi gameController;
	public CircpleDeploy[] playerElement;
	public CircpleDeploy[] energyElement;
	public CircpleDeploy waitYinYang;
	public CircpleDeploy waitText;
	int playerTouched;
	int energyTouched;

	public UIScaleFader noSabiduriaScaler;

	int state = 0; 	// 0 : idling
					// 1 : waiting for yin yang selectiong
					// 2 : 

	void init() {

	}

	public void startSchoolTask(Task w) {

		noSabiduriaScaler.Start ();
		noSabiduriaScaler.scaleOutImmediately ();

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		int index = 0;



		// disable all player elements
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			playerElement [i].gameObject.SetActive (false);
		}

		// disable all energy elements
		for (int i = 0; i < GameController_multi.MaxEnergies; ++i) {
			energyElement [i].gameObject.SetActive (false);
		}


		//if (gameController.nPlayers > 2) {

			// selectively re-enable player elements
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			
				if (gameController.playerPresent [i] && i != gameController.localPlayerN) {
					playerElement [i].gameObject.SetActive (true);
					playerElement [i].setNElements (gameController.nPlayers - 1);
					playerElement [i].setIndex (index);
					playerElement [i].extend ();
					++index;
				}

			}

		//} else {
			state = 0;
		//}

	}

	void shrinkPlayerCircle() {
		
		for (int i = 0; i < playerElement.Length; ++i) {
			if(playerElement[i].gameObject.activeInHierarchy)
			playerElement [i].retract();
		}
		playerElement [playerTouched].retractTask (this);
	}

	void shrinkEnergyCircle() {
		
		for (int i = 0; i < energyElement.Length; ++i) {
			if(energyElement[i].gameObject.activeInHierarchy)
			energyElement [i].retract();
		}
		energyElement [energyTouched].retractTask (this);
	}

	// Use this for initialization
	void Start () {
		/*gameController.Start ();
		gameController.nPlayers = 4;
		gameController.playerPresent [0] = true;
		gameController.playerPresent [1] = true;
		gameController.playerPresent [2] = true;
		gameController.playerPresent [3] = true;
		gameController.playerList [0].mainWisdom = 2;
		gameController.playerList [1].mainWisdom = 4;
		gameController.playerList [2].mainWisdom = 3;
		gameController.playerList [3].mainWisdom = 6;
		startSchoolTask (this);*/
	}

	public void touchPlayer0() {

		chooseEnergyTip.show ();
		playerTouched = 0;
		shrinkPlayerCircle ();
		if(state == 0)
		state = 1;
	}

	public void touchPlayer1() {

		chooseEnergyTip.show ();
		playerTouched = 1;
		shrinkPlayerCircle ();
		if(state == 0)
		state = 1;
	}

	public void touchPlayer2() {

		chooseEnergyTip.show ();
		playerTouched = 2;
		shrinkPlayerCircle ();
		if(state == 0)
		state = 1;
	}

	public void touchPlayer3() {

		chooseEnergyTip.show ();
		playerTouched = 3;
		shrinkPlayerCircle ();
		if(state == 0)
		state = 1;
	}

	public void touchPlayer4() {

		chooseEnergyTip.show ();
		playerTouched = 4;
		shrinkPlayerCircle ();
		if(state == 0)
			state = 1;
	}

	public void touchPlayer5() {

		chooseEnergyTip.show ();
		playerTouched = 5;
		shrinkPlayerCircle ();
		if(state == 0)
			state = 1;
	}

	public void touchEnergy0() {
		
		energyTouched = 0;
		shrinkEnergyCircle ();
		if(state == 3)
			state = 4;
	}

	public void touchEnergy1() {
		
		energyTouched = 1;
		shrinkEnergyCircle ();
		if(state == 3)
			state = 4;
	}

	public void touchEnergy2() {
		
		energyTouched = 2;
		shrinkEnergyCircle ();
		if(state == 3)
			state = 4;
	}

	public void touchEnergy3() {
		
		energyTouched = 3;
		shrinkEnergyCircle ();
		if(state == 3)
			state = 4;
	}

	public void touchEnergy4() {
		
		energyTouched = 4;
		shrinkEnergyCircle ();
		if(state == 3)
			state = 4;
	}

	public void touchEnergy5() {
		
		energyTouched = 5;
		shrinkEnergyCircle ();
		if(state == 3)
			state = 4;
	}

	public void touchEnergy6() {

		energyTouched = 6;
		shrinkEnergyCircle ();
		if(state == 3) 
			state = 4;
	}
	float remaining = 0f;
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idling

		}
		if (state == 1) { // waiting for circle to shrink
			if (!isWaitingForTaskToComplete) {
				state = 2;
				for (int i = 0; i < GameController_multi.MaxPlayers; ++i) { // tell non playing players to show the gallery
					if (gameController.playerPresent [i] && (i != gameController.localPlayerN) && (i != playerTouched)) {
						gameController.networkAgent.sendCommand (i, "showgallery");
					}
				}
			}
		}
		if (state == 2) { // deploy energies

			// remove these after testing
			//gameController.playerList [0].mainWisdom = 2;
			//gameController.playerList [1].mainWisdom = 4;
			//gameController.playerList [2].mainWisdom = 3;
			//gameController.playerList [3].mainWisdom = 6;
			//gameController.playerList [3].hasSecondariWisdoms = true;

			int playerNOfWisdoms = gameController.playerList [playerTouched].numberOfWisdoms ();
			if (playerNOfWisdoms == 0) {
				// end of thing
				noSabiduriaScaler.scaleIn();
				state = 100;
				remaining = 5f;
				//notifyFinishTask();
				//state = 0;
			} else {

//				gameController.playerList [playerTouched].addTraining (); // local
//				gameController.networkAgent.broadcast("addtraining:" + playerTouched + ":"); // remote


				int nEnergies = playerNOfWisdoms;


				// enable selectively
				int index = 0;
				for (int i = 0; i < energyElement.Length; ++i) {
					//if ((gameController.playerList [playerTouched].mainWisdom == i) || (gameController.playerList [playerTouched].hasSecondaryWisdoms && !GameController_multi.isPrincipalWisdom (i))) {
					if(gameController.playerList[playerTouched].wisdoms.Contains(i)) {
						energyElement [i].gameObject.SetActive (true);
						energyElement [i].setNElements (nEnergies);
						energyElement [i].setIndex (index++);
						energyElement [i].extend ();
					}
				}
				state = 3;
			}
		}
		if (state == 3) { // waiting for user interaction

		}
		if (state == 4) { // waiting for energy circle to shrink
			if (!isWaitingForTaskToComplete) {
				waitYinYang.extend ();
				waitText.extend ();
				state = 5;
				gameController.networkAgent.sendCommand (gameController.playerList [playerTouched].id, "schooltest:" + gameController.localPlayerN + ":" + energyTouched + ":");
			}
		}
		if (state == 5) { // wait until the other players frees us

		}
		if (state == 100) {
			remaining -= Time.deltaTime;
			if (remaining < 0f) {
				state = 101;

			}
		}
		if (state == 101) {
			notifyFinishTask ();
			state = 0;
		}

	}

	/* network callbacks */
	public void finishSchool(int score, int individual) {
		if (state == 5) {
			if (score == 0) {

			} else if (score == 1) {

			}
			state = 0;
			waitYinYang.reset ();
			waitText.reset ();
			notifyFinishTask ();
		}
	}
}
	