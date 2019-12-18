using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class WorkActivityController_multi : Task {

	int state = 0;
	int goldAmount;
	public UIFaderScript fader;
	float timer;
	public Text text;
	const float Delay = 0.75f;
	public GameController_multi gameController;
	public AudioClip cashSound;


	Animator dice1Animator;
	Animator dice2Animator;
	public GameObject dice1;
	public GameObject dice2;

	int roll1, roll2;


	public void initialize() {

		fader.fadeIn ();
		roll1 = Random.Range (1, 7);
		roll2 = Random.Range (1, 7);
		goldAmount = roll1 + roll2;
		text.text = "";
		timer = 0;
		state = 1;

		dice1Animator = dice1.GetComponent<Animator> ();
		dice2Animator = dice2.GetComponent<Animator> ();

	}

	public void startWorkActivityTask(Task w) {
		gameController.networkAgent.broadcast ("showgallery");
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		initialize ();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idling

		} else if (state == 1) { // initial delay

			timer += Time.deltaTime;

			if (timer > Delay || (Input.acceleration.magnitude > 3) || Input.GetMouseButtonDown(0))
			{
				timer = 0.0f;
				state = 2;
				// animate dice
				dice1Animator.SetInteger ("Roll", roll1); 
				dice2Animator.SetInteger ("Roll", roll2);

			}

		} 

		else if (state == 2) { // wait a bit before showing the written results
			timer += Time.deltaTime;
			if (timer > 2.5f) {
				state = 3;
				timer = 0.0f;
			}
		}

		else if (state == 3) {

			state = 4;
			string notif = "";
			string plName = "";
			if (goldAmount > 1) {
				text.text = "¡Has obtenido " + goldAmount + " piedras!";
				notif = gameController.getNotificationText (Notification.GANAOROS);
			} else {
				text.text = "¡Has obtenido 1 piedra!";
				notif = gameController.getNotificationText (Notification.GANAUNORO);
			}


			 plName = gameController.getPlayerName (gameController.localPlayerN);

			gameController.addNotification (notif, plName, "" + goldAmount, "", gameController.getPlayerFemality());
			// notification:<text>:<param1>:<param2>:<param3>
			notif = notif.Replace(" ", "_");
			gameController.networkAgent.broadcast("notification:"+notif+":" + plName + ":" + goldAmount+ "::" +gameController.getPlayerFemality() + ":");

			gameController.masterController.playSound (cashSound);
			// add gold for local player
			gameController.addGold (gameController.localPlayerN, goldAmount);
			// make others know about the earnings
			gameController.networkAgent.broadcast ("addgold:" + gameController.localPlayerN + ":");


		} 

		else if (state == 4) { // waiting for delay/screentouch

			timer += Time.deltaTime;
			if ((timer > (Delay * 2)) || Input.GetMouseButtonDown (0)) {
				fader.fadeOutTask (this);
				state = 5;
			}

		} 

		else if (state == 5) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				notifyFinishTask (); // return to PlayerActivity
				state = 0; // idle
			}

		}

	}
}

