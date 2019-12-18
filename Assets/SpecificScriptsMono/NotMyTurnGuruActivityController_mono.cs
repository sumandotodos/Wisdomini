using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotMyTurnGuruActivityController_mono : Task {

	public GameController_mono gameController;
	public RosettaWrapper rosettaWrap;
	public StringBank type1Test;
	public StringBank type2Test;
	public StringBank type3Test;

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

		type1Test.rosetta = rosettaWrap.rosetta;
		type2Test.rosetta = rosettaWrap.rosetta;
		type3Test.rosetta = rosettaWrap.rosetta;

		question.enabled = true;
		answer.enabled = false;
		answerShow = false;

		type1Test.reset ();
		type2Test.reset ();
		type3Test.reset ();

		if (t == 0) {
			answerLabel.fadein ();
			gameController.seedToPlayerController.answer.text = type1Test.getString (q + 1);
			question.text = type1Test.getString (q);
			answer.text = type1Test.getString (q + 1);
			guru.SetActive (true);
			particles.SetActive (true);
			BackgrBoat.SetActive (false);
			questionMark.SetActive (true);
			answer.enabled = false;
		}
		if (t == 1) {
			meaningLabel.fadein ();
			gameController.seedToPlayerController.answer.text = type2Test.getString (q + 1);
			question.text = type2Test.getString (q);
			//answer.text = type2Test.getString (q + 1);
			guru.SetActive (false);
			particles.SetActive (false);
			BackgrBoat.SetActive (true);
			questionMark.SetActive (false);
			//answer.enabled = false;
		}
		if (t == 2) {
			missingLabel.fadein ();
			gameController.seedToPlayerController.answer.text = type3Test.getString (q + 1);
			question.text = type3Test.getString (q);
			answer.text = type3Test.getString (q + 1);
			guru.SetActive (true);
			particles.SetActive (true);
			BackgrBoat.SetActive (false);
			questionMark.SetActive (true);
			answer.enabled = false;
		}

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
