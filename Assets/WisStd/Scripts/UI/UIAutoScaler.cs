using UnityEngine;
using System.Collections;

public class UIAutoScaler : MonoBehaviour {

	public float initialScale;
	public float endScale;
	public float speed;

	float timer;

	Vector3 originalScale;

	float scale;
	float t;

	int state;

	// Use this for initialization
	void Start () {
	
		originalScale = this.transform.localScale;
		this.transform.localScale = originalScale * initialScale;
		state = 0;
		t = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) { // idling

		}
		if (state == 1) { // going
			t += Time.deltaTime * speed;
			if (t > 1.0f) {
				t = 1.0f;
				state = 0;
			}
			float scaleValue = Mathf.Lerp (initialScale, endScale, t);
			this.transform.localScale = scaleValue * originalScale;
		}
	}

	public void go() {
		state = 1;
	}

	public void reset() {
		state = 0;
		this.transform.localScale = originalScale;
	}
}
