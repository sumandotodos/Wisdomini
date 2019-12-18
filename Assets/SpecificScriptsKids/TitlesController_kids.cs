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

public class TitlesController_kids : Task {

	/* references galore */
	public GameObject buyButton;
	public Text creditsHUD;

	public GameObject IAPCanvas;
	public InputField newMagicInput;

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
	public GameObject fondoKids;
	public UITextFader kidsFader;
	public UIFaderScript LITEFader;
//	public RawImage titleImage;
	public GameObject titleObject;
	public RawImage cloudsImage;
	public RawImage cloudsImage2;
	public RawImage goBackButton;
	public GameObject sessionMenu;
	public GameObject rootMenu;
	public GameObject registroMenu;
	public GameObject whiteBack;
	public UIFaderScript whiteCover;
	public GameObject SessionMenu2;
	public GameObject SessionMenu2Cont;
	public GameObject justLogoutMenu;
	public UIScaleFader continueGamePanel;
	bool continueGamePanelStartNewGame;

	public InputField newUser;
	public InputField passwd1;
	public InputField passwd2;
	public InputField magicInput;

	public InputField playcodeInput;

	public InputField loginUser;
	public InputField loginPasswd;

	public UIScaleFader noMoarCreditsPanel;
	public UIScaleFader noMoarMagicPanel;

	public Text passwordDoNotMatch;

	public GameObject checkMailNotice;

	public GameObject signInMenu;

	public GameObject loginIncorrectText;
	public GameObject serverUnreachableText;

	public GameController_multi gameController;

	public Text loggedInUserText;
	public Text loggedInUserTextbis;
	public Text loggedInUserTexttris;
	public Text loggedInUserTextquis;

	public MasterController_kids masterController;

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
		fondoKids.SetActive (false);
		IAPCanvas.SetActive (false);
		bambooFrame.SetActive (false);
		state0 = 0;
		substate0 = 0;
		timer0 = 0.0f;
		logoImage.enabled = true;
		titleObject.SetActive (false);

		copyrightNoticeFader.gameObject.SetActive (false);
		versionLabelFader.gameObject.SetActive (false);

		cloudsImage.enabled = false;
		cloudsImage2.enabled = false;
		passwordDoNotMatch.enabled = false;
		sessionMenu.SetActive (false);
		rootMenu.SetActive (false);
		registroMenu.SetActive (false);
		whiteBack.SetActive (false);
		checkMailNotice.SetActive (false);
		signInMenu.SetActive (false);
		goBackButton.enabled = false;
		SessionMenu2.SetActive (false);
		justLogoutMenu.SetActive (false);
		SessionMenu2Cont.SetActive (false);
		instructionsButton.SetActive (false);
		instructionsPanel.gameObject.SetActive (false);
		creditsHUD.text = "Créditos: ";
		buyButton.GetComponent<Button> ().interactable = false;
	}

	const int playcodeYear = 2018;
	const int playcodeMonth = 5;
	const int playcodeDay = 20;


	bool freePlay = true;

	const string CorrectPlayCode = "darwinazul78";

	public void titlesGoTask(Task w) {
	
		kidsFader.Start ();
		kidsFader.fadeOutImmediately ();
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();
		state0 = 1;
		gameController.gameState = 0; // titles
		continueGamePanel.Start ();
		continueGamePanel.scaleOutImmediately ();
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
				fondoKids.SetActive (true);
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
					cloudsImage2.enabled = true;
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
					kidsFader.fadeIn ();
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
				rootMenu.SetActive (false);
				instructionsButton.SetActive (false);
				registroMenu.SetActive (true);
				goBackButton.enabled = true;
				whiteCover.fadeIn ();
				substate0 = 3;

			} 

			else if (substate0 == 4) { // sign in menu

				if (isWaitingForTaskToComplete)
					return;
				// disable root menu and enable registry menu
				rootMenu.SetActive (false);
				loginIncorrectText.SetActive (false);
				serverUnreachableText.SetActive (false);
				instructionsButton.SetActive (false);
				signInMenu.SetActive (true);
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
				registroMenu.SetActive (false);
				signInMenu.SetActive (false);
				checkMailNotice.SetActive (true);
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
				cloudsImage2.enabled = true;
				fader.fadeIn ();
				whiteCover.enabled = true;
				whiteBack.SetActive (true);
				whiteCover.setFadeValue (1.0f);
				checkMailNotice.SetActive (false);
				rootMenu.SetActive (true);
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
								newMagicInput.text = "";
								signInMenu.SetActive (false);
								//noMagicMenu.SetActive (true);

								noMoarMagicPanel.Start ();
								noMoarMagicPanel.scaleIn ();
								gameController.localUserEMail = loginUser.text;
								gameController.localUserPass = loginPasswd.text;
								loggedInUserTextquis.text = gameController.localUserEMail;
								gameController.saveData ();
								justLogoutMenu.SetActive (true);
								state0 = 0;
								return;
							}
						
							int.TryParse (fields[0], out userUUID);

							if (userUUID < 0) {
								accountCredits = -100;
								updateCreditsHUD ();
							}

							if (userUUID != -1) {

								loginIncorrectText.SetActive (false);
								serverUnreachableText.SetActive (false);
								whiteCover.fadeOutTask (this);
								timer0 = 0.0f;
								substate0 = 0;
								substate0 = 201;
								gameController.setUserLogin ("" + userUUID);
								gameController.localUserEMail = loginUser.text;
								gameController.setUserPass (loginPasswd.text);
								gameController.saveData ();
							} else {
								
								loginIncorrectText.SetActive (true);
								substate0 = 1; // return to user input polling
							}
							

						} else {
							substate0 = 1;
						}

					} else {
						serverUnreachableText.SetActive (true);
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
				signInMenu.SetActive (false);
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
								newMagicInput.text = "";
								signInMenu.SetActive (false);

								gameController.saveData ();
								noMoarMagicPanel.Start ();
								noMoarMagicPanel.scaleIn ();
								loggedInUserTextquis.text = gameController.localUserEMail;
								justLogoutMenu.SetActive (true);
								rootMenu.SetActive (false);
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
								loginIncorrectText.SetActive (false);
								serverUnreachableText.SetActive (false);
								loggedInUserText.enabled = true;
								loggedInUserText.text = gameController.localUserEMail;
								loggedInUserTextquis.text = gameController.localUserEMail;
								rootMenu.SetActive (false);
								instructionsButton.SetActive (false);
								timer0 = 0.0f;
								substate0 = 0;
								substate0 = 201;

							} else {
								loginIncorrectText.SetActive (true);
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
				loginPasswd.text = "";
				timer0 = TitleDelay;
		
				justLogoutMenu.SetActive (false);
				loginIncorrectText.SetActive (false);
				serverUnreachableText.SetActive (false);
				signInMenu.SetActive (false);

				registroMenu.SetActive (false);
				logoImage.enabled = false;
				titleObject.SetActive (true);

				copyrightNoticeFader.reset ();
				copyrightNoticeFader.gameObject.SetActive (true);
				versionLabelFader.gameObject.SetActive (true);
				copyrightNoticeFader.go ();

				cloudsImage.enabled = true;
				cloudsImage2.enabled = true;
				goBackButton.enabled = false;
				fader.fadeIn ();
				whiteCover.enabled = true;
				whiteBack.SetActive(true);
				whiteCover.setFadeValue (1.0f);
				signInMenu.SetActive (false);
				sessionMenu.SetActive (false);
				rootMenu.SetActive (true);
				instructionsButton.SetActive (true);
				substate0 = 0;

			}

			else if (substate0 == 600) { // waiting for fade to black in order to start a new game

				if (isWaitingForTaskToComplete)
					return;
				masterController.startActivity = "StartNewGame";
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




	public void newGameCallback() {

		if (!freePlay) {

				if (playcodeInput.text != CorrectPlayCode) {
					return;
				}


			gameController.quickSaveInfo.playcode = playcodeInput.text;
			gameController.saveQuickSaveInfo ();
		}

		masterController.playSound (buttonSound);

		goBackButton.enabled = true;
		sessionMenu.SetActive (false);
		if (continueGame) {
			SessionMenu2Cont.SetActive (true);
			instructionsButton.SetActive (true);
		} else {
			SessionMenu2.SetActive (true);
			instructionsButton.SetActive (true);
		}

	}


	public void newUserCallback() {

		masterController.playSound (buttonSound);

	}


	public void registerCallback() {

		masterController.playSound (buttonSound);

		whiteCover.fadeOutTask (this);
		substate0 = 2;
	}


	public void signInCallback() {
		whiteCover.fadeOutTask (this);
		substate0 = 4;
	}


	public void logoutCallback() {

		masterController.playSound (buttonSound);

		noMoarMagicPanel.scaleOut ();

		creditsHUD.text = "Créditos: ";
		buyButton.GetComponent<Button> ().interactable = false;

		loginPasswd.text = "";

		whiteCover.fadeOutTask (this);
		substate0 = 500;
		state0 = 2;
		gameController.setUserLogin ("");
		gameController.setUserPass ("");
		gameController.saveData ();
	}

	public void continueGameCallback() {

		masterController.playSound (buttonSound);

		substate0 = 4000;
		fader.fadeOutTask (this);

	}

	public void createGameCallback() {

		//if (accountCredits > 0) {

			masterController.playSound (buttonSound);
			continueGamePanelStartNewGame = true;
			if (continueGame) {
				continueGamePanel.scaleIn ();
			} else {
				substate0 = 600; // waiting for fadeout
				fader.fadeOutTask (this);
			}

		//} else {
		//	noMoarCreditsPanel.scaleIn ();
		//}

	}


	public void joinGameCallback() {

		//if (accountCredits > 0) {

			masterController.playSound (buttonSound);
			continueGamePanelStartNewGame = false;
			if (continueGame) {
				continueGamePanel.scaleIn ();
			} else {
				substate0 = 700; // waiting for fadeout
				fader.fadeOutTask (this);
			}

	//	} else {
	//		noMoarCreditsPanel.scaleIn ();
	//	}

	}

	public void continuePanelYes() {
		continueGamePanel.scaleOut ();
		gameController.resetQuickSaveInfo ();
		if (continueGamePanelStartNewGame) {
			substate0 = 600; // waiting for fadeout
			fader.fadeOutTask (this);
		} else {
			substate0 = 700; // waiting for fadeout
			fader.fadeOutTask (this);
		}
	}

	public void continuePanelNo() {
		continueGamePanel.scaleOut ();
	}

	public void goBackCallback() {



		if (SessionMenu2.activeSelf == true) {
			SessionMenu2.SetActive (false);
			sessionMenu.SetActive (true);
			instructionsButton.SetActive (true);
			goBackButton.enabled = false;
		} 

		else if (SessionMenu2Cont.activeSelf == true) {
			SessionMenu2Cont.SetActive (false);
			sessionMenu.SetActive (true);
			instructionsButton.SetActive (true);
			goBackButton.enabled = false;
		} 

		else {
			whiteCover.fadeOutTask (this);
			substate0 = 500;
		}

	}
		
	public void tacOKButton() {
		if (taccanvas.activeSelf == true)
			taccanvas.SetActive (false);
		else
			taccanvas.SetActive (true);
	}





}
