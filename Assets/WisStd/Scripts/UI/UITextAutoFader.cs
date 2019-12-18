using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UITextAutoFader : Task {

	float opacity;
	float targetOpacity;
	public float opacitySpeed = 5.0f;
	Text img;
	float r, g, b;
	bool started = false;

	public void flash() {
		opacity = 1.0f;
	}

	// Use this for initialization
	public void Start () {
		if (started)
			return;
		if(!started) img = this.GetComponent<Text> ();
		opacity = targetOpacity = 0.0f;
		Color rgb = img.color;
		r = rgb.r;
		g = rgb.g;
		b = rgb.b;
		img.color = new Color (r, g, b, opacity);

		started = true;


	}

	// Update is called once per frame
	void Update () {

		bool change = Utils.updateSoftVariable (ref opacity, targetOpacity, opacitySpeed);
		if (change) {
			img.color = new Color (r, g, b, opacity);

		} else {
			notifyFinishTask ();
		}
	}

	public void reset() {
		opacity = targetOpacity = 0.0f;
		img.color = new Color (r, g, b, opacity);
	}

	public void fadein() {
		targetOpacity = 1.0f;
	}

	public void fadeout() {
		targetOpacity = 0.0f;
	}

	public void fadeinTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		fadein ();
	}

	public void fadeoutTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		fadeout();
	}
}
