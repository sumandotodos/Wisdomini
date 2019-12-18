using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SchoolActivityController_mono : Task {


	// referencias a controladores
	public GameController_mono gameController;
	public RosettaWrapper rosettaWrap;


	// tips
	public UITip chooseEnergyTip;

	// imágenes de los héroes (categorías)
	public Texture[] category;

	// imágenes de los individuos
	public Texture[] warrior;
	public Texture[] master;
	public Texture[] philosopher;
	public Texture[] sage;
	public Texture[] explorer;
	public Texture[] wizard;
	public Texture[] yogi;
	[HideInInspector]
	public Texture[] hero; // para apuntar a uno de los anteriores


	// Stringbanks con los alias de los héroes
	public StringBank[] alias; 


	// cosas para instanciación
	public GameObject appIconPrefab;
	public GameObject appIconParent;


	// Textos
	public FGTable[] heroTable;


	// elementos de la pantalla
	public UIFaderScript fader;
	public Text questionText;
	public Text answerText;
	public GameObject QuestionScroll;
	public GameObject AnswerScroll;
	public UIFaderScript maruFader;
	public UIScaleFader qMarkScaler;
	public CircpleDeploy[] energyElement;
	public CircpleDeploy waitYinYang;
	public CircpleDeploy waitText;


	// variables internas
	int playerTouched;
	int energyTouched;
	int individualTouched;
	bool showAnswer = false;
	public int state = 0; 		
	List<GameObject> appiconList;
	float remaining = 0f;
	int selectedHeroClass;
	bool situationChosen = false;


	// constantes
	public float questionFontHeight = 132.0f;


	/*
	 * 
	 * 
	 *  Métodos auxiliares
	 * 
	 * 
	 */

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

	private string fixString(string s) {
		return s.Replace ("\\n", "\n").Replace ("<br>", "\n\n");
	}

	private void destroyAppIcons() {
		for (int i = 0; i < appiconList.Count; ++i) {
			Destroy (appiconList [i]);
		}
	}

	private void shrinkEnergyCircle() {

		for (int i = 1; i < energyElement.Length; ++i) {
			if(energyElement[i].gameObject.activeInHierarchy)
				energyElement [i].retract();
		}
		energyElement [0].retractTask (this);
	}

	private void setHeroArray(int h) {
		
		selectedHeroClass = h;
		switch (h) {
		case 0:
			hero = warrior;
			break;
		case 1:
			hero = master;
			break;
		case 2:
			hero = philosopher;
			break;
		case 3:
			hero = sage;
			break;
		case 4:
			hero = explorer;
			break;
		case 5:
			hero = wizard;
			break;
		case 6:
			hero = yogi;
			break;
		default: // 'impossible' case
			hero = warrior;
			break;
		}

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

		for (int i = 0; i < heroTable [selectedHeroClass].nRows(); i ++) {
			int indiv = getIndividualFromDescr ((string)heroTable [selectedHeroClass].getElement (0, i));
			if (indiv == (individualTouched+1)) {
				string text = fixString ((string)heroTable [selectedHeroClass].getElement (1, i));
				if (text.Contains ("<app>")) {
					text = text.Replace ("<app>", "");
					GameObject newGO = (GameObject)Instantiate (appIconPrefab, new Vector3 (Screen.width * 0.095f, instantiationY * 0.3f, 0), 
						                  Quaternion.Euler (0, 0, 0));

					appiconList.Add (newGO);

					newGO.GetComponent<AppIconHelper_mono> ().wisdom = energyTouched;
					newGO.GetComponent<AppIconHelper_mono> ().eventDispatcher = this;
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
		for (int i = 0; i < heroTable [selectedHeroClass].nRows (); i ++) {
			int indiv = getIndividualFromDescr ((string)heroTable [selectedHeroClass].getElement (0, i));
			if (indiv == (individualTouched + 1)) {
				answerText.text += ToRoman (rom++) + " - " + fixString ((string)heroTable [selectedHeroClass].getElement (2, i));
				answerText.text += "\n\n\n";
			}
		}

	}


	/*
	 * 
	 * 
	 *  Métodos inicialización y actualización
	 * 
	 * 
	 */

	void init() {

	}


	public void startSchoolTask(Task w) {

		fader.Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();

		maruFader.setFadeValue (0f);
		qMarkScaler.scaleOutImmediately ();

		showAnswer = true;
		toggleAnswer ();

		w.isWaitingForTaskToComplete = true;
		waiter = w;

		int index = 0;

		// disable all energy elements
		for (int i = 0; i < GameController_mono.MaxEnergies; ++i) {
			energyElement [i].gameObject.SetActive (true);
			energyElement [i].setNElements (7);
			energyElement [i].setIndex (i);
			energyElement [i].GetComponentInChildren<Text> ().text = "";
			energyElement [i].GetComponentInChildren<RawImage> ().texture = category [i];
			energyElement [i].yScale = 1.0f;
			energyElement [i].extend ();
		}

		appiconList = new List<GameObject>();

		MusicController.playTrack (0);
		MusicController.fadeIn ();

		state = 0;


	}



	// Update is called once per frame
	void Update () {
	
		// idling
		if (state == 0) { 

		}


		// waiting for circle to shrink
		if (state == 1) { 
			if (!isWaitingForTaskToComplete) {
				state = 2;
				for (int i = 0; i < GameController_mono.MaxPlayers; ++i) { // tell non playing players to show the gallery
					if (gameController.playerPresent [i] && (i != gameController.localPlayerN) && (i != playerTouched)) {
						
					}
				}
			}
		}


		// deploy energies
		if (state == 2) { 

			int playerNOfWisdoms = gameController.playerList [playerTouched].numberOfWisdoms ();
			if (playerNOfWisdoms == 0) {
				// end of thing

				state = 100;
				remaining = 5f;
				//notifyFinishTask();
				//state = 0;
			} else {
				
				gameController.addNotification (Notification.CONSIGUEUNASOLASEMILLA, gameController.getPlayerName (playerTouched), "1", "", gameController.getPlayerFemality(playerTouched));
				gameController.playerList[playerTouched].addSeeds(1); // local


				int nEnergies = playerNOfWisdoms;


				// enable selectively
				int index = 0;
				for (int i = 0; i < energyElement.Length; ++i) {
					//if ((gameController.playerList [playerTouched].mainWisdom == i) || (gameController.playerList [playerTouched].hasSecondaryWisdoms && !GameController_mono.isPrincipalWisdom (i))) {
					if(gameController.playerList[playerTouched].wisdoms.Contains(i)) {
						energyElement [i].gameObject.SetActive (true);
						energyElement [i].setNElements (nEnergies);
						energyElement [i].setIndex (index++);
						energyElement [i].yScale = 1.66f;
						energyElement [i].extend ();
					}
				}
				state = 3;
			}
		}


		// sit here waiting for user interaction. Energy circle will start to shrink
		if (state == 3) { 

		}


		// waiting for energy circle to shrink
		if (state == 4) { 
			if (!isWaitingForTaskToComplete) {
				alias [energyTouched].rosetta = rosettaWrap.rosetta;
				alias [energyTouched].reset ();
				// Prepare individuals and extend circle
				for (int i = 0; i < hero.Length; ++i) {
					string str = alias [energyTouched].getNextString ();
					energyElement [i].GetComponentInChildren<Text> ().text = str;//Utils.chopSpaces(str);
					energyElement [i].setIndex (i);
					energyElement [i].setNElements (hero.Length);
					energyElement [i].GetComponentInChildren<RawImage> ().texture = hero [i];
					energyElement [i].yScale = 1.33f;
					energyElement [i].extend ();

				}
				state = 5;

			}
		}


		// sit here waiting for user interaction. Individuals circle will start to shrink
		if (state == 5) { 

		}


		// waiting for individuals circle to shrink
		if (state == 6) { 
			if (!isWaitingForTaskToComplete) {
				maruFader.fadeOut ();
				qMarkScaler.scaleIn ();
				prepareTexts ();
				state = 7;
			}
		}


		// small delay
		if (state == 100) {
			remaining -= Time.deltaTime;
			if (remaining < 0f) {
				state = 101;

			}
		}


		// return point
		if (state == 101) {
			notifyFinishTask ();
			MusicController.fadeOut ();
			state = 0;
			questionText.text = "";
			qMarkScaler.scaleOutImmediately ();
			maruFader.setFadeValue (0.0f);
		}

		if (state == 1000) {
			if (!isWaitingForTaskToComplete) {
				state = 0;
				questionText.text = "";
				qMarkScaler.scaleOutImmediately ();
				maruFader.setFadeValue (0.0f);
				notifyFinishTask ();
				MusicController.fadeOut ();
			}
		}

	}



	/*
	 * 
	 * 
	 *  Métodos llamados por red
	 * 
	 * 
	 */

	public void finishSchool(int score, int individual) {
		if (state == 5) {
			if (score == 0) {

			} else if (score == 1) {

			}
			state = 0;
			waitYinYang.reset ();
			waitText.reset ();
			notifyFinishTask ();
			MusicController.fadeOut ();
		}
	}


	/*
	 * 
	 * 
	 *  Métodos del UI
	 * 
	 * 
	 */

	public void toggleAnswer() {

		showAnswer = !showAnswer;

		QuestionScroll.SetActive (!showAnswer);
		AnswerScroll.SetActive (showAnswer);


	}

	public void touchOnYY() {
		fader.fadeOutTask (this);
		state = 1000;
	}


	public void touchEnergy(int e) {

		if (state == 0) { // tocamos una categoría de héroes
			energyTouched = e;
			shrinkEnergyCircle ();
			setHeroArray (e);
			state = 4;
		} else { // tocamos un individuo
			individualTouched = e;
			shrinkEnergyCircle ();

			state = 6;
		}
	}


	public void clickAppIcon(int w, int i) {



	}



}
