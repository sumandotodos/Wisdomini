using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ShodownController : Task {

	public RawImage[] shadows;
	public RawImage[] heroes;

	public Texture[] heroTex;

	public CircpleDeploy victory;
	public CircpleDeploy defeat;

	public Text nShadowsText;
	public Text nHeroesText;

	public LifeTestActivityController_multi parentController;

	public GameObject shodownDiceActivity;

	public GameController_multi gameController;

	public ShodownDiceActivityController shodownDiceController;

	public UIFaderScript fader;

	int state = 0;
	float timer;
	const float shortDelay = 0.25f;
	const float longDelay = 0.75f;

	//[HideInInspector]
	public int wisdom;

	int currentShadow;
	int current;

	int nShadows;
	int nHeroes;

	int ShodownMode;

	public void startShodownTask(Task w, int mode) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;

		ShodownMode = mode;

		// get number of individuals in wisdom categody 'wisdom'
		int nIndiv = //gameController.individualsInWisdom(wisdom);
			gameController.playerList[gameController.localPlayerN].heroAmount[wisdom];
	
		for (int i = 0; i < heroes.Length; ++i) {
			heroes [i].texture = heroTex [wisdom];
			heroes [i].GetComponent<UIAutoFader> ().reset ();
		}

		for (int i = 0; i < shadows.Length; ++i) {
			shadows [i].GetComponent<UIAutoFader> ().reset ();
		}

		// get number of shadows
		//nShadows = Random.Range(1, gameController.individualsInWisdom(wisdom)+1);

		nHeroes = nIndiv;

		switch (nHeroes) {
		case 1:
			nShadows = Random.Range (1, 3); 
			break;
		case 2:
			nShadows = Random.Range (1, 6); 
			break;
		case 3:
			nShadows = Random.Range (2, 7); 
			break;
		case 4:
			nShadows = Random.Range (3, 7); 
			break;
		case 5:
			nShadows = Random.Range (4, 8); 
			break;
		case 6:
			nShadows = Random.Range (4, 8); 
			break;
		case 7:
			nShadows = Random.Range (5, 8); 
			break;
		}

		nShadowsText.text = "";
		nHeroesText.text = "";

		fader.fadeIn ();

		current = 0;

		state = 10;

	}

	// Use this for initialization
	/*void Start () {
	
		wisdom = 1; // masters, for example
		startShodownTask (this);

	}*/
	
	// Update is called once per frame
	void Update () {

		if (state == 0) { // idling

		}

		if (state == 10) {
			timer += Time.deltaTime;
			if (timer > 0.25f) {
				timer = 0.0f;
				state = 1;
			}
		}

		if (state == 1) { // making shadows appear
			timer += Time.deltaTime;
			if (timer > shortDelay) {
				shadows [current].GetComponent<UIAutoFader> ().fadein ();
				timer = 0.0f;
				++current;
				if (current == nShadows) {
					current = 0;
					state = 2;
				}
			}
		}

		if (state == 2) { // making heroes appear
			timer += Time.deltaTime;
			if (timer > shortDelay) {
				heroes [current].GetComponent<UIAutoFader> ().fadein ();
				timer = 0.0f;
				++current;
				if (current == nHeroes) {
					current = 0;
					state = 3;
					nShadowsText.text = "" + nShadows;
					nShadowsText.GetComponent<UITextAutoFader> ().fadein ();
					nShadowsText.GetComponent<UIAutoScaler> ().go ();
				}
			}
		}

		if (state == 3) { // additional short delay
			timer += Time.deltaTime;
			if (timer > shortDelay/2.0f) {
				timer = 0.0f;
				nHeroesText.text = "" + nHeroes;
				nHeroesText.GetComponent<UITextAutoFader> ().fadein ();
				nHeroesText.GetComponent<UIAutoScaler> ().go ();

				// notification
				gameController.addNotification (Notification.SEENFRENTA, gameController.getPlayerName (gameController.localPlayerN),
					"" + nShadows, "" + nHeroes, gameController.getPlayerFemality());

				state = 4;
			}

		}

		if (state == 4) { // additional short delay
			timer += Time.deltaTime;
			if (timer > shortDelay) {
				timer = 0.0f;
				state = 5;
			}

		}

		if (state == 5) { // extend result buttons
			//victory.extend (); // no more user input, start new activity
			//defeat.extend ();
			timer += Time.deltaTime;
			if (timer > 3.0f) {
				shodownDiceActivity.SetActive (true);
				shodownDiceController.startDiceShodown (this, nHeroes, nShadows, ShodownMode);
				fader.fadeOut ();
				state = 6;
			}
			if (Input.GetMouseButtonDown (0))
				timer += 3.0f;
		}

		if (state == 6) { // wait for user input...
			if (!isWaitingForTaskToComplete) {
				if (bReturnValue == true)
					parentController.activityResult = 1;
				else
					parentController.activityResult = -1;
				shodownDiceActivity.SetActive (false);
				state = 7;
			}
		}

		if (state == 7) { // wait for fadeout to finish
			//if (!isWaitingForTaskToComplete) {
				notifyFinishTask ();
				state = 0; // idle
			//}
		}
	
	}




	// THESE TWO HAVE BECOME DEPRECATED!!!
	/* event callbacks */
	public void clickOnVictory() {
		state = 7;
		parentController.activityResult = 1;
		fader.fadeOutTask (this);

		gameController.addNotification (Notification.VENCESOMBRAS, gameController.getPlayerName (gameController.localPlayerN),
			"", "", gameController.getPlayerFemality());
	}

	public void clickOnDefeat() {
		
		state = 7;

		parentController.activityResult = 0;
		fader.fadeOutTask (this);

		gameController.addNotification (Notification.DOMINADOPORSOMBRAS, gameController.getPlayerName (gameController.localPlayerN),
			"", "", gameController.getPlayerFemality());

		// lose gold
		gameController.clearGold(gameController.localPlayerN);
		gameController.networkAgent.broadcast ("cleargold:" + gameController.localPlayerN + ":");

		gameController.addNotification (Notification.PIERDEOROS, gameController.getPlayerName (gameController.localPlayerN),
			"", "", gameController.getPlayerFemality());
		/*string notif = gameController.getNotificationText (Notification.PIERDEOROS);
		string plName = gameController.getPlayerName (gameController.localPlayerN);
		gameController.addNotification (notif, "" + plName, "", "");
		// notification:<text>:<param1>:<param2>:<param3>
		notif = notif.Replace(" ", "_");
		gameController.networkAgent.broadcast("notification:" + notif + ":" + plName + ":::");
		*/
	}
}
	