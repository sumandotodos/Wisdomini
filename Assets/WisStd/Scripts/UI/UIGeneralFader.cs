using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIGeneralFader : MonoBehaviour {

	UIFaderScript[] imageFaders;
	UITextFader[] textFaders;

	// Use this for initialization
	bool started = false;
	public void Start () {
		if (started)
			return;
		started = true;
		imageFaders = this.GetComponentsInChildren<UIFaderScript> ();
		textFaders = this.GetComponentsInChildren<UITextFader> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void fadeToOpaque() {
		foreach (UIFaderScript f in imageFaders) {
			f.Start ();
			f.fadeOut ();
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeIn ();
		}
	}

	public void fadeToTransparent() {
		foreach (UIFaderScript f in imageFaders) {
			f.Start ();
			f.fadeIn ();
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeOut ();
		}
	}

	public void fadeOutImmediately() {
		foreach (UIFaderScript f in imageFaders) {
			f.Start ();
			f.setFadeValue (0f);
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeOutImmediately();
		}
	}
}

