using UnityEngine;
using System.Collections;

public class UIButtonPress : MonoBehaviour {


	public MonoBehaviour buttonPressListener_N;
	public AudioClip sound_N;

	public float maxScale = 1.0f;
	public float minScale = 0.8f;
	float scale;

	public bool enabled = true;

	public bool execute = true;

	// Use this for initialization
	void Start () {
		scale = 1.0f;
		this.transform.localScale = new Vector3 (maxScale, maxScale, maxScale);
	}

	public void onPress() {
		if (!enabled)
			return;
		this.transform.localScale = new Vector3 (minScale, minScale, minScale);
		if (execute) {
			if (buttonPressListener_N != null) {
				ButtonPressListener bl = (ButtonPressListener)buttonPressListener_N;
				bl.buttonPress ();
			}
		}
		if (sound_N != null) {
			//controllerHub.audioController.playSound (sound_N);
		}
	}

	public void onRelease() {
		if (!enabled)
			return;
		if (execute) {
			if (buttonPressListener_N != null) {
				ButtonPressListener bl = (ButtonPressListener)buttonPressListener_N;
				//bl.buttonPress ();
			}
		}
		this.transform.localScale = new Vector3 (maxScale, maxScale, maxScale);
	}


}
