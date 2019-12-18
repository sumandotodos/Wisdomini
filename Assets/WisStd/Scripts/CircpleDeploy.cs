﻿using UnityEngine;
using System.Collections;

public class CircpleDeploy : Task {

	public float nElements = 7;
	public int elementIndex = 0;

	public float maxRadius = 60.0f;
	public float speed = 4.0f;

	public float yScale = 1.0f;

	Vector2 originalPosition;
	Vector2 displacement;

	public ClickEventReceiver clickEventReceiver_N;

	float radius;
	public float angle;

	int state;

	float initialScale;

	bool started = false;

	// Use this for initialization
	public void Start () {
	
		if (started)
			return;
		started = true;
		initialScale = this.transform.localScale.x;
		if (initialScale == 0.0f)
			initialScale = 1.0f;
		originalPosition = this.transform.localPosition;
		reset ();

	}

	public void setNElements(int n) {
		nElements = n;
		angle = (2.0f * (float)Mathf.PI) * ((float)elementIndex / (float)nElements);
	}

	public void setIndex(int i) {
		elementIndex = i;
		angle = (2.0f * (float)Mathf.PI) * ((float)elementIndex / (float)nElements);
	}

	public void reset() {
		radius = 0.0f;

		angle = (2.0f * (float)Mathf.PI) * ((float)elementIndex / (float)nElements);
		this.transform.localScale = new Vector3 (initialScale*(radius / maxRadius), 
			initialScale*(radius / maxRadius), 
			initialScale*(radius / maxRadius));
	}

	// Update is called once per frame
	void Update () {
	
		if (state == 0) { // idle

		}

		if (state == 1) { // extending


			radius += speed * Time.deltaTime;
			if (radius > maxRadius) {
				radius = maxRadius;
				notifyFinishTask ();
				state = 2;
			}
			if (nElements > 1) {
				displacement.x = radius * Mathf.Cos (angle);
				displacement.y = radius * Mathf.Sin (angle) * yScale;
			} else {
				displacement = Vector2.zero;
			}
			this.transform.localPosition = originalPosition + displacement;
			this.transform.localScale = new Vector3 (initialScale*(radius / maxRadius), 
				initialScale*(radius / maxRadius), 
				initialScale*(radius / maxRadius));

		}

		if (state == 2) { // extended

		}

		if (state == 3) { // retracting


			radius -= speed * Time.deltaTime;
			if (radius < 0.0f) {
				radius = 0.0f;
				state = 0;
				notifyFinishTask ();
			}
			if (nElements > 1) {
				displacement.x = radius * Mathf.Cos (angle);
				displacement.y = radius * Mathf.Sin (angle) * yScale;
			} else {
				displacement = Vector2.zero;
			}
			this.transform.localPosition = originalPosition + displacement;
			this.transform.localScale = new Vector3 (initialScale*(radius / maxRadius), 
				initialScale*(radius / maxRadius), 
				initialScale*(radius / maxRadius));

		}

	}

	public void extendTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		extend ();
	}

	public void extend() {

		state = 1;

	}

	public void retractTask(Task w) {
		w.isWaitingForTaskToComplete = true;
		waiter = w;
		retract ();
	}

	public void retract() {

		state = 3;

	}

	// click event callback
	public void clickCallback() {
		if (clickEventReceiver_N != null) {
			clickEventReceiver_N.clickEvent (elementIndex);
		}
	}


}
