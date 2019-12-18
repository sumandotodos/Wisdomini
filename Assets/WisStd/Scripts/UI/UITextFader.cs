using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UITextFader : Task {

	//public RawImage image;
	public float scale;
	public float targetScale;
	public float scaleSpeed = 1.0f;
	public float minScale = 0.0f;
	public float maxScale = 1.0f;

	Text thisText;

	public float startScale = 1.0f;

	private void setOpacity(float a) {
		Color c = thisText.color;
		c.a = a;
		thisText.color = c;
	}

	// Use this for initialization
	bool started = false;
	public void Start () {
		if (started)
			return;
		started = true;
		thisText = this.GetComponent<Text> ();
		scale = startScale;
		//this.transform.localScale = new Vector3 (scale, scale, scale);
		setOpacity(scale);

	}

	// Update is called once per frame
	void Update () {
		bool change = Utils.updateSoftVariable (ref scale, targetScale, scaleSpeed);
		if (change) {
			//this.transform.localScale = new Vector3 (scale, scale, scale);
			setOpacity(scale);
		}
		else {
			notifyFinishTask ();
		}
	}



	public void reset() {
		scale = targetScale = maxScale;
		setOpacity(scale);
	}

	public void fadeOut() {
		targetScale = minScale;
		//this.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
	}

	public void fadeIn() {
		targetScale = maxScale;
		//this.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
	}

	public void fadeOutTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		fadeOut ();
	}

	public float getOpacity() {
		return scale;
	}

	public void fadeInImmediately() {
		startScale = 1.0f;
		scale = targetScale = maxScale;
		setOpacity(scale);
	}

	public void fadeOutImmediately() {
		startScale = 0.0f;
		scale = targetScale = minScale;
		setOpacity(scale);
	}
}


