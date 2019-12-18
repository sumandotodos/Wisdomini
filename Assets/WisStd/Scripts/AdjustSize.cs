using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdjustSize : MonoBehaviour {

	public int imageIndex;

	public FGTable originalSizes;

	public float desiredMaxDimension;

	bool started = false;
	void Start() {
		if (started)
			return;
		started = true;

	}

	public void updateSize() {
		Start ();
		float w = (float)((int)originalSizes.getElement (0, imageIndex)) ;//sr.sprite.rect.width;
		float h = (float)((int)originalSizes.getElement(1, imageIndex)) ;//sr.sprite.rect.height;

		if (w > h) { // horizontal
			this.transform.localScale = new Vector3(desiredMaxDimension, h/w * desiredMaxDimension, desiredMaxDimension);
		} else { // vertical
			this.transform.localScale = new Vector3(w/h * desiredMaxDimension, desiredMaxDimension, desiredMaxDimension);
		}
	}
}

