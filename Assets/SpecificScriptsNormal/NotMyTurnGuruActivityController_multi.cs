using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NotMyTurnGuruActivityController_multi : Task {

	public GameController_multi gameController;
	public RosettaWrapper rosettaWrap;
	public FGTable type1Table;
	public FGTable type2Table;
	public FGTable type3Table;

	public UITextAutoFader missingLabel;
	public UITextAutoFader meaningLabel;
	public UITextAutoFader answerLabel;

	public GameObject guru;
	public GameObject BackgrBoat;
	public GameObject particles;

	public Text question;
	public Text answer;

	public RawImage ansBg;
	public GameObject questionMark;

	bool answerShow;

	public void startGuruActivityTask(Task w, int t, int q) {
		missingLabel.Start ();
		meaningLabel.Start ();
		missingLabel.reset ();
		meaningLabel.reset ();
		answerLabel.Start ();
		answerLabel.reset ();
		w.isWaitingForTaskToComplete = true;
		waiter = w;



		question.enabled = true;
		answer.enabled = false;
		answerShow = false;


//		if (t == 0) {
//			answerLabel.fadein ();
//			gameController.seedToPlayerController.answer.text = type1Test.getString (q + 1);
//			question.text = type1Test.getString (q);
//			answer.text = type1Test.getString (q + 1);
//			guru.SetActive (true);
//			particles.SetActive (true);
//			BackgrBoat.SetActive (false);
//			questionMark.SetActive (true);
//			answer.enabled = false;
//		}
//		if (t == 1) {
//			meaningLabel.fadein ();
//			gameController.seedToPlayerController.answer.text = type2Test.getString (q + 1);
//			question.text = type2Test.getString (q);
//			//answer.text = type2Test.getString (q + 1);
//			guru.SetActive (false);
//			particles.SetActive (false);
//			BackgrBoat.SetActive (true);
//			questionMark.SetActive (false);
//			//answer.enabled = false;
//		}
//		if (t == 2) {
//			missingLabel.fadein ();
//			gameController.seedToPlayerController.answer.text = type3Test.getString (q + 1);
//			question.text = type3Test.getString (q);
//			answer.text = type3Test.getString (q + 1);
//			guru.SetActive (true);
//			particles.SetActive (true);
//			BackgrBoat.SetActive (false);
//			questionMark.SetActive (true);
//			answer.enabled = false;
//		}
		string test = "";
		string ans = "";
		if (t == 0) {
			answerLabel.fadein ();
			test = (string)type1Table.getElement (0, q);
			ans = (string)type1Table.getElement(1, q);
			answer.enabled = false;
			guru.SetActive (true);
			particles.SetActive (true);
			BackgrBoat.SetActive (false);
			questionMark.SetActive (true);
		}
		if (t == 1) {
			meaningLabel.fadein ();
			test = (string)type2Table.getElement (0, q);
			ans = (string)type2Table.getElement (1, q);
			guru.SetActive (false);
			particles.SetActive (false);
			BackgrBoat.SetActive (true);
			questionMark.SetActive (false);
		}
		if (t == 2) {
			missingLabel.fadein ();
			test = (string)type3Table.getElement (0, q);
			ans = (string)type3Table.getElement (1, q);
			guru.SetActive (true);
			particles.SetActive (true);
			BackgrBoat.SetActive (false);
			questionMark.SetActive (true);
			answer.enabled = false;
		}
		question.text = test;
		answer.text = ans;
		gameController.seedToPlayerController.answer.text = ans;
		//answer.enabled = false;
		ansBg.enabled = false;
		answerShow = false;

	}

	/* event callbacks */
	public void questionMarkClick() {
		if (answerShow) {
			answer.enabled = false;
			question.enabled = true;
			ansBg.enabled = false;
			answerShow = false;
		} else {
			question.enabled = false;
			answer.enabled = true;
			ansBg.enabled = true;
			answerShow = true;
		}
	}
}
	