using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DossierController_multi : Task, LongTouchListener {

	public RosettaWrapper rwrapper;

	public GameController_multi gameController;

	public CircpleDeploy coinsHelp;

	public Texture[] playerImgs;

	public RawImage[] energiesRawImage;

	public RawImage localPlayer;
	public Text goldText;
	public Text gompaText;
	public Text schoolText;
	public Text seedText;
	public Text bumisText;
	public Text initiaText;
	public Text initiaClassText;
	public Text heroText;

	public RawImage forestImg;
	//public RawImage mainWisdomImg;
	//public RawImage secondaryWisdom1Img;
	//public RawImage secondaryWisdom2Img;
	public RawImage secondaryWisdom3Img;

	public Texture[] wisdomImage;
	public Texture[] forestImage;

	public UIAutoFader bgFader;
	public CircpleDeploy powerCircle;
	public CircpleDeploy[] heros;

	public Text[] heroValue;

	public StringBank heroesNames;
	public StringBank heroesDescritions;
	public UITextAutoFader explain;
	public UIFaderScript explainCanvas;
	public FGTable heroExplainTable;
	public UIFaderScript explainPageUpArrow;
	public UIFaderScript explainPageDownArrow;
	int explainPage = 0;
	int explainHero = 0;

	public void longTouch(int id) {
		heroesNames.rosetta = rwrapper.rosetta;
		heroesNames.reset ();
		heroesDescritions.rosetta = rwrapper.rosetta;
		heroesDescritions.reset ();
		explain.Start ();
		explainCanvas.Start ();

		string name = heroesNames.getString(id);
		string descr = (string)heroExplainTable.getElement (0, id);//heroesDescritions.getString (id);
		string[] pages = descr.Split('#');
		descr = descr.Replace ("<br>", "\n");
		explain.gameObject.GetComponent<Text> ().text = name + "\n\n" + pages[0];
		explain.fadein ();
		explainPage = 0;
		explainHero = id;
		if(pages.Length > 1) explainPageDownArrow.fadeOut ();
		explainCanvas.fadeOut ();
	}

	int state = 0;	// 0 idle
					// 1 waiting for fadeout to finish

	public UIFaderScript fader;

	public void startDossierActivity(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		init ();
	}

	public void init() {

		bgFader.fadeout ();
		bgFader.GetComponent<RawImage> ().raycastTarget = false;
		localPlayer.texture = playerImgs [gameController.localPlayerN];
		goldText.text = "" + gameController.playerList [gameController.localPlayerN].gold;
		gompaText.text = "" + gameController.playerList [gameController.localPlayerN].nGompas;
		schoolText.text = "" + gameController.playerList [gameController.localPlayerN].nSchools;
		seedText.text = "" + gameController.playerList [gameController.localPlayerN].seeds;
		bumisText.text = "" + gameController.playerList [gameController.localPlayerN].bumis;
		initiaText.text = "" + gameController.playerList [gameController.localPlayerN].nInitiations;
		initiaClassText.text = "" + gameController.playerList [gameController.localPlayerN].initiationsDifferentClasses;
		heroText.text = "" + gameController.playerList [gameController.localPlayerN].totalHeroes;

		for (int i = 0; i < energiesRawImage.Length; ++i) {
			energiesRawImage [i].enabled = false;
		}

		state = 0;
		int forestOwned = -1;
		for(int i = 0; i < GameController_multi.MAXFORESTS; ++i) {
			if (gameController.forestOwner [i] == gameController.localPlayerN) {
				forestOwned = i;
			}
		}
		if (forestOwned == -1) {
			forestImg.enabled = false;
		} else {
			forestImg.enabled = true;
			forestImg.texture = forestImage [forestOwned];
		}

		/* DEPRECATED
		if (gameController.playerList [gameController.localPlayerN].hasSecondaryWisdoms) {
			secondaryWisdom1Img.enabled = true;
			secondaryWisdom2Img.enabled = true;
			secondaryWisdom3Img.enabled = true;
		} else {
			secondaryWisdom1Img.enabled = false;
			secondaryWisdom2Img.enabled = false;
			secondaryWisdom3Img.enabled = false;
		}
		if (gameController.playerList [gameController.localPlayerN].mainWisdom != -1) {
			mainWisdomImg.enabled = true;
			mainWisdomImg.texture = wisdomImage [gameController.playerList [gameController.localPlayerN].mainWisdom];
		} else
			mainWisdomImg.enabled = false;
		*/


		for (int i = 0; i < energiesRawImage.Length; ++i) {
			
			if (gameController.playerList [gameController.localPlayerN].hasWisdom (i)) {
				energiesRawImage [i].enabled = true;
			
			}
		}


		powerCircle.reset ();
		for (int i = 0; i < heros.Length; ++i) {
			heros [i].reset ();
		}
		fader.fadeIn ();

	}

	// Use this for initialization
	void Start () {
	
	}

	public void backButton() {
		fader.fadeOutTask (this);
		state = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			backButton ();
		}
	
		if (state == 0) { // idling


		} 

		else if (state == 1) {

			if (!isWaitingForTaskToComplete) {
				notifyFinishTask ();
			}

		}

	}

	public void clickOnCoins() {
		bgFader.fadein ();
		bgFader.GetComponent<RawImage> ().raycastTarget = true;
		coinsHelp.extend ();
	}

	public void clickOnHeroes() {
		bgFader.fadein ();
		bgFader.GetComponent<RawImage> ().raycastTarget = true;
		for (int i = 0; i < heroValue.Length; ++i) {
			heroValue [i].text = "" + gameController.playerList [gameController.localPlayerN].heroAmount [i];
		}
		for (int i = 0; i < heros.Length; ++i) {
			heros [i].extend ();
		}

		powerCircle.extend ();
	}

	public void clickOnIniciations() {
		bgFader.fadein ();
		bgFader.GetComponent<RawImage> ().raycastTarget = true;
		for (int i = 0; i < heroValue.Length; ++i) {
			heroValue [i].text = "" + gameController.playerList [gameController.localPlayerN].nInitiations;
		}
		powerCircle.extend ();
		for (int i = 0; i < heros.Length; ++i) {
			heros [i].extend ();
		}
	}

	public void clickOnBg() {
		bgFader.fadeout ();
		bgFader.GetComponent<RawImage> ().raycastTarget = false;

		for(int i = 0; i < heros.Length; ++i) {
			heros [i].retract ();
		}

		powerCircle.retract ();

		coinsHelp.retract ();
	}



	//ui callbacks
	public void explainHeroPageNextButton() {
		string name = heroesNames.getString(explainHero);
		++explainPage;
		string data = (string)heroExplainTable.getElement (0, explainHero);
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
		string name = heroesNames.getString(explainHero);
		--explainPage;
		string data = (string)heroExplainTable.getElement (0, explainHero);
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
	