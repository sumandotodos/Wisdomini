using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIImageBlinker : MonoBehaviour {

	public BlinkStyle blinkStyle;
	public float speed;
	RawImage image = null;
	float time;
	bool direction;
	public UIImageBlinker[] stopOthers_N; // for mutual exclusion
	bool disabled = false;
	int state;

	float r, g, b;

	public void init() {
		time = 0.0f;
		state = 0;
		direction = false;
		image = this.GetComponent<RawImage> ();
		Color c = image.color;
		r = c.r;
		g = c.g;
		b = c.b;
	}

	// Use this for initialization
	void Start () {
		init ();
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
					f = 0.0f; 
					time = 0.0f;
					direction = !direction;
				}

				if (direction) { // going up
					image.color = new Color(r, g, b, f);
				} 

				else { // going down
					image.color = new Color(r, g, b, 1.0f - f);
				}

			}
			else {

				time += Time.deltaTime;
				if (time > (1.0f / speed)) {
					time = 0.0f;
					if (image.enabled == true)
						image.enabled = false;
					else
						image.enabled = true;
				}

			}

		}

	}

	public void startBlink() {
		image.enabled = true;
		state = 1;
		for (int i = 0; i < stopOthers_N.Length; ++i) {
			stopOthers_N [i].stopBlink ();
		}
	}

	public void stopBlink() {
		if (disabled) {
			image.enabled = false;
			return;
		}
		state = 0;
		direction = false;
		image.enabled = true;
		image.color = new Color (r, g, b, 1);
	}

	public void stopBlinkHidden() {
		if (image == null)
			init ();
		image.enabled = false;
	}

	public void disable() {
		disabled = true;
	}
}
