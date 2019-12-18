using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 
 * We'll use the slot:state:substate paradigm for sequencing
 * 
 * 
 * 
 */

public class TitlesController_monoNormal : Task {

	/* references galore */
	public GameObject buyButton;
	public Text creditsHUD;



	public UIFaderScript fader;

	public AudioClip buttonSound;

	public UITextAutoFader versionLabelFader;

	public UITextFadeDelay copyrightNoticeFader;

	public GameObject instructionsButton;
	public UIScaleFader instructionsPanel;
	public string PDFLink;
	public string VideoLink;

	public RawImage logoImage;
//	public RawImage logoYinYangQueGira;
	public UIFaderScript LITEFader;
//	public RawImage titleImage;
	public GameObject titleObject;
	public RawImage cloudsImage;
	public RawImage goBackButton;
	public GameObject sessionMenu;
	public GameObject whiteBack;
	public UIFaderScript whiteCover;

	public InputField playcodeInput;


	public GameController_mono gameController;

	public Text loggedInUserText;
	public Text loggedInUserTextbis;
	public Text loggedInUserTexttris;
	public Text loggedInUserTextquis;

	public MasterController_mono masterController;

	public GameObject bambooFrame;

	public GameObject taccanvas;

	public int accountCredits;

	WWW www;

	/* slot 0 */
	public int state0;
	public int substate0;
	float timer0;


	const float LogoDelay = 3.0f;

	const float InterFadeDelay = 0.5f;

	const float TitleDelay = 1.0f;

	const float ServerTimeout = 10.0f;

	bool continueGame;


	void Start () {
		
		bambooFrame.SetActive (false);
		state0 = 0;
		substate0 = 0;
		timer0 = 0.0f;
		logoImage.enabled = true;
		titleObject.SetActive (false);

		copyrightNoticeFader.gameObject.SetActive (false);
		versionLabelFader.gameObject.SetActive (false);

		cloudsImage.enabled = false;

		sessionMenu.SetActive (false);

		whiteBack.SetActive (false);


		goBackButton.enabled = false;

		instructionsButton.SetActive (false);
		instructionsPanel.gameObject.SetActive (false);
		creditsHUD.text = "Créditos: ";
		buyButton.GetComponent<Button> ().interactable = false;
	}

	const int playcodeYear = 2018;
	const int playcodeMonth = 5;
	const int playcodeDay = 20;

	bool freePlay = true;

	public void titlesGoTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();
		state0 = 1;
		gameController.gameState = 0; // titles

		continueGame = gameController.checkQuickSaveInfo ();

		System.DateTime dt = System.DateTime.Now;
		System.DateTime pcdt = new System.DateTime (playcodeYear, playcodeMonth, playcodeDay, 0, 0, 0);
		//if (dt.CompareTo (pcdt) <= 0) {
			playcodeInput.text = CorrectPlayCode;
			playcodeInput.gameObject.SetActive (false);
			freePlay = true;
		//} else {
		//	playcodeInput.text = gameController.quickSaveInfo.playcode;
		//	playcodeInput.gameObject.SetActive (true);
		//	freePlay = false;
		//}

		gameController.loadTipSaveData ();
	}

	public void updateCreditsHUD() {

		if (accountCredits >= 0) {
			creditsHUD.text = "Créditos: " + accountCredits;
			buyButton.GetComponent<Button> ().interactable = true;
		} else if (accountCredits == -1) {
			creditsHUD.text = "Créditos: ∞";
			buyButton.GetComponent<Button> ().interactable = false;
		} else if (accountCredits == -2) {
			creditsHUD.text = "Créditos: ?";
			buyButton.GetComponent<Button> ().interactable = false;
		}
		else  {
			creditsHUD.text = "Créditos: ";
			buyButton.GetComponent<Button> ().interactable = false;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (state0 == 0) { // idling

			return;

		}
	
		else if (state0 == 1) { // State 0: FlyGames logo

			if (substate0 == 0) { // showing the logo

				timer0 += Time.deltaTime;
				if ((timer0 > LogoDelay) || (Input.GetMouseButtonDown (0))) {
					fader.fadeOutTask (this);
					substate0 = 1;
				}

			} else if (substate0 == 1) { // fading out
				if (isWaitingForTaskToComplete)
					return;
				substate0 = 2;
				timer0 = 0.0f;
			} 

			else if (substate0 == 2) {
				timer0 += Time.deltaTime;
				if (timer0 > InterFadeDelay) {
					timer0 = 0.0f;
					logoImage.enabled = false;
					bambooFrame.SetActive (true);
					titleObject.SetActive (true);
					copyrightNoticeFader.reset ();
					copyrightNoticeFader.gameObject.SetActive (true);
					versionLabelFader.gameObject.SetActive (true);
					cloudsImage.enabled = true;

					fader.fadeIn ();
					whiteCover.enabled = true;
					whiteBack.SetActive(true);
					whiteCover.setFadeValue (1.0f);
					sessionMenu.SetActive (true);
					instructionsButton.SetActive (true);

					state0 = 2;
					substate0 = 0;

					gameController.loadData ();

//					if (!gameController.localUserEMail.Equals ("")) {
//						// send data to server
//						WWWForm wwwForm = new WWWForm();
//						wwwForm.AddField("email", gameController.localUserEMail);
//						wwwForm.AddField("passwd", gameController.getUserPass());
//						wwwForm.AddField("app", "Wis");
//
//						www = new WWW (gameController.networkAgent.bootstrapData.loginServer + ":" + gameController.networkAgent.bootstrapData.loginServerPort + Utils.CheckUserScript, wwwForm);
//
//						substate0 = 300;
//					}



				}
			}

		} 

		else if (state0 == 2) { // State 1: showing Title

			if (substate0 == 0) {
				timer0 += Time.deltaTime;
				if (timer0 > TitleDelay) {
					substate0 = 1;
					whiteCover.fadeIn ();
					versionLabelFader.fadein ();
					LITEFader.fadeOut ();
					//copyrightNoticeFader.reset ();
					copyrightNoticeFader.go ();
				}
			} 

			else if (substate0 == 1) { // waiting for user input via callbacks


			} 

			else if (substate0 == 2) { // register menu

				if (isWaitingForTaskToComplete)
					return;
				// disable root menu and enable registry menu

				instructionsButton.SetActive (false);

				goBackButton.enabled = true;
				whiteCover.fadeIn ();
				substate0 = 3;

			} 

			else if (substate0 == 4) { // sign in menu

				if (isWaitingForTaskToComplete)
					return;
				// disable root menu and enable registry menu


				instructionsButton.SetActive (false);

				goBackButton.enabled = true;
				whiteCover.fadeIn ();
				substate0 = 5;
			} 

			else if (substate0 == 100) { // waiting for www to return data from new user submittal
				if (www.isDone) {
					whiteCover.fadeOutTask (this);
					timer0 = 0.0f;
					substate0 = 0;
					substate0 = 101;
				}
					
			} 

			else if (substate0 == 101) { // www has returned data, wait for fadeout
				if (isWaitingForTaskToComplete)
					return;
				

				goBackButton.enabled = false;
				whiteCover.fadeIn ();
				substate0 = 102;
			} 

			else if (substate0 == 102) {
				timer0 += Time.deltaTime;
				if ((timer0 > TitleDelay * 4) || Input.GetMouseButtonDown (0)) {
					whiteCover.fadeOutTask (this);
					substate0 = 103;
				}
			} 

			else if (substate0 == 103) { // wait for fadeout

				if (isWaitingForTaskToComplete)
					return;
				timer0 = 0.0f; // go back to root menu
				logoImage.enabled = false;
				titleObject.SetActive (true);
				copyrightNoticeFader.reset ();
				copyrightNoticeFader.gameObject.SetActive (true);
				versionLabelFader.gameObject.SetActive (true);
				cloudsImage.enabled = true;

				fader.fadeIn ();
				whiteCover.enabled = true;
				whiteBack.SetActive (true);
				whiteCover.setFadeValue (1.0f);


				instructionsButton.SetActive (true);
				substate0 = 0;
			} 


			else if (substate0 == 200) { // manual input of credentials
				if (www.isDone) {
					
					int userUUID;

					if (!www.text.Equals ("")) {

						string[] fields = www.text.Split (':');

						if (fields.Length == 2) {

							int.TryParse (fields [1], out accountCredits);
							updateCreditsHUD ();
							if (accountCredits == -2) { // special meaning
								

								//noMagicMenu.SetActive (true);


								loggedInUserTextquis.text = gameController.localUserEMail;
								gameController.saveData ();

								state0 = 0;
								return;
							}
						
							int.TryParse (fields[0], out userUUID);

							if (userUUID < 0) {
								accountCredits = -100;
								updateCreditsHUD ();
							}

							if (userUUID != -1) {


								whiteCover.fadeOutTask (this);
								timer0 = 0.0f;
								substate0 = 0;
								substate0 = 201;
								gameController.setUserLogin ("" + userUUID);

								gameController.saveData ();
							} else {
								

								substate0 = 1; // return to user input polling
							}
							

						} else {
							substate0 = 1;
						}

					} else {
						
						substate0 = 1; // return to user input polling
					}
				}

			} 

			else if (substate0 == 201) { // waiting for fadeout from substate0 = 200

				if (isWaitingForTaskToComplete)
					return;
				goBackButton.enabled = false;
				sessionMenu.SetActive (true);
				loggedInUserText.text = gameController.localUserEMail;
				loggedInUserTextbis.text = gameController.localUserEMail;
				loggedInUserTexttris.text = gameController.localUserEMail;
				loggedInUserTextquis.text = gameController.localUserEMail;

				instructionsButton.SetActive (true);
				whiteCover.fadeIn ();
				versionLabelFader.fadein ();
				//copyrightNoticeFader.reset ();
				copyrightNoticeFader.go ();
				substate0 = 1;

			}

			else if (substate0 == 300) { // waiting for www to return user id confirm data (stored data)
				if (www.isDone) {

					int userUUID;

					if (!www.text.Equals ("")) {

						string[] fields = www.text.Split (':');

						if (fields.Length == 2) {

							int.TryParse (fields [1], out accountCredits);
							updateCreditsHUD ();
							if (accountCredits == -2) { // special meaning
								


								gameController.saveData ();

								loggedInUserTextquis.text = gameController.localUserEMail;


								whiteCover.fadeIn ();
								state0 = 0;
								return;
							}

							int.TryParse (fields [1], out accountCredits);

							int.TryParse (fields [0], out userUUID);

							if (userUUID < 0) {
								accountCredits = -100;
								updateCreditsHUD ();
							}

							if (userUUID != -1) {
								
								loggedInUserText.enabled = true;
								loggedInUserText.text = gameController.localUserEMail;
								loggedInUserTextquis.text = gameController.localUserEMail;

								instructionsButton.SetActive (false);
								timer0 = 0.0f;
								substate0 = 0;
								substate0 = 201;

							} else {

								whiteCover.fadeIn ();
								substate0 = 1; // return to user input polling
							}

						} else {
							whiteCover.fadeIn ();
							substate0 = 1; // return to user input polling
						}

					} else {
						whiteCover.fadeIn ();
						substate0 = 1; // return to user input polling
					}
				}

			} 

			else if (substate0 == 500) { // waiting for fadeout in order to go back to substate 0

				if (isWaitingForTaskToComplete)
					return;
				
				timer0 = TitleDelay;
		



				logoImage.enabled = false;
				titleObject.SetActive (true);

				copyrightNoticeFader.reset ();
				copyrightNoticeFader.gameObject.SetActive (true);
				versionLabelFader.gameObject.SetActive (true);
				copyrightNoticeFader.go ();

				cloudsImage.enabled = true;

				goBackButton.enabled = false;
				fader.fadeIn ();
				whiteCover.enabled = true;
				whiteBack.SetActive(true);
				whiteCover.setFadeValue (1.0f);

				sessionMenu.SetActive (false);

				instructionsButton.SetActive (true);
				substate0 = 0;

			}

			else if (substate0 == 600) { // waiting for fade to black in order to start a new game

				if (isWaitingForTaskToComplete)
					return;
				masterController.startActivity = "MainGame";
				state0 = 0;
				substate0 = 0;
				notifyFinishTask ();
				buyButton.GetComponent<Button> ().interactable = false;
			}

			else if (substate0 == 700) { // waiting for fade to black in order to start a new game

				if (isWaitingForTaskToComplete)
					return;
				masterController.startActivity = "JoinNewGame";
				state0 = 0;
				substate0 = 0;
				notifyFinishTask ();
				buyButton.GetComponent<Button> ().interactable = false;
			}

			else if (substate0 == 4000) { // waiting for fade to black in order to continue a new game

				if (isWaitingForTaskToComplete)
					return;
				masterController.startActivity = "ContinueGame";
				state0 = 0;
				substate0 = 0;
				notifyFinishTask ();
				buyButton.GetComponent<Button> ().interactable = false;
			}

		}

	}

	const string CorrectPlayCode = "darwinazul78";

	public void playButton() {

		if (!freePlay) {
			
				if (playcodeInput.text != CorrectPlayCode) {
					return;
				}


			gameController.quickSaveInfo.playcode = playcodeInput.text;
			gameController.saveQuickSaveInfo ();
		}

		substate0 = 600;
		fader.fadeOutTask (this);
	}


	/*
	 * 
	 * OnClick event handlers
	 * 
	 */

	public void ClickOnPDF()
	{
		Application.OpenURL (PDFLink);
	}

	public void ClickOnVideo()
	{
		if(VideoLink != "")
		Application.OpenURL (VideoLink);
	}

	public void clickOnInfo()
	{
		instructionsPanel.gameObject.SetActive (true);
		instructionsPanel.Start ();
		instructionsPanel.scaleIn ();
	}

	public void clickOnCloseInfo()
	{
		instructionsPanel.scaleOut ();
	}




	public void tacOKButton() {
		if (taccanvas.activeSelf == true)
			taccanvas.SetActive (false);
		else
			taccanvas.SetActive (true);
	}


}
