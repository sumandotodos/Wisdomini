using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DossierControllerLite_multi : Task, LongTouchListener {

	public GameObject extraScreen;
	public CircpleDeploy[] heroesElements;
	public LifeTestActivityController_multi lifeTestController;
	public UIFaderScript explainPageDownArrow;
	public UIFaderScript explainPageUpArrow;
	public HeroController_multi heroController;
	public UITip heroExplainTip;

	public UITextAutoFader explain;
	public UIFaderScript explainCanvas;

	public FGTable nombrePlayersTable;
	public Text nombrePlayersText;

	public Texture[] playerImgs;

	public RawImage[] energiesRawImage;

	public UIFaderScript fader;

	public GameObject dossierCanvas;

	public Text monedosText;

	public GameController_multi gameController;

//	public CircpleDeploy coinsHelp;

	public RawImage localPlayer;

	public Text semillasText;

	public RawImage forestImg;
//	public RawImage secondaryWisdom3Img;

	public Texture[] wisdomImage;
	public Texture[] forestImage;

//	public UIAutoFader bgFader;
//	public CircpleDeploy powerCircle;
//	public CircpleDeploy[] heros;

	public void refreshDossier() {
		// refresh energies
		for (int i = 0; i < energiesRawImage.Length; ++i) {
			if (gameController.playerList [gameController.localPlayerN].hasWisdom (i)) {
				energiesRawImage [i].enabled = true;
			} else
				energiesRawImage [i].enabled = false;
		}
		localPlayer.texture = playerImgs [gameController.localPlayerN];
		forestImg.enabled = false;
		for (int i = 0; i < gameController.forestOwner.Length; ++i) {
			if (gameController.forestOwner [i] == gameController.localPlayerN) {
				forestImg.texture = forestImage [i];
				forestImg.enabled = true;
				break;
			}
		}
		monedosText.text = "" + gameController.playerList [gameController.localPlayerN].gold;
		semillasText.text = "" + gameController.playerList [gameController.localPlayerN].seeds;

	}

	public void startDossierActivity(Task w) {
		nombrePlayersText.text = (string)nombrePlayersTable.getElement (0, gameController.localPlayerN);
		extraScreen.SetActive (false);
		waiter = w;
		refreshDossier ();
		dossierCanvas.SetActive (true);
		w.isWaitingForTaskToComplete = true;
		fader.Start ();
		fader.setFadeValue (1.0f);
		fader.fadeIn ();

	}

	void Start() {
		dossierCanvas.SetActive (false);
	}

	int state;
	void Update() {
		if (state == 0) {

		}
		if (state == 1) {
			if (!isWaitingForTaskToComplete) {
				state = 0;
				dossierCanvas.SetActive (false);
				notifyFinishTask ();
			}
		}
		if (state == 10) { // waiting for extraScreen fadeout to finish
			if (!isWaitingForTaskToComplete) {
				state = 11;
				extraScreen.SetActive (true);
				heroExplainTip.Start ();
				heroExplainTip.show ();
				fader.fadeIn ();
				heroController.setNElements (7);
				heroController.setUpHeroes ();
				for (int i = 0; i < heroesElements.Length; ++i) {
					heroesElements [i].reset ();
					heroesElements [i].extend ();
				}
			}
		}
		if (state == 11) { // nothing...

		}
		if (state == 12) { // wait for returning from extraScreen fadeout to finish
			if (!isWaitingForTaskToComplete) {
				state = 0;
				extraScreen.SetActive (false);
				fader.fadeIn ();
			}
		}
		if (state == 20) { // waiting for hero to retract
			if (!isWaitingForTaskToComplete) {
				heroController.setUpHeroClass (selectedClass);
				heroController.extend ();
				state = 21;
				ScreenState = 2;
			}
		}
		if (state == 21) {

		}
		if (state == 22) {
			if (!isWaitingForTaskToComplete) {
				heroController.setNElements (7);
				heroController.setUpHeroes ();
				heroController.extend ();
				ScreenState = 1;
				state = 11;
			}
		}
	}

	public void goBackButton() {
		if (state == 0) {
			state = 1;
			fader.fadeOutTask (this);
		} else if (state == 11) {
			state = 12;
			fader.fadeOutTask (this);
		} else if (state == 21) {
			state = 22;
			heroController.retractTask (this);
		}
	}

	public void showHelpButton() {
		state = 10;
		fader.fadeOutTask (this);
	}

	int selectedClass;



	public void clickOnElement(int e) {
		if (state == 11) {
			selectedClass = e;
			heroesElements [0].retractTask (this);
			for (int i = 1; i < heroesElements.Length; ++i) {
				heroesElements [i].retract ();
			}
			state = 20;
		}
	}

	int ScreenState = 1;
	int explainPage = 0;
	int explainHero = 0;
	public void longTouch(int id) {
		
		if (ScreenState == 1) { // classes
			
			lifeTestController.heroesNames.rosetta = lifeTestController.rosettaWrap.rosetta;
			lifeTestController.heroesNames.reset ();
			lifeTestController.heroesDescritions.rosetta = lifeTestController.rosettaWrap.rosetta;
			lifeTestController.heroesDescritions.reset ();
			explain.Start ();
			explainCanvas.Start ();
			string name = lifeTestController.heroesNames.getString (id);
			string descr = (string)lifeTestController.heroExplainTable.getElement (0, id);//heroesDescritions.getString (id);
			string[] pages = descr.Split ('#');
			descr = descr.Replace ("<br>", "\n");
			explain.gameObject.GetComponent<Text> ().text = name + "\n\n" + pages [0];
			explain.fadein ();
			explainPage = 0;
			explainHero = id;
			//explainPageDownArrow.fadeOut ();
			//explainPageUpArrow.fadeOut ();
			if (pages.Length > 1)
				explainPageDownArrow.fadeOut ();
			explainCanvas.fadeOut ();
		} else if (ScreenState == 2) {

			explain.Start ();
			explainCanvas.Start ();
			string name = (string)lifeTestController.individualsExplainTable [selectedClass].getElement (0, id);
			string descr = (string)lifeTestController.individualsExplainTable[selectedClass].getElement (1, id);
			string[] pages = descr.Split ('#');
			descr = descr.Replace ("<br>", "\n");
			explain.gameObject.GetComponent<Text> ().text = name + "\n\n" + pages [0];
			explain.fadein ();
			explainPage = 0;
			explainHero = id;
			//explainPageDownArrow.fadeOut ();
			//explainPageUpArrow.fadeOut ();
			if (pages.Length > 1)
				explainPageDownArrow.fadeOut ();
			explainCanvas.fadeOut ();
		}
	}

	//ui callbacks
	public void explainHeroPageNextButton() {
		string name = lifeTestController.heroesNames.getString(explainHero);
		++explainPage;
		string data = (string)lifeTestController.heroExplainTable.getElement (0, explainHero);
		string[] pages = data.Split ('#');
		if (explainPage >= pages.Length) {
			--explainPage;
			return;
		}
		explain.gameObject.GetComponent<Text> ().text = name + "\n\n" + pages[explainPage];
		explainPageDownArrow.fadeOut ();
		explainPageUpArrow.fadeOut ();
		if (explainPage < pages.Length - 1)
			explainPageDownArrow.fadeOut ();
		else
			explainPageDownArrow.fadeIn ();



	}

	public void explainHeroPagePrevButton() {
		if (explainPage == 0)
			return;
		string name = lifeTestController.heroesNames.getString(explainHero);
		--explainPage;
		string data = (string)lifeTestController.heroExplainTable.getElement (0, explainHero);
		string[] pages = data.Split ('#');
		explain.gameObject.GetComponent<Text> ().text = name + "\n\n" + pages[explainPage];
		explainPageDownArrow.fadeOut ();
		if (explainPage > 0)
			explainPageUpArrow.fadeOut ();
		else
			explainPageUpArrow.fadeIn ();

	}

	public void hideExplain() {
		explainCanvas.fadeIn ();
		explainPageDownArrow.fadeIn ();
		explainPageUpArrow.fadeIn ();
		explain.fadeout ();
	}

}
	