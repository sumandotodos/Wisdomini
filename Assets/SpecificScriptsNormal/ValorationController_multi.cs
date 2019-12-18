using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ValorationController_multi : Task {

	//public Text coordianteText;

	public GameObject volcanoObject;

	public int creativityAwardedPlayer;
	public int solidarityAwardedPlayer;
	public const int PlayerNone = -1;

	public GameController_multi gameController;

	public RawImage hasReceivedSignal;

	public GameObject[] magnets;
	public UIDragIcon bulb;
	public UIDragIcon hand;

	public UIFaderScript fader;

	public Text empathyAwardText;
	public Text creativityAwardText;
	public AudioClip awardSound_N;

	public int screenWidth;
	public int screenHeight;

	Vector3 startingPosHand;
	Vector3 startingPosBulb;

	int state=0;    // 0: idle
			    	// 1: waiting for fadeout

	public bool awardedCreativity;
	public bool awardedEmpathy;

	float timer;

	bool started = false;

	public void startValorationTask(Task w) {
		//state = 0;
		if (gameController.isMaster) {
			state = 2; // waiting for votes
		} else
			state = 0;
		fader.fadeIn ();
		waiter = w;
		w.isWaitingForTaskToComplete = true;
		empathyAwardText.enabled = false;
		creativityAwardText.enabled = false;
		awardedCreativity = false;
		awardedEmpathy = false;
		gameController.creativityAwardPlayer = PlayerNone;
		gameController.empathyAwardPlayer = PlayerNone;
		timer = 0.0f;
		hasReceivedSignal.enabled = false;


		hand.transform.position = new Vector3 (-20, 20, 0) + volcanoObject.transform.position; //(screenWidth/4-20, screenHeight/4+20, 0);
		bulb.transform.position = new Vector3 (20, -20, 0) + volcanoObject.transform.position; //(screenWidth/4+20, screenHeight/4-20, 0);

	}

	// Use this for initialization
	void Start () {
	
		if (started)
			return;

		started = true;
		// -1 means none
		/*creativityAwardedPlayer = gameController.getAward(DragIconType.bulb);
		solidarityAwardedPlayer = gameController.getAward (DragIconType.hand);

		if (creativityAwardedPlayer != ValorationController_multi.PlayerNone) {
			bulb.transform.position = magnets [creativityAwardedPlayer].transform.position;
		}

		if (solidarityAwardedPlayer != ValorationController_multi.PlayerNone) {
			hand.transform.position = magnets [solidarityAwardedPlayer].transform.position;
		}*/
		startingPosBulb = new Vector3 (Screen.width/2-20, Screen.height/2+20, 0); //bulb.transform.position;
		startingPosHand = new Vector3(Screen.width+20, Screen.height-20, 0); //bulb.transform.position;

	}

	public void setAward(DragIconType iconType, int player) {
		if (iconType == DragIconType.bulb) {

			if (gameController.creativityAwardPlayer == player) // caso x->x
				return; // do nothing!
			
			else if ((gameController.creativityAwardPlayer == PlayerNone) && // caso -1 -> -1
			   player == PlayerNone)
				return;

			else if ((gameController.creativityAwardPlayer == PlayerNone) && // caso -1 -> x
			   player != PlayerNone) {

				gameController.creativityVotes++;
				if (gameController.isMaster) {
					gameController.playerList [player].creativityAwards++;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"creativityaward:" + player + ":1:");
				}

				gameController.creativityAwardPlayer = player;
			}

			else if ((gameController.creativityAwardPlayer != PlayerNone) && // caso x -> -1
			   player == PlayerNone) {

				gameController.creativityVotes--;
				if (gameController.isMaster) {
					gameController.playerList [gameController.creativityAwardPlayer].creativityAwards--;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"creativityaward:" + gameController.creativityAwardPlayer + ":-1:");
				}

				gameController.creativityAwardPlayer = player;

			}

			else if ((gameController.creativityAwardPlayer != PlayerNone) && // caso x -> y
				player == PlayerNone) {

				gameController.creativityVotes--;
				if (gameController.isMaster) {
					gameController.playerList [gameController.creativityAwardPlayer].creativityAwards--;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"creativityaward:" + gameController.creativityAwardPlayer + ":-1:");
				}

				gameController.creativityVotes++;
				if (gameController.isMaster) {
					gameController.playerList [player].creativityAwards++;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"creativityaward:" + player + ":1:");
				}

				gameController.creativityAwardPlayer = player;

			}



		}



		if (iconType == DragIconType.hand) {

			if (gameController.empathyAwardPlayer == player) // caso x->x
				return; // do nothing!

			else if ((gameController.empathyAwardPlayer == PlayerNone) && // caso -1 -> -1
				player == PlayerNone)
				return;

			else if ((gameController.empathyAwardPlayer == PlayerNone) && // caso -1 -> x
				player != PlayerNone) {

				gameController.empathyVotes++;
				if (gameController.isMaster) {
					gameController.playerList [player].empathyAwards++;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"empathyaward:" + player + ":1:");
				}

				gameController.empathyAwardPlayer = player;
			}

			else if ((gameController.empathyAwardPlayer != PlayerNone) && // caso x -> -1
				player == PlayerNone) {

				gameController.empathyVotes--;
				if (gameController.isMaster) {
					gameController.playerList [gameController.empathyAwardPlayer].empathyAwards--;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"empathyaward:" + gameController.empathyAwardPlayer + ":-1:");
				}

				gameController.empathyAwardPlayer = player;

			}

			else if ((gameController.empathyAwardPlayer != PlayerNone) && // caso x -> y
				player == PlayerNone) {

				gameController.empathyVotes--;
				if (gameController.isMaster) {
					gameController.playerList [gameController.empathyAwardPlayer].empathyAwards--;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"empathyaward:" + gameController.empathyAwardPlayer + ":-1:");
				}

				gameController.empathyVotes++;
				if (gameController.isMaster) {
					gameController.playerList [player].empathyAwards++;
				} else {
					gameController.networkAgent.sendCommand (0, 
						"empathyaward:" + player + ":1:");
				}

				gameController.empathyAwardPlayer = player;

			}



		}



	}

	public void backArrowClicked() {
		fader.fadeOutTask (this);
		state = 1;
	}

	public void receiveAward(bool empathy, bool creativity) {
		awardedEmpathy |= empathy;
		awardedCreativity |= creativity;
	}

	void Update() {

		//coordianteText.text = "" + bulb.transform.position;

		//if (Input.GetKeyDown (KeyCode.Escape)) {
		//	backArrowClicked ();
		//}

		if (state == 0) { // idle state, for slaves

		}

		// waiting for fadeout
		else if (state == 1) {

			if (!isWaitingForTaskToComplete) {
				notifyFinishTask (); // return control to parent controller
			}

		}

		// master waiting for nPlayer votes on both cathegories
		else if (state == 2) {
			if ((gameController.empathyVotes == gameController.nPlayers) &&
			    (gameController.creativityVotes == gameController.nPlayers)) {

				// check players with better score
				int maxEmpathy = -1;
				int maxCreativity = -1;
				int nEmpathy = 1;
				int nCreativity = 1;
				int maxCreativityPlayer = -1;
				int maxEmpathyPlayer = -1;
				for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
					if (gameController.playerPresent [i]) {
						if (gameController.playerList [i].empathyAwards > maxEmpathy) {
							nEmpathy = 1;
							maxEmpathyPlayer = i;
							maxEmpathy = gameController.playerList [i].empathyAwards;
						} else if (gameController.playerList [i].empathyAwards == maxEmpathy) {
							++nEmpathy;
						}

						if (gameController.playerList [i].creativityAwards > maxCreativity) {
							nCreativity = 1;
							maxCreativityPlayer = i;
							maxCreativity = gameController.playerList [i].creativityAwards;
						} else if (gameController.playerList [i].creativityAwards == maxCreativity) {
							++nCreativity;
						}
					}
				}

				// send messages
				if (nEmpathy == 1) {
					gameController.networkAgent.sendCommand (gameController.playerList [maxEmpathyPlayer].id, 
						"receiveaward:empathy:");
				}
				if (nCreativity == 1) {
					gameController.networkAgent.sendCommand (gameController.playerList [maxCreativityPlayer].id, 
						"receiveaward:creativity:");
				}
				for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
					if (gameController.playerPresent [i] && (i != gameController.localPlayerN)) {
						gameController.networkAgent.sendCommand (gameController.playerList [i].id, 
							"finishvaloration:");
					}
				}
				//gameController.networkAgent.broadcast ("finishvaloration:");
				gameController.networkAgent.sendCommand (0, "finishvaloration:");
				timer = 0.0f;
				state = 0;
			}
		}

		//processing awards. check creativity
		else if (state == 3) {
			if (awardedCreativity) {
				creativityAwardText.enabled = true;
				if (awardSound_N != null) {
					gameController.masterController.playSound (awardSound_N);
				}
				state = 4;
			} else
				state = 5;
		}

		// wait a little bit, touch cancellable
		else if (state == 4) {
			timer += Time.deltaTime;
			if (timer > 2.0f) {
				state = 5;
				timer = 0.0f;
			}
			if (Input.GetMouseButtonDown (0))
				timer += 1.0f;
		}

		// check empathy
		else if (state == 5) {
			if (awardedEmpathy) {
				empathyAwardText.enabled = true;
				if (awardSound_N != null) {
					gameController.masterController.playSound (awardSound_N);
				}
				state = 6;
			} else
				state = 7;
		}

		// wait a little bit, touch cancellable
		else if (state == 6) {
			timer += Time.deltaTime;
			if (timer > 3.0f) {
				state = 7;
				timer = 0.0f;
			}
			if (Input.GetMouseButtonDown (0))
				timer += 1.0f;
		}

		// start fadeout
		else if (state == 7) {
			fader.fadeOutTask (this);
			state = 8;
		}

		// wait for fadeout and return to master controller
		else if (state == 8) {
			if (!isWaitingForTaskToComplete) {
				gameController.masterController.startActivity = "ResetGame";
				notifyFinishTask ();
			}
		}

	}

	// network callbacks
	public void finishValoration() {
		
		state = 3;
		timer = 0.0f;
	}
}


