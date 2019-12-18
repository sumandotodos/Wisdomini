using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MasterControllerType { multi, mono, multikids, monokids };

public enum DismissType {  dismissForGame, doNotDismiss };

public class UITip : MonoBehaviour {

	public bool autostart = true;
	public UITip nextTip;

	public MasterControllerType mcType = MasterControllerType.multi;

	public DismissType dismissType = DismissType.doNotDismiss;

	public string tipID;

	GameController_multi gameController;

	UIGeneralFader generalFader;

	bool visible;
	bool waitingToVanish;

	public IEnumerator launchCoRo() {
		yield return new WaitForSeconds (0.5f);
		generalFader.fadeToOpaque ();
		this.GetComponentInChildren<Image> ().raycastTarget = true;
		yield return new WaitForSeconds (0.5f);
		visible = true;
	}

	int state = -1;
	// Use this for initialization

	void OnEnable() {
		Start ();
		if(autostart) show ();
	}

	bool started = false;
	public void Start () {

		if (started)
			return;
		started = true;

		generalFader = this.GetComponent<UIGeneralFader> ();

		this.GetComponentInChildren<Text> ().raycastTarget = false;
		this.GetComponentInChildren<Image> ().raycastTarget = false;

		state = -1;
		visible = false;
		waitingToVanish = false;

		if (autostart) {
			
				show ();	

		}
	}



	public void show() {
		bool mustShow = false;
		switch (mcType) {
		case MasterControllerType.multi:
			mustShow = MasterController_multi.mustShowHelp ();
			break;
		case MasterControllerType.mono:
			mustShow = MasterController_mono.mustShowHelp ();
			break;
		case MasterControllerType.multikids:
			mustShow = MasterController_kids.mustShowHelp ();
			break;
		case MasterControllerType.monokids:
			mustShow = MasterController_kidsmono.mustShowHelp ();
			break;
		}
		if (mustShow) {
			
			//if (!GameObject.Find ("GameController_multi").GetComponent<GameController_multi> ().tipSaveData.dismissedTips.Contains (tipID)) {
			StartCoroutine ("launchCoRo");

			//}

		}
	}

	// Update is called once per frame
	void Update () {
		if (state == -1) {
			generalFader.fadeOutImmediately ();
			state = 0;
		}

	}

	public void dismissTip() {
		if (visible) {
			visible = false;
			if (nextTip) {
				nextTip.show ();
			}
			generalFader.fadeToTransparent ();
			//this.GetComponentInChildren<Image> ().raycastTarget = false;
			// never dismiss forever
//			if (dismissType == DismissType.dismissForGame) {
//				if (!tipID.Equals ("")) {
//					GameObject.Find ("GameController_multi").GetComponent<GameController_multi> ().tipSaveData.dismissedTips.Add (tipID);
//				}
//			}
		}
	}
}

