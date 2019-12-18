using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LifeTestActivityController_mono : Task, LongTouchListener {

	public UITip turnTip;
	public UITip nonTurnTip;

	public GameObject myTurnCanvas;
	public GameObject notMyTurnCanvas;
	public GameObject rakingCanvas;
	public GameObject dossierCanvas;
	public GameObject lifeTestCanvas;
	public GameObject lifeTestAllCanvas;

	public GameObject OKButtonGO;

	public VotationController_mono votationController;

	public UITextFader defeatFader;

	public const int NormalMode = 0;
	public const int SummonMode = 1;
	public const int NonTurnMode = 2;

	int mode;

	public int Sabiduria;
	public int Sabio;
	public int QuestionNumber;
	//public StringBank QuestionBank;

	bool hintLock = false;

	public int SummoningPlayer;

	public int lastChanceResult;

	public UITextAutoFader explain;
	public UIFaderScript explainCanvas;

	public DiablilloMeditadorController diablilloMeditadorController;

	public GameController_mono gameController;

	public ExplainController_mono explainController;

	public TextureSwitch texSwitch;
	public TextureSwitch headAnimation;

	public RawImage dimmer;

	public UILongTouch[] longTouches;

	public Texture[] bigHeroes;
	public Texture[] warriorIndividuals;
	public Texture[] masterIndividuals;
	public Texture[] philosopherIndividuals;
	public Texture[] sageIndividuals;
	public Texture[] explorerIndividuals;
	public Texture[] wizardIndividuals;
	public Texture[] yogiIndividuals;

	public RawImage OKButtonImg;

	public RawImage backgrImage;

	public GameObject scrollPanel;
	public GameObject leftPanel; // disconnect one or another to save battery
	public GameObject rightPanel; 

	//public LifeTest lifeTestObject;
	public FGTable lifeTestTable;
	public RosettaWrapper rosettaWrap;

	/*	*/
	public UIScrollHide bigHeroBackgr;
	public UIScrollHide bigHero;
	public UIScrollHide bigHeroIndividual;
	public UITextScrollHide bigHeroScroll;
	public Text bigHeroText;

	public StringBank heroesNames;
	public StringBank heroesDescritions;

	public ShodownController shodownController;
	public LastChanceController_mono lastChanceController;
	public LifeTestVoteResultController_mono resultController;

	public GameObject shodownCanvas;
	public GameObject resultCanvas;


	public FGTable heroExplainTable;
	public UIFaderScript explainPageUpArrow;
	public UIFaderScript explainPageDownArrow;
	int explainPage = 0;
	int explainHero = 0;

	public FGTable[] individualsExplainTable;

	public UIScaleFader[] hintOrbScaler;

	public UIFaderScript fader;
	//public Text bigHeroText;

	public const int nHints = 3;
	bool[] hintUsed;

	bool isTurnPlayer;

	Vector3 touchCoords;

	int state0; // slot 0: panel scrolling
	float timer0;


	int state1; // slot 1: head animation
	float timer1;


	int state2; // slot 2: hero selection controller
	float timer2;


	float eyesDelayTime;
	const float MinEyesDelayTime = 1.0f;
	const float MaxEyesDelayTime = 3.0f;
	const float FrameDelay = 0.125f;

	public Vector3 scrollInitialPos;
	bool initialPosCapture = false;
	float scroll;
	const float scrollSpeed = 2000.0f;

	public Text theText;

	public HeroController_mono heroController;

	int clickedHero;

	LifeTestNode answers;
	int questionNumber;

	int nIndivs;

	bool isMyTurn;

	int categoryAnswer;
	int individualAnswer;

	[HideInInspector]
	public int activityResult;




	public void hideExplain() {
		explainCanvas.fadeIn ();
		explainPageDownArrow.fadeIn ();
		explainPageUpArrow.fadeIn ();
		explain.fadeout ();
	}



	public void longTouch(int id) {
		if (ScreenState == 1) { // classes
//			if (id == 2)
//				id = 4;
//			else if (id == 3)
//				id = 5;
		
			heroesNames.rosetta = rosettaWrap.rosetta;
			heroesNames.reset ();
			heroesDescritions.rosetta = rosettaWrap.rosetta;
			heroesDescritions.reset ();
			explain.Start ();
			explainCanvas.Start ();
			string name = heroesNames.getString (id);
			string descr = (string)heroExplainTable.getElement (0, id);//heroesDescritions.getString (id);
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
			string name = (string)individualsExplainTable [categoryAnswerFlat].getElement (0, id);
			string descr = (string)individualsExplainTable[categoryAnswerFlat].getElement (1, id);
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

	bool isCategoryCorrect(int cat) {

		if (answers.sabiduria == cat) // try main sabiduria
			return true;
		else {
			LifeTestAuxNode auxNode = answers.others [QuestionNumber]; // try other sabidurias
			for(int i = 0; i<auxNode.sabiduria.Count; ++i) {
				if (auxNode.sabiduria [i] == cat)
					return true;
			}
		}

		// give up
		return false;

	}

	// precondition: category has already been checked
	bool isIndividualCorrect(int indiv) {
		
		if (answers.sabio == indiv) // try main sabio
			return true;
		else {
			LifeTestAuxNode auxNode = answers.others [QuestionNumber]; // try other sabios
			for(int i = 0; i<auxNode.sabio.Count; ++i) {
				if (auxNode.sabio [i] == indiv)
					return true;
			}
		}

		// give up
		return false;


	}

	public int selectSituationFromFGTable() {
		return lifeTestTable.getNextRowIndex ();
	}

//	public void selectSituation(out int sabiduria, out int sabio, out int question) {
//
//		int r = Random.Range(0,2);
//		sabiduria = r;
////		if (r == 1)
////			sabiduria = 1;
////		if (r == 2)
////			sabiduria = 4;
////		if (r == 3)
////			sabiduria = 5;
//
//
//
//		LifeTestNode[] lifeTestNodeList = null;
//		switch (sabiduria) {
//		case 0:
//			lifeTestNodeList = lifeTestObject.guerreros;
//			break;
//		case 1:
//			lifeTestNodeList = lifeTestObject.maestras;
//			break;
//		case 2:
//			lifeTestNodeList = lifeTestObject.filosofos;
//			break;
//		case 3:
//			lifeTestNodeList = lifeTestObject.sabios;
//			break;
//		case 4:
//			lifeTestNodeList = lifeTestObject.exploradoras;
//			break;
//		case 5:
//			lifeTestNodeList = lifeTestObject.magas;
//			break;
//		case 6:
//			lifeTestNodeList = lifeTestObject.yoguis;
//			break;
//		}
//		sabio = Random.Range (0, lifeTestNodeList.Length);
//
//		LifeTestNode testNode = lifeTestNodeList [sabio];
//		answers = testNode;
//		StringBank testBank = testNode.stringBank;
//		testBank.reset ();
//		testBank.rosetta = rosettaWrap.rosetta;
//
//		question = Random.Range (0, testBank.nItems ());
//
//	}

	void init(int forceQuestion) {

		//texSwitch.setChannel (3); // gameController.localPlayerN

		timer0 = 0.0f;
		state0 = 0;
		timer1 = 0.0f;
		state1 = 0;
		timer2 = 0.0f;
		state2 = 0;
		scroll = 0;

		diablilloMeditadorController.go ();

		dimmer.enabled = false;

		OKButtonImg.enabled = true;

		//if (initialPosCapture == false) {
			scrollInitialPos = new Vector3 (0, 0, 0); // BOOM! Headshot of arbitrary constants //scrollPanel.transform.localPosition;
			initialPosCapture = true;
		//} else {
			scrollPanel.transform.localPosition = new Vector3 (0, 0, 0);
		//}
		scrollPanel.transform.localScale = Vector3.one;

		heroController.setNElements (7); 
		heroController.reset ();
		heroController.setUpHeroes ();
		heroController.retract ();

		bigHeroBackgr.reset ();
		bigHero.reset ();
		bigHeroScroll.reset ();
		bigHeroIndividual.reset ();

		isMyTurn = (gameController.localPlayerN == gameController.playerTurn);
		// is it my turn, or isn't?

		OKButtonGO.SetActive (isMyTurn);

//		LifeTestNode[] lifeTestNodeList = null;
//		switch (forceSabiduria) {
//			case 0:
//				lifeTestNodeList = lifeTestObject.guerreros;
//				break;
//			case 1:
//				lifeTestNodeList = lifeTestObject.maestras;
//				break;
//			case 2:
//				lifeTestNodeList = lifeTestObject.filosofos;
//				break;
//			case 3:
//				lifeTestNodeList = lifeTestObject.sabios;
//				break;
//			case 4:
//				lifeTestNodeList = lifeTestObject.exploradoras;
//				break;
//			case 5:
//				lifeTestNodeList = lifeTestObject.magas;
//				break;
//			case 6:
//				lifeTestNodeList = lifeTestObject.yoguis;
//				break;
//		}


		 /*
		  *  gameController.broadcast ( " ... ")
		  * 
		  *   en el caso de los clientes, inicializar a -1, y hacer un pooling hasta que cambie
		  * 
		  * 
		  */

		/* retrieve a text and possible answers from data structures */

		//LifeTestNode testNode = lifeTestNodeList [forceSabio];
		//answers = testNode;
		//StringBank testBank = testNode.stringBank;
		//testBank.reset ();
		//testBank.rosetta = rosettaWrap.rosetta;

		// obtain questionNumer & get text from rosetta
		questionNumber = QuestionNumber;
		if (mode == NormalMode) {
			QuestionNumber = questionNumber = lifeTestTable.getNextRowIndex (); //Random.Range (0, testBank.nItems ());
		}


		//QuestionBank = testBank;

		activityResult = 0; // ni fu ni fa
	
		// update theText component
		theText.text = ((string)lifeTestTable.getElement(0, forceQuestion)).Replace("\\n", "\n\n");//testBank.getString (forceQuestion);
		//theText.gameObject.GetComponent<UIFingerScroll> ().init ();

		shodownCanvas.SetActive (false);
		lifeTestCanvas.SetActive (true);

		fader.fadeIn ();

		for (int i = 0; i < longTouches.Length; ++i) {
			longTouches [i].setEnabled (true);
		}
		// 

		//}

		for (int i = 0; i < Player.nHints; ++i) {
			hintOrbScaler [i].Start ();

			if (gameController.playerList [gameController.localPlayerN].hintUsed [i]) {
				hintOrbScaler [i].scaleOutImmediately ();
			} else
				hintOrbScaler [i].reset ();
		}

		hintLock = false;

		defeatFader.Start ();
		defeatFader.fadeOutImmediately ();

		//explainController.startExplanation(this, isTurnPlayer);
		state2 = 0;

		MusicController.playTrack (0);
		MusicController.fadeIn ();

	}

	public void startLifeTestActivity(Task w, int q, bool turnPlayer) {
		ScreenState = 0;
		resultCanvas.SetActive (false);

		if (mode == SummonMode) {
			QuestionNumber = q;

		}
		isTurnPlayer = turnPlayer;
		waiter = w;
		w.isWaitingForTaskToComplete = true;
		init (q);
	}

	public int result() {
		return activityResult;
	}

	// Use this for initialization
	void Start () {

		initialPosCapture = false;


		//init ();
	
	}

	int ScreenState = 0; // 0 : enunciado
						 // 1 : héroes
						 // 2 : individuos
	
	// Update is called once per frame
	void Update () { // viva el modelo de lonchas!

		/* slot 0: panel scrolling */

		if (state0 == -1) {
			scroll -= scrollSpeed * Time.deltaTime;
			if (scroll < 0f) {
				scroll = 0f;
				state0 = 0;
			}
		}

		if (state0 == 0) { // idling

		} else if (state0 == 1) {
			fader.fadeOutTask (this); // CAMBIO: fadeout y no muestra la rosca con héroes
			state0 = 0;
			state2 = 8;
//			scroll += scrollSpeed * Time.deltaTime;
//			if (scroll > 720.0f) {
//				scroll = 720.0f;
//				state0 = 0; // go back to idling
//				if (gameController.playerTurn == gameController.localPlayerN) {
//					turnTip.show ();
//				} else
//					nonTurnTip.show ();
//			}

		} 
		scrollPanel.transform.localPosition = scrollInitialPos + new Vector3 (-scroll, 0, 0);

		/* end of slot 0 */






		/* slot 1: head animation */

		if (state1 == 0) { // setting eyesDelayTime
			eyesDelayTime = Random.Range(MinEyesDelayTime, MaxEyesDelayTime);
			headAnimation.setChannel (0);
			state1 = 1;
		}

		else if (state1 == 1) { // looking right
			timer1 += Time.deltaTime;
			if (timer1 > eyesDelayTime) {
				timer1 = 0.0f;
				state1 = 2;
				headAnimation.setChannel (1);
			}
		}

		else if (state1 == 2) { // looking center-right
			timer1 += Time.deltaTime;
			if (timer1 > FrameDelay) {
				timer1 = 0.0f;
				state1 = 3;
				diablilloMeditadorController.changeDiablillo ();
				headAnimation.setChannel (2);
			}
		}

		else if (state1 == 3) { // looking center-left
			timer1 += Time.deltaTime;
			if (timer1 > FrameDelay) {
				timer1 = 0.0f;
				state1 = 4;
				headAnimation.setChannel (3);
				eyesDelayTime = Random.Range(MinEyesDelayTime, MaxEyesDelayTime);
			}
		}

		else if (state1 == 4) { // looking left
			timer1 += Time.deltaTime;
			if (timer1 > eyesDelayTime) {
				timer1 = 0.0f;
				state1 = 5;
				headAnimation.setChannel (2);
			}
		}

		else if (state1 == 5) { // looking center-left
			timer1 += Time.deltaTime;
			if (timer1 > FrameDelay) {
				timer1 = 0.0f;
				state1 = 6;
				diablilloMeditadorController.changeMeditador ();
				headAnimation.setChannel (1);
			}
		}

		else if (state1 == 6) { // looking center-right
			timer1 += Time.deltaTime;
			if (timer1 > FrameDelay) {
				timer1 = 0.0f;
				state1 = 1;
				eyesDelayTime = Random.Range(MinEyesDelayTime, MaxEyesDelayTime);
				headAnimation.setChannel (0);
			}
		}

		/* end of slot 1 */




		/* slot 2: activity */
		if (state2 == 0) { // idle on heroes

		
		} 

		if (state2 == 1) { // waiting for explain canvas to finish
			if (!isWaitingForTaskToComplete) {
				state2 = 0;
			}
		}

		// wait a little bit and then extend the wheel
		else if (state2 == 2) {
			timer2 += Time.deltaTime;
			if (timer2 > 0.5f) {
				timer2 = 0.0f;
				heroController.setNElements (7);
				heroController.extend ();
				state2 = 0;
			}
		} 

		// wait a little bit and then retract
		else if (state2 == 3) {
			timer2 += Time.deltaTime;
			if (timer2 > 0f) {
				heroController.hideMaru ();
				heroController.reset ();
				heroController.retract ();
				timer2 = 0.0f;
				state2 = 4;
			}
		} else if (state2 == 4) { // wait until the circle of heroes has retracted
			timer2 += Time.deltaTime;
			if (timer2 > 1.0f) {
//				if (clickedHero == 2)
//					clickedHero = 4;
//				if (clickedHero == 3)
//					clickedHero = 5;
				nIndivs = heroController.setUpHeroClass (clickedHero);
				heroController.extend ();
				state2 = 5;
			}

		} else if (state2 == 5) { // idling on individuals



		} 

		// // wait a little bit and then retract
		else if (state2 == 6) { 
			timer2 += Time.deltaTime;
			if (timer2 > 0f) {
				heroController.reset ();
				heroController.retractTask (this);
				state2 = 7;
			}

		} 

		// waiting for hero wheel to retract
		else if (state2 == 7) { 
			if (!isWaitingForTaskToComplete) {

				fader.fadeOutTask (this);
				state2 = 8;

			}
		} 

		// waiting for fadeout. We got wisdom and individual right.
		else if (state2 == 8) { 
			if (!isWaitingForTaskToComplete) {

				//shodownCanvas.SetActive (true);
				resultCanvas.SetActive (true);
				lifeTestCanvas.SetActive (false);
				//shodownController.startShodownTask (this, mode);
				resultController.startLifeTestVoteResultController_mono (this);

				state2 = 9;

			}
		} else if (state2 == 9) { // waiting for result activity to end

			if (!isWaitingForTaskToComplete) {
				dimmer.enabled = false;
				diablilloMeditadorController.stop ();
				notifyFinishTask ();
				MusicController.fadeOut ();

			}

		} 
		// wait for last chance to finish
		else if (state2 == 3000) {
			int usedOrbs = 0;
			for (int i = 0; i < gameController.playerList [gameController.localPlayerN].orbUsed.Length; ++i) {
				if (gameController.playerList [gameController.localPlayerN].orbUsed [i]) {
					++usedOrbs;
				}
			}
			if (usedOrbs < gameController.playerList [gameController.localPlayerN].orbUsed.Length) {
				lastChanceController.startLastChance (this);
				state2 = 3001;
			} else {
				notifyFinishTask ();
				MusicController.fadeOut ();
				state2 = 0;
			}
		} else if (state2 == 3001) {
			if (!isWaitingForTaskToComplete) {
				if (lastChanceResult == LastChanceController_mono.DEFEAT) {
					notifyFinishTask ();
					MusicController.fadeOut ();
					state2 = 0;
				} else {
					activityResult = 0;
					state2 = 0;
					notifyFinishTask ();
					MusicController.fadeOut ();
				}
			}
		}

		// small delay before showing big hero
		else if (state2 == 10) {
			// set the appropiate texture
			bigHero.gameObject.GetComponent<RawImage> ().texture = bigHeroes [answers.sabiduria];
			timer2 += Time.deltaTime;
			if (timer2 > 3.5f) {
				timer2 = 0.0f;
				state2 = 11;
			}
		}

		//
		else if (state2 == 11) {
			bigHeroBackgr.show ();
			timer2 = 0.0f;
			state2 = 12;
		} 

		//
		else if (state2 == 12) {
			timer2 += Time.deltaTime;
			if (timer2 > 0.25f) {
				bigHero.show ();
				bigHeroScroll.show ();
				state2 = 13;
			}
		}

		// waiting for screen touch
		else if (state2 == 13) {
			if (Input.GetMouseButtonDown (0)) {
				touchCoords = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp (0)) {
				if ((touchCoords - Input.mousePosition).magnitude < 1.0f) {
					fader.fadeOutTask (this);
					if (mode == SummonMode) {
						
					}
					state2 = 14;
				}
			}
		}

		// waiting for fadeout
		else if (state2 == 14) {
			if (!isWaitingForTaskToComplete) {

				defeatFader.fadeIn ();

				timer2 = 2.0f;
				state2 = 15;

			}
		}



		// show 'vuelve al principio' cartel
		else if (state2 == 15) {
			timer2 -= Time.deltaTime;
			if (Input.GetMouseButtonDown (0))
				timer2 = -1.0f;
			if (timer2 < 0.0f) {
				
				defeatFader.fadeOutTask (this);
				state2 = 116;



			}
		} else if (state2 == 116) {
			if (!isWaitingForTaskToComplete) {
				state2 = 0; // go back to previous
				dimmer.enabled = false;
				diablilloMeditadorController.stop ();
				notifyFinishTask ();
				MusicController.fadeOut ();
			}
		}

		// small delay before showing big hero
		else if (state2 == 16) {
			// set the appropiate texture
			bigHero.gameObject.GetComponent<RawImage> ().texture = bigHeroes [answers.sabiduria];
			Texture[] tex = null;
			switch (answers.sabiduria) {
			case 0:
				tex = warriorIndividuals;
				break;
			case 1:
				tex = masterIndividuals;
				break;
			case 2:
				tex = philosopherIndividuals;
				break;
			case 3:
				tex = sageIndividuals;
				break;
			case 4:
				tex = explorerIndividuals;
				break;
			case 5:
				tex = wizardIndividuals;
				break;
			case 6:
				tex = yogiIndividuals;
				break;
			}
			bigHeroIndividual.gameObject.GetComponent<RawImage> ().texture = tex [answers.sabio];
			timer2 += Time.deltaTime;
			if (timer2 > 2.5f) {
				timer2 = 0.0f;
				state2 = 17;
			}
		}

		//
		else if (state2 == 17) {
			bigHeroBackgr.show ();
			timer2 = 0.0f;
			state2 = 18;
		} 

		//
		else if (state2 == 18) {
			timer2 += Time.deltaTime;
			if (timer2 > 0.25f) {
				bigHero.show ();
				bigHeroIndividual.show ();
				bigHeroScroll.show ();
				state2 = 19;
			}
		}

		// waiting for screen touch
		else if (state2 == 19) {
			if (Input.GetMouseButtonDown (0)) {
				touchCoords = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp (0)) {
				if (((touchCoords - Input.mousePosition).magnitude) < 0.1f) {
					fader.fadeOutTask (this);
					state2 = 8; // proceed to shodown
				}
			}
		}

		// waiting for fadeout
		else if (state2 == 20) {
			if (!isWaitingForTaskToComplete) {
				state2 = 0; // go back to previous
				dimmer.enabled = false;
				diablilloMeditadorController.stop ();
				notifyFinishTask ();
				MusicController.fadeOut ();
			}
		}

		// waiting for retract
		else if (state2 == 60) {
			heroController.reset ();
			heroController.retractTask (this);
			state2 = 61;
		} else if (state2 == 61) {
			if (!isWaitingForTaskToComplete) {
				state2 = 0;
				state0 = -1; // go back to enunciado
			}
		}

		// waiting for retract
		else if (state2 == 70) {
			heroController.reset ();
			heroController.retractTask (this);
			state2 = 71;
		} else if (state2 == 71) {
			if (!isWaitingForTaskToComplete) {
				heroController.setNElements (7); 
				heroController.reset ();
				heroController.setUpHeroes ();
				heroController.extend ();
				ScreenState = 1;
				state2 = 0;
			}
		}

	}

	public void arrowGoBack() {
		if (ScreenState == 1) {
			state2 = 60;
		} else if (ScreenState == 2) {
			state2 = 70;
		}
	}


	/* callback methods */

	public void nextPanel() {
		state0 = 0;
		state2 = 9; // extend hero wheel
		fader.fadeOutTask(this);
		ScreenState = 0;
	}

	public void enableOKButton() {
		OKButtonImg.enabled = true;
	}

	int categoryAnswerFlat;

	public void clickOnHero(int id) {
		
		if(state2 == 0) { // if idling on categories...

			categoryAnswerFlat = id;
			categoryAnswer = id; // keep track of clicked hero class
//			if(categoryAnswer == 2) categoryAnswer = 4;
//			if (categoryAnswer == 3)
//				categoryAnswer = 5;

				ScreenState = 2;
				timer2 = 0.0f;
				state2 = 3; // retract hero wheel and proceed to individuals
				

		}
		if (state2 == 5) { // if idling on individuals...
			individualAnswer = id; // keep track of clicked individual hero

			if(isTurnPlayer) {
				timer2 = 0.0f;
				state2 = 6; // retract hero wheel
			}

		}
		clickedHero = id;
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

	// more ui callbacks
	public void hintOrbTouch(int id) {
		if (hintLock)
			return;
		hintOrbScaler [id].scaleOut ();
		hintLock = true;
		if (!gameController.playerList [gameController.localPlayerN].hintUsed [id]) {
			gameController.playerList [gameController.localPlayerN].hintUsed [id] = true;
			// do hint shit
			if(state2 == 0) { // if idling on categories...
				List<int> hidden = new List<int> ();
				while (hidden.Count < 4) {
					for (int i = 0; i < heroController.nElements; ++i) {
						if ((!isCategoryCorrect (i)) && (!hidden.Contains(i)) && (hidden.Count < 4)) {
							if (Random.Range (0.0f, 100.0f) < 50.0f) {
								hidden.Add (i);
								heroController.retract (i);
							}
						}
					}
				}
			}
			else if (state2 == 5) { // this can be factored a lot!
				int nToRemove = 1;
				if (nIndivs == 4)
					nToRemove = 2;
				if (nIndivs == 5)
					nToRemove = 2;
				if (nIndivs == 6)
					nToRemove = 3;
				if (nIndivs == 7)
					nToRemove = 4;
				List<int> hidden = new List<int> ();
				while (hidden.Count < nToRemove) {
					for (int i = 0; i < nIndivs; ++i) {
						if ((!isIndividualCorrect (i)) && (!hidden.Contains(i)) && (hidden.Count < nToRemove)) {
							if (Random.Range (0.0f, 100.0f) < 50.0f) {
								hidden.Add (i);
								heroController.retract (i);
							}
						}
					}
				}
				// 3 -> quita 1
				// 4 -> quita 2
				// 5-> quita 2
				// 6-> quita 3
				// 7->quita 4
			}
		}
	}



//	// Network callbacks
//
//	public void startSummonLifeTest(int sbio, int sria, int q, int sPlayer) {
//		myTurnCanvas.SetActive (false);
//		notMyTurnCanvas.SetActive (false);
//		dossierCanvas.SetActive (false);
//		rakingCanvas.SetActive (false);
//		lifeTestAllCanvas.SetActive (true);
//		startLifeTestActivity (this, LifeTestActivityController_mono.SummonMode, sbio, sria, q, sPlayer);
//	}

	public void startNonTurnLifeTest(int sabid, int sabio, int question) {
		myTurnCanvas.SetActive (false);
		notMyTurnCanvas.SetActive (false);
		dossierCanvas.SetActive (false);
		rakingCanvas.SetActive (false);
		lifeTestAllCanvas.SetActive (true);
		startLifeTestActivity (this, question, false);
	}

}
