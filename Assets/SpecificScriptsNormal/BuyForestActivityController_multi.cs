using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BuyForestActivityController_multi : Task {

	public Texture[] playerBg;
	public RawImage[] forest;
	public GameController_multi gameController;
	public AudioClip buySound_N;
	public UIFaderScript forestFader;
	public UIFaderScript buildingFader;
	/*public Texture gompaHighlight;
	public Texture schoolHighlight;
	public Texture gompaNormal;
	public Texture schoolNormal;*/
	public RawImage gompaImage;
	public RawImage schoolImage;
	public RawImage gompaHLImage;
	public RawImage schoolHLImage;

	RawImage currentBuild;

	public GameObject forestCanvas;
	public GameObject buildingCanvas;

	public UITextAutoFader notEnough_Text;
	public UITextAutoFader noReps_Text;

	public static int ManyForestsRemainCost = 3;
	public static int FewForestsRemainCost = 4;
	public static int BuildGompaCost = 2;
	public static int BuildSchoolCost = 2;

	int numCurrentBuildings = 0;
	int maxBuildings = 3;

	float contBuilds;

	int state = 0; // 0: idling

	public int forestRemain;

	float timer;
	const float delay = 1.5f;

	public void startBuyActivityTask(Task w) {
		gameController.networkAgent.broadcast ("showgallery");
		w.isWaitingForTaskToComplete = true;
		notEnough_Text.Start ();
		notEnough_Text.reset ();
		noReps_Text.Start ();
		noReps_Text.reset ();
		waiter = w;
		init ();
	}

	public void init() {
		bool playerHasForest = false;
		forestRemain = GameController_multi.MAXFORESTS;
		for (int i = 0; i < GameController_multi.MAXFORESTS; ++i) {
			if (gameController.forestOwner [i] == gameController.localPlayerN) {
				playerHasForest = true;
			}
			if (gameController.forestOwner [i] != -1) {
				--forestRemain;
				forest [i].color = new Color (1, 1, 1, 0.25f);
			} else
				forest [i].color = new Color (1, 1, 1, 1);
		}
		timer = 0.0f;
		state = 0;
		numCurrentBuildings = 0;
		contBuilds = float.MaxValue;
		gompaHLImage.color =  new Color (gompaHLImage.color.r, gompaHLImage.color.g, gompaHLImage.color.b, 0);
		gompaHLImage.raycastTarget = false;
		schoolHLImage.color =  new Color (schoolHLImage.color.r, schoolHLImage.color.g, schoolHLImage.color.b, 0);
		schoolHLImage.raycastTarget = false;

		//gompaImage.texture = gompaNormal;
		//schoolImage.texture = schoolNormal;

		if (playerHasForest) {
			forestCanvas.SetActive (false);
			buildingCanvas.SetActive (true);
			buildingFader.fadeIn ();
		} else {
			forestCanvas.SetActive (true);
			buildingCanvas.SetActive (false);
			forestFader.fadeIn ();
		}
	}

	void Start () 
	{
		init ();
	}
	
	void Update () 
	{	
		contBuilds -= Time.deltaTime;

		if (contBuilds <= 0) 
		{
			StartCoroutine (FadeAlphaOut (currentBuild));
			contBuilds = float.MaxValue;
		}

		if (state == 0) 
		{

		}

		if (state == 1) { // small delay (forest)
			timer += Time.deltaTime;
			if (timer > delay) {
				timer = 0.0f;
				forestFader.fadeOutTask (this);
				state = 2;
			}
		}

		if (state == 2) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				notifyFinishTask (); // return to parent task
				state = 0;
			}

		}

		if (state == 3) { // small delay (building)
			timer += Time.deltaTime;
			if (timer > delay) {
				timer = 0.0f;
				buildingFader.fadeOutTask (this);
				state = 2;
			}
		}

		if (state == 60) { // small delay (forest)
			timer += Time.deltaTime;
			if (timer > 2.0f) {
				timer = 0.0f;
				forestFader.fadeOutTask (this);
				state = 2;
			}
		}
	}

	void build(int type) { // type 0 : gompa    1 : school    No, I'm not making an enum or constants for that shit!
		int price = BuildGompaCost;
		if (type == 1)
			price = BuildSchoolCost;
		if (type == 0) { // build a gompa
			if ((gameController.playerList [gameController.localPlayerN].gold >= price) &&
				(gameController.playerList[gameController.localPlayerN].nGompas <= GameController_multi.MAXGOMPAS)) { // check if we can build a new gompa
					//gameController.playerList [gameController.localPlayerN].gold -= price; // send state!
					//gameController.playerList [gameController.localPlayerN].nGompas += 1;
					gameController.build (0, gameController.localPlayerN, price);
					string notif = gameController.getNotificationText (Notification.COMPRAGOMPA);
					string plName = gameController.getPlayerName (gameController.localPlayerN);
					
					gameController.networkAgent.broadcast("build:0:"+gameController.localPlayerN+":"+price+":");
					gameController.addNotification (notif, plName, "", "", gameController.getPlayerFemality());
					// notification:<text>:<param1>:<param2>:<param3>
					notif = notif.Replace(" ", "_");
				gameController.networkAgent.broadcast("notification:" + notif +":" + plName + ":::" +gameController.getPlayerFemality() + ":");
					//gompaImage.texture = gompaHighlight;
					ShowBuildHL (gompaHLImage);
					if (buySound_N != null) {
						gameController.masterController.playSound (buySound_N);
					}
					//state = 3;
			}
		} else { // build a school
			if ((gameController.playerList [gameController.localPlayerN].gold >= price) &&
				(gameController.playerList[gameController.localPlayerN].nSchools<= GameController_multi.MAXSCHOOLS)) { // check if we can build a new gompa
				//gameController.playerList [gameController.localPlayerN].gold -= price; // send state!
				//gameController.playerList [gameController.localPlayerN].nSchools += 1;
				gameController.build (1, gameController.localPlayerN, price);
				gameController.networkAgent.broadcast("build:1:"+gameController.localPlayerN+":"+price+":");

				string notif = gameController.getNotificationText (Notification.COMPRAESCUELA);
				string plName = gameController.getPlayerName (gameController.localPlayerN);

				gameController.addNotification (notif, "" + plName, "", "", gameController.getPlayerFemality());
				// notification:<text>:<param1>:<param2>:<param3>
				notif = notif.Replace(" ", "_");
				gameController.networkAgent.broadcast("notification:" + notif + ":" + plName + ":::" +gameController.getPlayerFemality() + ":");

				//schoolImage.texture = schoolHighlight;
				ShowBuildHL (schoolHLImage);
				if (buySound_N != null) {
					gameController.masterController.playSound (buySound_N);
				}
				//state = 3;
			}
		}
		numCurrentBuildings++;

		if (numCurrentBuildings >= maxBuildings) 
		{
			cancelThat ();
		}
	}

	IEnumerator FadeAlphaOut(RawImage _build)
	{
		float _alpha = 1;
		do {
			_alpha -= 0.1f;
			_build.color = new Color (_build.color.r, _build.color.g, _build.color.b, _alpha);
			yield return new WaitForSeconds(0.1f);

		} while (_build.color.a > 0);
		_build.raycastTarget = false;
	}

	void ShowBuildHL(RawImage _build)
	{
		_build.raycastTarget = true;
		_build.color = new Color (_build.color.r, _build.color.g, _build.color.b, 1);
		contBuilds = 0.5f;
		currentBuild = _build;
	}

	void buyForest(int frst) {

//		int price = ManyForestsRemainCost;
//		if ((gameController.nPlayers >= 4) && (forestRemain <= 2))
//			price = FewForestsRemainCost;
		int price = 0;


		if (gameController.playerList [gameController.localPlayerN].numberOfWisdoms () == 0) {

			noReps_Text.fadein ();
			state = 60;

		} else {
			if (gameController.playerList [gameController.localPlayerN].gold >= price) { // can buy
				gameController.buyForest (frst, gameController.localPlayerN, price);
				gameController.networkAgent.broadcast ("buyforest:" + frst + ":" + gameController.localPlayerN + ":" + price + ":");
				//gameController.playerList [gameController.localPlayerN].gold -= price;
				forest [frst].color = new Color (1, 1, 1, 0.25f);
				if (buySound_N != null) {
					gameController.masterController.playSound (buySound_N);
				}
				state = 1;

				forestFader.GetComponent<RawImage> ().enabled = true;

				string notif = gameController.getNotificationText (Notification.COMPRABOSQUE);
				string plName = gameController.getPlayerName (gameController.localPlayerN);
				string forestName = gameController.getForestName (frst);
				gameController.addNotification (notif, plName, forestName, "", gameController.getPlayerFemality ());
				// notification:<text>:<param1>:<param2>:<param3>
				notif = notif.Replace (" ", "_");
				gameController.networkAgent.broadcast ("notification:" + notif + ":" + plName + ":" + forestName + "::" + gameController.getPlayerFemality () + ":");

				if ((forestRemain == 2) && (gameController.nPlayers == GameController_multi.MaxPlayers)) { // have to buy both forest, one for me, one for the player with no forests
					int lastForest = 0;
					int lastPlayer = 0;
					for (int i = 0; i < GameController_multi.MAXFORESTS; ++i) {
						if (gameController.forestOwner [i] == -1)
							lastForest = i;
					}
					for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
						bool hasForest = false;
						for (int j = 0; j < GameController_multi.MAXFORESTS; ++j) {
							if (gameController.forestOwner [j] == i)
								hasForest = true;
						}
						if (hasForest == false)
							lastPlayer = i;
					}
					gameController.buyForest (lastForest, lastPlayer, 0);
					forest [lastForest].color = new Color (1, 1, 1, 0.25f);
					gameController.networkAgent.broadcast ("buyforest:" + lastForest + ":" + lastPlayer + ":" + 0 + ":");
					notif = gameController.getNotificationText (Notification.COMPRABOSQUE);
					plName = gameController.getPlayerName (lastPlayer);
					forestName = gameController.getForestName (lastForest);
					gameController.addNotification (notif, plName, forestName, "", gameController.getPlayerFemality (lastPlayer));
					// notification:<text>:<param1>:<param2>:<param3>
					notif = notif.Replace (" ", "_");
					gameController.networkAgent.broadcast ("notification:" + notif + ":" + plName + ":" + forestName + "::" + gameController.getPlayerFemality (lastPlayer) + ":");
				}

			} else {
				//forestFader.fadeOutTask (this);
				notEnough_Text.fadein ();
				state = 60;
			}
		}

	}

	/* event callbacks */
	public void clickOnForest1() {
		if (forest [GameController_multi.ORANGE].enabled == true) {
			buyForest (GameController_multi.ORANGE);
		}
	}

	public void clickOnForest2() {
		if (forest [GameController_multi.TEAL].enabled == true) {
			buyForest (GameController_multi.TEAL);
		}
	}

	public void clickOnForest3() {
		if (forest [GameController_multi.PURPLE].enabled == true) {
			buyForest (GameController_multi.PURPLE);
		}
	}

	public void clickOnForest4() {
		if (forest [GameController_multi.DARKBLUE].enabled == true) {
			buyForest (GameController_multi.DARKBLUE);
		}
	}

	public void clickOnGompa() {
		
		build (0);
	}

	public void clickOnSchool() {

		build (1);

	}

	public void cancelThat() {

		if (state != 0)
			return;

		if (forestCanvas.activeInHierarchy == true) {
			//forestFader.fadeOutTask (this);
			state = 60;
		} else
			//buildingFader.fadeOutTask (this);
			state = 3;
		//state = 3;
	}

}
	