using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiablilloMeditadorController : MonoBehaviour {

	public UIFaderScript meditador1;
	public UIFaderScript meditador2;
	public UIFaderScript diablillo1;
	public UIFaderScript diablillo2;

	public Texture[] meditadorTex;
	public Texture[] diablilloTex;

	public float offset;

	public float delay;

	public float elapsedTime1;
	public float elapsedTime2;

	int nextFrameMedit;
	int nextFrameDiabl;

	int currentFrameMedit;
	int currentFrameDiabl;

	bool turnMedit, turnDiabl;

	public bool going;

	bool started = false;

	// Use this for initialization
	public void Start () {
		
		elapsedTime1 = 0.0f;
		elapsedTime2 = 0.0f;
		turnMedit = true;
		turnDiabl = true;
		going = true;

		currentFrameDiabl = Random.Range (0, diablilloTex.Length);
		currentFrameMedit = Random.Range (0, meditadorTex.Length);


		meditador1.gameObject.GetComponent<RawImage> ().texture = meditadorTex [currentFrameMedit];
		diablillo1.gameObject.GetComponent<RawImage> ().texture = diablilloTex [currentFrameDiabl];

		meditador1.Start ();
		meditador2.Start ();
		diablillo1.Start ();
		diablillo2.Start ();

		meditador1.setFadeValue (1.0f);
		diablillo1.setFadeValue (1.0f);
		meditador2.setFadeValue (0.0f);
		diablillo2.setFadeValue (0.0f);


	}

	public void go() {
		Start ();

	}

	public void stop() {
		going = false;
	}

	public void changeMeditador() {
		elapsedTime1 = 2 * delay;
	}

	public void changeDiablillo() {
		elapsedTime2 = 2 * delay;
	}
	
	// Update is called once per frame
	void Update () {

		if (!going)
			return;

		//elapsedTime1 += Time.deltaTime;
		//elapsedTime2 += Time.deltaTime;

		if (elapsedTime1 > delay) {
			nextFrameMedit = Random.Range (0, meditadorTex.Length);
			while (nextFrameMedit == currentFrameMedit) {
				nextFrameMedit = Random.Range (0, meditadorTex.Length);
			}
			currentFrameMedit = nextFrameMedit;
			if (turnMedit) {
				meditador2.gameObject.GetComponent<RawImage> ().texture = meditadorTex [nextFrameMedit];
				meditador1.fadeIn ();
				meditador2.fadeOut ();
				turnMedit = false;
			} else {
				meditador1.gameObject.GetComponent<RawImage> ().texture = meditadorTex [nextFrameMedit];
				meditador1.fadeOut ();
				meditador2.fadeIn ();
				turnMedit = true;
			}
			elapsedTime1 = 0.0f;
		}

		// sabemos perfectamente que esta repetición de código es evitable, pero no me da la gana
		// construir los componentes que permiten factorizarlo a una forma más elegante
		// no va a ser más legible, ni va a funcionar mejor...
		if (elapsedTime2 > delay) {
			nextFrameDiabl = Random.Range (0, diablilloTex.Length);
			while (nextFrameDiabl == currentFrameDiabl) {
				nextFrameDiabl = Random.Range (0, diablilloTex.Length);
			}
			currentFrameDiabl = nextFrameDiabl;
			if (turnDiabl) {
				diablillo2.gameObject.GetComponent<RawImage> ().texture = diablilloTex [nextFrameDiabl];
				diablillo1.fadeIn ();
				diablillo2.fadeOut ();
				turnDiabl = false;
			} else {
				diablillo1.gameObject.GetComponent<RawImage> ().texture = diablilloTex [nextFrameDiabl];
				diablillo1.fadeOut ();
				diablillo2.fadeIn ();
				turnDiabl = true;
			}
			elapsedTime2 = 0.0f;
		}

	}
}

