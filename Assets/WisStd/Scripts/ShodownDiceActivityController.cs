using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShodownDiceActivityController : Task {

	public GameController_multi gameController;

	Animator[] playerDiceAnimator;
	public GameObject[] playerDice;

	Animator[] shadowDiceAnimator;
	public GameObject[] shadowDice;

	public UITextFader defeatFader;

	public GameObject shodownDiceCanvas;

	public CircpleDeploy victoryDeploy;
	public CircpleDeploy defeatDeploy;

	public UIFaderScript fader;

	int []playerRoll;
	int []shadowRoll;

	int playerScore;
	int shadowScore;

	public Text playerScoreText;
	public Text shadowScoreText;

	int state;
	float timer;

	int pNDice;
	int sNDice;

	int ShodownMode;

	public void startDiceShodown(Task w, int playerNDice, int shadowNDice, int mode) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		pNDice = playerNDice;
		sNDice = shadowNDice;

		playerScoreText.text = "";
		shadowScoreText.text = "";

		ShodownMode = mode;
		initialize ();


		fader.fadeIn ();

	}

	// Use this for initialization
	void Start () {




	}

	void initialize() {

		playerDiceAnimator = new Animator[playerDice.Length];
		for (int i = 0; i < playerDice.Length; ++i) {
			playerDiceAnimator[i] = playerDice[i].GetComponent<Animator> ();
			if (i < pNDice)
				playerDice [i].gameObject.SetActive (true);
			else
				playerDice [i].gameObject.SetActive (false);
		}

		shadowDiceAnimator = new Animator[shadowDice.Length];
		for (int i = 0; i < playerDice.Length; ++i) {
			shadowDiceAnimator[i] = shadowDice[i].GetComponent<Animator> ();
			if (i < sNDice)
				shadowDice [i].gameObject.SetActive (true);
			else
				shadowDice [i].gameObject.SetActive (false);
		}

		playerRoll = new int[playerDice.Length];
		shadowRoll = new int[shadowDice.Length];

		timer = 0.0f;

		playerScore = 0;

		for (int i = 0; i < pNDice; ++i) {
			playerRoll[i] = Random.Range (1, 7);
			playerScore += playerRoll [i];
		}

		shadowScore = 0;

		for (int i = 0; i < sNDice; ++i) {
			shadowRoll [i] = Random.Range (1, 7);
			shadowScore += shadowRoll [i];
		}

		defeatDeploy.Start ();
		defeatDeploy.reset ();

		defeatFader.Start ();
		defeatFader.fadeOutImmediately ();

		timer = 0.0f;

		state = 1;

	}


	
	// Update is called once per frame
	void Update () {

		if (state == 0) {
			return;
		}

		if (state == 1) {
			if ((Input.acceleration.magnitude > 3) || Input.GetMouseButtonDown (0)) {
				for (int i = 0; i < pNDice; ++i) {
					playerDiceAnimator [i].SetInteger ("Roll", playerRoll [i]);
				}
				for (int i = 0; i < sNDice; ++i) {
					shadowDiceAnimator[i].SetInteger("Roll", shadowRoll[i]);
				}
				state = 2;
			}
		}
		if (state == 2) {
			timer += Time.deltaTime;
			if(timer > 4) {
				playerScoreText.text = "" + playerScore;
				shadowScoreText.text = "" + shadowScore;
				timer = 0.0f;
				state = 3;
			}
		}
		if(state == 3) {
			timer += Time.deltaTime;
			if(timer > 0.5f) {
				
				state = 4;
			}
			if(Input.GetMouseButtonDown(0)) timer += 2.0f;
		}
		if(state == 4) {
			if(!isWaitingForTaskToComplete) {
				if (playerScore >= shadowScore) {
					waiter.returnBool (true);
					gameController.addNotification (Notification.VENCESOMBRAS, gameController.getPlayerName (gameController.localPlayerN),
						"", "", gameController.getPlayerFemality());
					victoryDeploy.extend ();
					state = 5;
				} else {
					gameController.addNotification (Notification.DOMINADOPORSOMBRAS, gameController.getPlayerName (gameController.localPlayerN),
						"", "", gameController.getPlayerFemality());

					// lose gold
//					gameController.clearGold(gameController.localPlayerN);
//					gameController.networkAgent.broadcast ("cleargold:" + gameController.localPlayerN + ":");
//
//					gameController.addNotification (Notification.PIERDEOROS, gameController.getPlayerName (gameController.localPlayerN),
//						"", "");
					defeatDeploy.extend ();
					if (ShodownMode == LifeTestActivityController_multi.SummonMode) {
						defeatFader.fadeIn ();
					} else {
						int usedOrbs = 0;
						for (int i = 0; i < gameController.playerList [gameController.localPlayerN].orbUsed.Length; ++i) {
							if (gameController.playerList [gameController.localPlayerN].orbUsed [i]) {
								++usedOrbs;
							}
						}
						if (usedOrbs == gameController.playerList [gameController.localPlayerN].orbUsed.Length)
							defeatFader.fadeIn ();
					}
					waiter.returnBool (false);
					timer = 0.0f;
					state = 6;
				}

				timer = 0.0f;

			}
		}
		if (state == 5) {
			timer += Time.deltaTime;
			if (timer > 1.5f) {
				state = 6;
			}
			if (Input.GetMouseButtonDown (0)) {
				timer += 1.5f;
			}
		}
		if (state == 6) {
			timer += Time.deltaTime;
			if (timer > 1.0f) {
				fader.fadeOutTask(this);
				state = 7;
			}

		}
		if (state == 7) {
			if (!isWaitingForTaskToComplete) {
				victoryDeploy.reset ();
				defeatDeploy.reset ();
				notifyFinishTask();
				state = 0;
			}
		}

	}
}
	