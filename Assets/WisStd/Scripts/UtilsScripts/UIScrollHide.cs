using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIScrollHide : MonoBehaviour {

	public Vector2 initial;
	public Vector2 final;
	public float speed;

	RawImage imgComponent;

	float t;

	float targetT;

	// Use this for initialization
	void Start () {
	
		reset ();

	}
	
	// Update is called once per frame
	void Update () {
	
		bool change = Utils.updateSoftVariable (ref t, targetT, speed);
		if (change) {
			this.transform.localPosition = Vector2.Lerp (initial, final, t);
		} else if (t == 0.0f) {
			imgComponent.enabled = false; // hide to save time!
		}
	}

	public void show() {

		targetT = 1.0f;
		imgComponent.enabled = true;

	}

	public void hide() {

		targetT = 0.0f;

	}

	public void reset() {

		targetT = t = 0;

		imgComponent = this.GetComponent<RawImage> ();
		imgComponent.enabled = false; 
		this.transform.localPosition = initial;

	}
}

