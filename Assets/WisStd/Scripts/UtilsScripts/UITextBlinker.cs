using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum BlinkStyle { Blink, XFade };

public class UITextBlinker : MonoBehaviour {

	public BlinkStyle blinkStyle;
	public float speed;
	Text text;
	float time;
	bool direction;
	public UITextBlinker[] stopOthers_N; // for mutual exclusion
	bool disabled = false;
	int state;

	float r, g, b;

	bool started = false;
	// Use this for initialization
	void Start () {
		if(started) return;
		started = true;
		time = 0.0f;
		state = 0;
		direction = false;
		text = this.GetComponent<Text> ();
		Color c = text.color;
		r = c.r;
		g = c.g;
		b = c.b;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idle

		} 


		if (state == 1) { // going
			
			if (blinkStyle == BlinkStyle.XFade) {
				time += Time.deltaTime;
				float f = time / (1.0f / speed);
				if (f > 1.0f) {
					f = 1.0f; 

					if (direction) { // going up
						text.color = new Color(r, g, b, f);
					} 

					else { // going down
						text.color = new Color(r, g, b, 1.0f - f);
					}
					direction = !direction;
				}
			} else {

				time += Time.deltaTime;
				if (time > (1.0f / speed)) {
					time = 0.0f;
					if (text.enabled == true)
						text.enabled = false;
					else
						text.enabled = true;
				}

			}

		}

	}

	public void startBlink() {
		state = 1;
		for (int i = 0; i < stopOthers_N.Length; ++i) {
			stopOthers_N [i].stopBlink ();
		}
	}

	public void stopBlink() {
		Start();
		/*if (disabled) {
			text.enabled = false;
			return;
		}*/
		state = 0;
		direction = false;
		text.enabled = true;
		text.color = new Color (r, g, b, 1);
	}

	public void disable() {
		disabled = true;
	}
}
