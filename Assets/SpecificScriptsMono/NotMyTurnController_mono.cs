using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotMyTurnController_mono : Task {

	public GameController_mono gameController;

	public GameObject immuneText;
	public GameObject backToBeginningText;


	//public Material playerMaterial;
	public RawImage playerFrameTop;
	public RawImage playerFrameBottom;
	public Texture[] playerFrameTopTexColor;
	public Texture[] playerFrameBottomTexColor;

	public GameObject valorationActivity;
	public GameObject rankingActivity;
	public GameObject dossierActivity;

	public GameObject confirmCanvas;
	public GameObject notMyTurnSchoolCanvas;
	public GameObject notMyTurnGuruCanvas;

	public PlayerActivityController_mono playerController;
	public NotMySchoolController_mono notMyTurnSchoolController;
	public RankingControllerLite_mono rankingController;

	public DossierControllerLite_mono dossierController;

	public RawImage victory;
	public RawImage defeat;
	public RawImage yinYang;
	public RawImage monedo;
	public RawImage maru;
	public RawImage tick;

	public Texture voidTile;
	public Texture[] usedTile;

	public RawImage[] buttonImage;

	public Text waitText;
	public Text notificationsText;
	public UIFaderScript turnConfirmFader;

	public ValorationController_mono valorationController;

	public int volcanoResult = 0;

	bool hasVoted;

	int state = 0;	// 0: idle
					// 1: waiting for valoration to finish
					// 2: waiting for ranking to finish
					// 3: pooling confirm players

	int confirmPlayers = 0;

	const int MAXNOTIFICATIONS = 12;

	/* event callbacks */
	public void clickOnValoration() {


		valorationActivity.SetActive (true);
		valorationController.startValorationTask (this);
		state = 20; // waiting for valoration to finish

	}

	public void clickOnDossier() {
		

		dossierController.startDossierActivity (this);
		state = 40; // wait for dossier to finish
	}

	public void clickOnRanking() {

		//rankingActivity.SetActive (true);
		rankingController.startRankingActivity (this);

		state = 60; // wait for ranking to finish

	}

	public void disableAll() {

		valorationActivity.SetActive (false);


	}

	public void reset() {
		
		waitText.enabled = false;
		valorationActivity.SetActive (false);
		confirmCanvas.SetActive (false);

	}

	public void updateNotifications() {
		notificationsText.text = "";
		int nNotifs = MAXNOTIFICATIONS;
		if (gameController.notificationList.Count < nNotifs)
			nNotifs = gameController.notificationList.Count;
		float opacity = 1.0f;
		const float opacityDelta = 0.15f;
		Color col = notificationsText.color;
		string colorBase = "" + Utils.valueToHexstring (col.r) + Utils.valueToHexstring (col.g) + Utils.valueToHexstring (col.b);
		for (int i = 0; i < nNotifs; ++i) {
			string webCol = colorBase + Utils.valueToHexstring (opacity);
			string singleNotification = "";
			if (i == 0)
				singleNotification = "· ";
			singleNotification += gameController.notificationList [gameController.notificationList.Count - 1 - i];
			notificationsText.text = notificationsText.text + ("<color=#"+ webCol +">" + singleNotification + 
				"</color>\n\n");
			
			opacity -= opacityDelta;
			if (opacity < 0.45f) {
				opacity = 0.35f; 

			}
		}
		//	if(i == 0) notificationsText.text = "· ";
		//	notificationsText.text += gameController.notificationList[gameController.notificationList.Count - 1 - i];
		//	notificationsText.text += "\n\n";

		//}
	}

	public void init() {
		
		waitText.enabled = false;
		valorationActivity.SetActive (false);
		//rankingActivity.SetActive (false);
		updateNotifications();
		hasVoted = false;
		state = 0;
		confirmCanvas.SetActive (false);
		notMyTurnGuruCanvas.SetActive (false);
		victory.enabled = false;
		defeat.enabled = true;
		yinYang.enabled = false;
		monedo.enabled = false;
		maru.enabled = false;
		tick.enabled = false;


	}


	// Use this for initialization
	void Start () {
	
		init ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idling

		}

		//
		else if (state == 1) {

		}

		//
		else if (state == 2) {



		}

		//
		else if (state == 3) {

			turnConfirmFader.setFadeOutValue (1.0f);
			turnConfirmFader.fadeOutTask (this);
			state = 4;

		}

		//
		else if (state == 4) {



		}

		//
		else if (state == 20) { // waiting for valoration to finish
			if (!isWaitingForTaskToComplete) {
				valorationActivity.SetActive (false); // reenable not my turn activity

				state = 0;
			}

		}

		//
		else if (state == 30) { // waiting for school test to finish
			if (!isWaitingForTaskToComplete) {
				notMyTurnSchoolCanvas.SetActive (false); // reenable not my turn activity

				state = 0;
			}

		}

		//
		else if (state == 40) { // waiting for dossier activity to finish
			if (!isWaitingForTaskToComplete) {
				dossierActivity.SetActive (false); // reenable not my turn activity

				state = 0;
			}

		}

		//
		else if (state == 60) { // waiting for dossier activity to finish
			if (!isWaitingForTaskToComplete) {
				rankingActivity.SetActive (false); // reenable not my turn activity

				state = 0;
			}

		}

	}

	public void confirmationSynch() {

		if (state == 2)
			state = 3;

	}


	// turn confirmation OK
	public void voteOK() { 

		if (hasVoted)
			return;
	
		++confirmPlayers;


		waitText.enabled = true;
		turnConfirmFader.setFadeOutValue (0.5f);
		turnConfirmFader.fadeOut ();
		state = 2; // wait for synch

		hasVoted = true;


	}

	// turn confirmation NO
	public void voteNO() {

		if (hasVoted)
			return;


		waitText.enabled = true;
		turnConfirmFader.setFadeOutValue (0.5f);
		turnConfirmFader.fadeOut ();
		state = 2; // wait for synch

		hasVoted = true;

	}

	// this is called from gameController as a response to network command
	//	report:<player>:<life>:<work>... etc...
	public void turnReport(int player, bool life, bool work, bool school, bool gompa, bool guru, bool volcano, bool build,
		bool moned, 
		bool vic, bool yY, bool m, bool t, bool def, bool immune) {
		backToBeginningText.SetActive (false);
		immuneText.SetActive (false);
		if(life)
			buttonImage [0].texture = usedTile [0];
		else buttonImage[0].texture = voidTile;
		if(work)
			buttonImage [1].texture = usedTile [1];
		else buttonImage[1].texture = voidTile;
		if(school) 
			buttonImage [2].texture = usedTile [2];
		else buttonImage[2].texture = voidTile;
		if (gompa) {
			if (immune)
				immuneText.SetActive (true);
			else
				backToBeginningText.SetActive (true);
			buttonImage [3].texture = usedTile [3];
		}
		else buttonImage[3].texture = voidTile;
		if(guru)
			buttonImage [4].texture = usedTile [4];
		else buttonImage[4].texture = voidTile;
		if(volcano)
			buttonImage [5].texture = usedTile [5];
		else buttonImage[5].texture = voidTile;
		if (build)
			buttonImage [6].texture = usedTile [6];
		else
			buttonImage [6].texture = voidTile;

		if (vic)
			victory.enabled = true;
		else
			victory.enabled = false;

		if (moned)
			monedo.enabled = true;
		else
			monedo.enabled = false;

		if (def)
			defeat.enabled = true;
		else
			defeat.enabled = false;

		if (yY)
			yinYang.enabled = true;
		else
		yinYang.enabled = false;

		if (m)
			maru.enabled = true;
		else
		maru.enabled = false;

		if (t)
			tick.enabled = true;
		else
		tick.enabled = false;

		// enable confirm canvas
		confirmCanvas.SetActive(true);


		//playerMaterial.SetColor ("_TintColor", gameController.colorFromPlayerN(player));
		playerFrameTop.texture = playerFrameTopTexColor[player];
		playerFrameBottom.texture = playerFrameBottomTexColor[player];

	}


	// this is called from gameController as a response to network command:
	//   schooltest:<type of wisdom>
	public void schoolTest(int pl, int w) {
		
		notMyTurnSchoolCanvas.SetActive (true);
		notMyTurnSchoolController.startNotMySchoolTask (this, w);
		notMyTurnSchoolController.turnPlayer = pl;
		state = 30; // wait for school activity to finish
	}

	public void guruTest(int type, int q) {
		
		rankingActivity.SetActive (false);
		dossierActivity.SetActive (false);
		valorationActivity.SetActive (false);
		notMyTurnGuruCanvas.SetActive (true);

	}



}
