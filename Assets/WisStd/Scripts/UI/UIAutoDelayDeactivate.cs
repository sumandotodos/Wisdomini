using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAutoDelayDeactivate : MonoBehaviour {

	public float delay = 2.5f;

	float remaining;

	// Use this for initialization
	void Start () {
		remaining = delay;
	}
	
	// Update is called once per frame
	void Update () {
		if (remaining > 0.0f) {
			remaining -= Time.deltaTime;
			if (remaining <= 0.0f) {
				remaining = delay;
				this.gameObject.SetActive (false);
			}
		}
	}
}
