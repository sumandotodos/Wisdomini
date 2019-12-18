using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UITextFadeDelay : MonoBehaviour {

	public UITextAutoFader autoFader;
	public float delay;
	public float remainingTime;

	public bool autoStart;
	public bool fadeIn;

	public bool going = false;

	public void reset() {
		remainingTime = delay;
		going = autoStart;
	}

	public void go() {
		going = true;
	}

	// Use this for initialization
	void Start () {
		reset ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (!going)
			return;
		if (remainingTime > 0.0f) {
			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f) {
				if (fadeIn) {
					autoFader.fadein ();
				} else {
					autoFader.fadeout ();
				}
			}
		}
	}
}

