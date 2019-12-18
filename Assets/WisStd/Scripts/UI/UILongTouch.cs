using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface LongTouchListener {

	void longTouch(int id);

}

public class UILongTouch : MonoBehaviour {

	public int id;

	public HeroClick heroClick;
	public MonoBehaviour longTouchController_multi_N;

	public float delay;
	public float remainingTime;

	bool isEnabled = true;

	bool going = false;

	public void go() {
		going = true;
	}

	public void reset() {
		remainingTime = delay;
		isEnabled = true;
		going = false;
	}

	public void setEnabled(bool en) {
		isEnabled = en;
	}

	// Use this for initialization
	void Start () {
		reset ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isEnabled)
			return;
		if (!going)
			return;
		remainingTime -= Time.deltaTime;
		if (remainingTime < 0.0f) {
			if (longTouchController_multi_N != null) {
				LongTouchListener listener = (LongTouchListener)longTouchController_multi_N;
				listener.longTouch (id);
				if(heroClick!=null) heroClick.setPenalty ();
			}
			going = false;
		}
	}
}

