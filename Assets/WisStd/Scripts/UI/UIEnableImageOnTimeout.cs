using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnableImageOnTimeout : MonoBehaviour {

	public float timeout;
	public float remainingTime;

	RawImage theImage = null;
	Text text = null;
	public RawImage cubreTodo;

	public bool going = false;
	//float time;

	public void Start () 
	{
		remainingTime = timeout;
		theImage = this.GetComponent<RawImage> ();
		theImage.enabled = false;
		cubreTodo.enabled = false;
		going = false;
		text = this.GetComponentInChildren<Text> ();
		//text.enabled = false;
		text.enabled = false;
		//text.gameObject.SetActive (false);
	}
	
	void Update ()
	{
		if (!going)
			return;

		if (remainingTime > 0.0f) {

			remainingTime -= Time.deltaTime;
			if (remainingTime <= 0.0f)
			{
				theImage.enabled = true;
				cubreTodo.enabled = true;
				text.enabled = true;
				//text.gameObject.SetActive (true);
				text.GetComponent<UITextBlinker> ().startBlink ();
			}
		}
	}

	public void go() {
		going = true;
		remainingTime = timeout;
	}

	public void stop() {
		going = false;
		keepAlive ();
	}

	public void keepAlive() {
		if (theImage != null) {
			theImage.enabled = false;
		}
		if (text != null) {
			text.GetComponent<UITextBlinker> ().disable ();
			text.GetComponent<UITextBlinker> ().stopBlink ();
			text.enabled = false;
			//text.gameObject.SetActive (false);
		}
		cubreTodo.enabled = false;
		text.enabled = false;
		remainingTime = timeout;
	}
}
