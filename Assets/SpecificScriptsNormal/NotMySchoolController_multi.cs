﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class NotMySchoolController_multi : Task, ClickEventReceiver {

	public Texture[] warrior;
	public Texture[] master;
	public Texture[] philosopher;
	public Texture[] sage;
	public Texture[] explorer;
	public Texture[] wizard;
	public Texture[] yogi;

	int ownerPlayer;

	public UIGeneralFader[] votationButtons;
	public UIScaleFader interrogationButton;

	public GameObject theCanvas;
	public GameObject auxCanvas;

	public GameObject appIconPrefab;

	public GameObject appIconParent;

	public AudioClip questionMarkSound_N;

	public RawImage answersBG;

	public LifeTest lifeTestObject;

	[HideInInspector]
	public int turnPlayer;

	public float questionFontHeight = 35.0f;

	[HideInInspector]
	public Texture[] hero;
//	[HideInInspector]
//	public StringBank[] heroString;

	// Textos
	public FGTable[] heroTable;
	public FGTable herotab;

	/*public CircpleDeploy questionMark;
	public CircpleDeploy batsuMark;
	public CircpleDeploy maruMark;
	public CircpleDeploy tickMark;*/

//	public StringBank[] warriorStrings;
//	public StringBank[] masterStrings;
//	public StringBank[] philosopherStrings;
//	public StringBank[] sageStrings;
//	public StringBank[] explorerStrings;
//	public StringBank[] wizardStrings;
//	public StringBank[] yogiStrings;

	public Text questionText;
	public Text answerText;

	public RosettaWrapper rosettaWrap;
	public StringBank[] alias;

	public CircpleDeploy[] element;

	public GameController_multi gameController;

	public Text exampleSituationText;

	int chosenIndividual = -1;

	List<GameObject> appiconList;

	int heroWisdom;

	int state = 0; // 0: idle
					// 1: waiting fot shit

	public static string ToRoman(int number)
	{
		//if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
		if (number < 1) return string.Empty;            
		if (number >= 1000) return "M" + ToRoman(number - 1000);
		if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
		if (number >= 500) return "D" + ToRoman(number - 500);
		if (number >= 400) return "CD" + ToRoman(number - 400);
		if (number >= 100) return "C" + ToRoman(number - 100);            
		if (number >= 90) return "XC" + ToRoman(number - 90);
		if (number >= 50) return "L" + ToRoman(number - 50);
		if (number >= 40) return "XL" + ToRoman(number - 40);
		if (number >= 10) return "X" + ToRoman(number - 10);
		if (number >= 9) return "IX" + ToRoman(number - 9);
		if (number >= 5) return "V" + ToRoman(number - 5);
		if (number >= 4) return "IV" + ToRoman(number - 4);
		if (number >= 1) return "I" + ToRoman(number - 1);
		//throw new ArgumentOutOfRangeException("something bad happened");
		return "000"; // fuck this, just fail miserably!
	}

	public void startNotMySchoolTask(Task w, int wisdom, int owner) {

		situationChosen = false;
		questionText.enabled = true;
		answerText.enabled = false;
		answersBG.enabled = false;

		ownerPlayer = owner;

		for (int i = 0; i < votationButtons.Length; ++i) {
			votationButtons [i].Start ();
			votationButtons [i].fadeOutImmediately ();
			//votationButtons [i].GetComponent<RawImage> ().enabled = false;
		}
		interrogationButton.Start ();
		interrogationButton.scaleOutImmediately ();

		answersBG.enabled = false;

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		// wisdom = 0:   this player main wisdom (whatev)
		// wisdom = 1:     warrior (0)   
		// wisdom = 2:     explorer (4)
		// wisdom = 3:     wizard (5)
		int turnPlayerHero = wisdom; // for testing purposes. remove later
		/*if (wisdom == 0) {
			turnPlayerHero = 0;

		} 
		else if (wisdom == 1) {
			turnPlayerHero = 4;
		} 
		else if (wisdom == 2) {
			turnPlayerHero = 5;
		} 
		else if (wisdom == 3) {
			turnPlayerHero = gameController.playerList [gameController.localPlayerN].mainWisdom;
		}*/

		setHeroArray (turnPlayerHero);

		// ready the stringbanks
		for (int i = 0; i < alias.Length; ++i) {
			alias [i].rosetta = rosettaWrap.rosetta;
			alias [i].reset ();
		}

		// disable all circleDeplay elements
		/*for (int i = 0; i < element.Length; ++i) {
			element [i].gameObject.SetActive (false);
		}*/


		// and selectively re-enable some...
		for (int i = 0; i < hero.Length; ++i) {
			//element [i].gameObject.SetActive (true);
			//element [i].GetComponentInChildren<RawImage> ().texture = warrior [0]; //hero [i];
			string str = alias [turnPlayerHero].getNextString ();
			element [i].GetComponentInChildren<Text> ().text = str;//Utils.chopSpaces(str);
			element [i].setIndex (i);
			element [i].setNElements (hero.Length);
			element [i].GetComponentInChildren<RawImage> ().texture = hero [i];
			element [i].extend ();
			element [i].clickEventReceiver_N = this;
		}

		heroWisdom = wisdom;

		appiconList = new List<GameObject>();

		theCanvas.SetActive (true);
		auxCanvas.SetActive (false);
		//batsuMark.reset();
		//maruMark.reset();
		//tickMark.reset();
		//questionMark.reset ();

//		// notify & add bumi
//		gameController.addNotification (Notification.LOGRABUMI, gameController.getPlayerName (gameController.localPlayerN), 
//			"", "");
//		gameController.playerList [gameController.localPlayerN].bumis++;
//		gameController.networkAgent.broadcast ("addbumi:" + gameController.localPlayerN + ":");

	}

	// Use this for initialization
	void Start () {
		//startNotMySchoolTask (this, 1);
	}

	void setHeroArray(int h) {
		switch (h) {
		case 0:
			hero = warrior;
			//heroString = warriorStrings;
			break;
		case 1:
			hero = master;
			//heroString = masterStrings;
			break;
		case 2:
			hero = philosopher;
			//heroString = philosopherStrings;
			break;
		case 3:
			hero = sage;
			//heroString = sageStrings;
			break;
		case 4:
			hero = explorer;
			//heroString = explorerStrings;
			break;
		case 5:
			hero = wizard;
			//heroString = wizardStrings;
			break;
		case 6:
			hero = yogi;
			//heroString = yogiStrings;
			break;
		default: // 'impossible' case
			hero = warrior;
			//heroString = warriorStrings;
			break;
		}
		herotab = heroTable [h];

	}

	private string fixString(string s) {
		return s.Replace ("\\n", "\n").Replace ("<br>", "\n\n");
	}

	private int getIndividualFromDescr(string descr) {
		descr = descr.TrimEnd ('\n', '\r');
		int indexOf = -1;
		int res = -1;
		for (int i = 0; i <= 9; ++i) {
			string searchFor = "" + i;
			int foundIndex = descr.IndexOf (searchFor);
			if (foundIndex > indexOf)
				indexOf = foundIndex;
		}
		if (indexOf > -1) {
			int.TryParse (descr.Substring (indexOf), out res);
		}
		return res;
	}

	private void prepareTexts() {

		questionText.text = "";

		float factor = Screen.height / 1280.0f;
		float instantiationY = Screen.height * 3.33f * 0.8f;

		for (int i = 0; i < herotab.nRows(); i ++) {
			int indiv = getIndividualFromDescr ((string)herotab.getElement (0, i));
			if (indiv == (chosenIndividual+1)) {
				string text = fixString ((string)herotab.getElement (1, i));
				if (text.Contains ("<app>")) {
					text = text.Replace ("<app>", "");
					GameObject newGO = (GameObject)Instantiate (appIconPrefab, new Vector3 (Screen.width * 0.095f, instantiationY * 0.3f, 0), 
						                   Quaternion.Euler (0, 0, 0));

					appiconList.Add (newGO);

					newGO.GetComponent<AppIconHelper_multi> ().wisdom = heroWisdom;
					newGO.GetComponent<AppIconHelper_multi> ().eventDispatcher = this;
					newGO.transform.SetParent (appIconParent.transform);
					newGO.transform.localScale = Vector3.one;
				}
			
				questionText.text += text;
				questionText.text += "\n\n";
				int nLines;
				float fontHeight;

				Canvas.ForceUpdateCanvases ();
				//questionText.cachedTextGenerator.fontSizeUsedForBestFit
				fontHeight = questionFontHeight;
				nLines = questionText.cachedTextGenerator.lineCount;

				instantiationY = Screen.height * 3.33f * 0.8f
				//- Screen.height * 0.24f
				- ((nLines) * fontHeight) * factor;

			}

		}

		answerText.text = "\n\n";
		int rom = 1;
		for (int i = 0; i < herotab.nRows (); i ++) {
			answerText.text += ToRoman (rom++) + " - " + fixString((string)herotab.getElement (2, i));
			answerText.text += "\n\n\n";
		}

	}



	// Update is called once per frame
	void Update () {

		appIconParent.transform.position = questionText.transform.position;

		if (state == 0) { // idle

		}

		if (state == 1) { // waiting for circle to retract
			if (!isWaitingForTaskToComplete) {
				state = 2;
				//questionMark.extend ();
				for (int i = 0; i < votationButtons.Length; ++i) {
					votationButtons [i].fadeToOpaque ();
					//votationButtons [i].GetComponent<RawImage> ().enabled = true;
					//votationButtons [i].fadeOut ();
				}
				interrogationButton.scaleIn ();
				questionText.text = "";

				float factor = Screen.height / 1280.0f;
				//Debug.Log ("height:" + Screen.height);
				float instantiationY = Screen.height*3.33f * 0.8f;
				for (int i = 0; i < herotab.nRows(); i++) {
					int indiv = getIndividualFromDescr ((string)herotab.getElement (0, i));
					if (indiv == (chosenIndividual+1)) {
						string text = (string)herotab.getElement (1, i);
						if (text.Contains ("<app>")) {
							text = text.Replace ("<app>", "");
							GameObject newGO = (GameObject)Instantiate (appIconPrefab, new Vector3 (Screen.width * 0.095f, instantiationY * 0.3f, 0), 
								                  Quaternion.Euler (0, 0, 0));
						
							appiconList.Add (newGO);
							//newGO.GetComponent<RawImage> ().rectTransform.sizeDelta = new Vector2 (86, 100);
							//newGO.GetComponent<RawImage> ().rectTransform.localScale = Vector3.one;
							//newGO.GetComponent<RawImage> ().transform.localScale = Vector3.one;
							//newGO.transform.localScale = Vector3.one;
							newGO.GetComponent<AppIconHelper_multi> ().individual = chosenIndividual;
							newGO.GetComponent<AppIconHelper_multi> ().wisdom = heroWisdom;
							newGO.GetComponent<AppIconHelper_multi> ().eventDispatcher = this;
							newGO.transform.SetParent (appIconParent.transform);
							newGO.transform.localScale = Vector3.one;
						}
						questionText.text += text;
						questionText.text += "\n";
						int nLines;
						float fontHeight;

						Canvas.ForceUpdateCanvases ();
						//questionText.cachedTextGenerator.fontSizeUsedForBestFit
						fontHeight = questionFontHeight;
						nLines = questionText.cachedTextGenerator.lineCount;

						instantiationY = Screen.height * 3.33f * 0.8f
						//- Screen.height * 0.24f
						- ((nLines) * fontHeight) * factor;

					}

				}




				answerText.text = "\n\n";
				//questionText.GetComponent<UIFingerScroll> ().init ();
				int rom = 1;
				for (int i = 1; i < herotab.nRows (); i ++) {
					int indiv = getIndividualFromDescr ((string)herotab.getElement (0, i));
					if (indiv == (chosenIndividual + 1)) {
						answerText.text += ToRoman (rom++) + " - " + (string)herotab.getElement (2, i);
						answerText.text += "\n\n\n";
					}
				}
				//answerText.GetComponent<UIFingerScroll> ().init ();
				answerText.enabled = false;
				answersBG.enabled = false;
			}
		}
	
	}


	/* events callbacks */
	public void clickEvent(int id) {
		state = 1;
		chosenIndividual = id;
		element [0].retractTask (this);
		for (int i = 1; i < element.Length; ++i) {
			element [i].retract ();
		}
	}

	void destroyAppIcons() {
		for (int i = 0; i < appiconList.Count; ++i) {
			Destroy (appiconList [i]);
		}
	}

	public void clickOnBatsu() {
		gameController.networkAgent.sendCommand (turnPlayer, "finishschool:-1:" + chosenIndividual + ":");
		questionText.text = ""; // reset text
		notifyFinishTask ();
		gameController.addNotification (Notification.CONSIGUESEMILLA, gameController.getPlayerName (gameController.localPlayerN), 
			"1", "", gameController.getPlayerFemality());
		gameController.playerList [gameController.localPlayerN].addSeeds (1);
		gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":1:");
		state = 0;
		destroyAppIcons ();
	}

	public void clickOnScore(int n) {
		gameController.networkAgent.sendCommand (turnPlayer, "finishschool:"+n+":" + chosenIndividual + ":");
		questionText.text = ""; // reset text
		notifyFinishTask ();
		state = 0;
		destroyAppIcons ();


		// SEMILLAS PARA EL DUEÑO DE LA ESCUELA
		if (n == 1) {
			gameController.addNotification (Notification.CONSIGUEUNASOLASEMILLA, gameController.getPlayerName (gameController.localPlayerN), 
				"1", "", gameController.getPlayerFemality ());
		} else {
			gameController.addNotification (Notification.CONSIGUESEMILLAS, gameController.getPlayerName (gameController.localPlayerN), 
				""+n, "", gameController.getPlayerFemality ());
		}
		gameController.playerList [gameController.localPlayerN].addSeeds (n);
		gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":"+n+":");



		// SEMILLAS PARA EL QUE CAE EN LA ESCUELA
		if (n == 1) {
			gameController.addNotification (Notification.CONSIGUEUNASOLASEMILLA, gameController.getPlayerName (ownerPlayer), "1", "", gameController.getPlayerFemality (ownerPlayer));
		} else {
			gameController.addNotification (Notification.CONSIGUESEMILLAS, gameController.getPlayerName (ownerPlayer), ""+n, "", gameController.getPlayerFemality (ownerPlayer));
		}
		gameController.playerList[ownerPlayer].addSeeds(1); // local
		gameController.networkAgent.broadcast ("addseeds:" + ownerPlayer + ":" + 1 + ":"); // global
	}

	public void clickOnMaru() {
		gameController.networkAgent.sendCommand (turnPlayer, "finishschool:0:" + chosenIndividual + ":");
		questionText.text = ""; // reset text
		notifyFinishTask ();
		state = 0;
		destroyAppIcons ();


		gameController.addNotification (Notification.GANACARTA, gameController.getPlayerName (turnPlayer), "", "", gameController.getPlayerFemality(turnPlayer));
	
		 
		gameController.addNotification (Notification.CONSIGUEUNASOLASEMILLA, gameController.getPlayerName (gameController.localPlayerN), 
			"1", "", gameController.getPlayerFemality());
		
		gameController.playerList [gameController.localPlayerN].addSeeds (1);
		gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":1:");
	}

	public void clickOnTick() {
		gameController.networkAgent.sendCommand (turnPlayer, "finishschool:1:" + chosenIndividual + ":");
		questionText.text = ""; // reset text
		notifyFinishTask ();
		state = 0;
		destroyAppIcons ();

	
		gameController.addNotification (Notification.GANACARTAYSEMILLA, gameController.getPlayerName (turnPlayer), "", "", gameController.getPlayerFemality(turnPlayer));
		gameController.playerList[turnPlayer].addSeeds(1); // local
		gameController.networkAgent.broadcast ("addseeds:" + turnPlayer + ":" + 1 + ":"); // global


		gameController.addNotification (Notification.CONSIGUESEMILLA, gameController.getPlayerName (gameController.localPlayerN), 
			"1", "", gameController.getPlayerFemality());
		gameController.playerList [gameController.localPlayerN].addSeeds (1);
		gameController.networkAgent.broadcast ("addseeds:" + gameController.localPlayerN + ":1:");

		//}
	}

	bool situationChosen = false;
	public void clickAppIcon(int w, int i) {

		if (!situationChosen) {
			LifeTestNode[] nodes = lifeTestObject.arrayByHero (w);
			LifeTestNode node = nodes [i];
			StringBank sb = node.stringBank;
			sb.rosetta = rosettaWrap.rosetta;

			int s = Random.Range (0, sb.nItems ());

			exampleSituationText.text = sb.getString (s);
			situationChosen = true;
		}

		theCanvas.SetActive (false);
		auxCanvas.SetActive (true);
	}

	public void revealAnswers() {
		if (answerText.enabled == true) {
			questionText.enabled = true;
			answerText.enabled = false;
			answersBG.enabled = false;
			//answerText.GetComponent<UIFingerScroll> ().setEnabled (false);
			//questionText.GetComponent<UIFingerScroll> ().setEnabled (true);
		} else {
			questionText.enabled = false;
			answerText.enabled = true;
			answersBG.enabled = true;
			//answerText.GetComponent<UIFingerScroll> ().setEnabled (true);
			//questionText.GetComponent<UIFingerScroll> ().setEnabled (false);
		}
		if(questionMarkSound_N!=null) {
			gameController.masterController.playSound (questionMarkSound_N);
		}
	}

	public void closeAux() {
		theCanvas.SetActive (true);
		auxCanvas.SetActive (false);
	}

}
	