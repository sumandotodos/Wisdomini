using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIFaderScript : Task {

	float opacity;
	float targetOpacity;
	public Color fadeColor;
	public float fadeSpeed = 0.9f;
	RawImage riComponent;
	Image imageComponent;
	public bool autoFadeIn;
	public float initialOpacity;
	public float fadeInValue = 0.0f;
	public float fadeOutValue = 1.0f;

	bool started = false;

	// Use this for initialization
	public void Start () {
		if (started)
			return;
		started = true;
		riComponent = this.GetComponent<RawImage> ();
		imageComponent = this.GetComponent<Image> ();
		opacity = initialOpacity;
		targetOpacity = initialOpacity;
		if (riComponent != null) {
			riComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
		}
		if (imageComponent != null) {
			imageComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
		}
		if (initialOpacity > 0.0f) {
			if (riComponent != null) {
				riComponent.enabled = true;
			}
			if (imageComponent != null) {
				imageComponent.enabled = true;
			}
		} else {
			if (riComponent != null) {
				riComponent.enabled = false;
			}
			if (imageComponent != null) {
				imageComponent.enabled = false;
			}
		}
		if (autoFadeIn) {
			opacity = 1.0f;
			if (riComponent != null) {
				riComponent.enabled = true;
				riComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
			}
			if (imageComponent != null) {
				imageComponent.enabled = true;
				imageComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
			}
			fadeIn ();
		}
	}

	public float getOpacity() {
		return opacity;
	}

	public void setFadeValue(float nOp) {
		opacity = targetOpacity = nOp;
		if (riComponent != null) {
			riComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
		}
		if (imageComponent != null) {
			imageComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
		}
		if (nOp > 0.0f) {
			if (riComponent != null) {
				riComponent.enabled = true;
			}
			if (imageComponent != null) {
				imageComponent.enabled = true;
			}

		} else {
			if (riComponent != null) {
				riComponent.enabled = false;
			}
			if (imageComponent != null) {
				imageComponent.enabled = false;
			}

		}
		
	}



	public void fadeInTask(Task w) {
		waiter = w;
		waiter.isWaitingForTaskToComplete = true;
		fadeIn ();
	}

	public void fadeOutTask(Task w) {
		waiter = w;
		waiter.isWaitingForTaskToComplete = true;
		fadeOut ();
	}

	public void fadeIn() {
		targetOpacity = fadeInValue;
	}

	public void fadeOut() {
		if (riComponent != null) {
			riComponent.enabled = true;
		}
		if (imageComponent != null) {
			imageComponent.enabled = true;
		}
		targetOpacity = fadeOutValue;
	}
	
	// Update is called once per frame
	void Update () {
	
		bool change = Utils.updateSoftVariable (ref opacity, targetOpacity, fadeSpeed);
		if (change) {
			if (riComponent != null) {
				riComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
			}
			if (imageComponent != null) {
				imageComponent.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, opacity);
			}
			if (opacity == fadeInValue) {
				if (opacity == 0.0f) {
					if (riComponent != null) {
						riComponent.enabled = false;
					}
					if (imageComponent != null) {
						imageComponent.enabled = false;
					}
				}
				notifyFinishTask ();
			} else if (opacity == fadeOutValue) {
				notifyFinishTask ();
			}
		} 

	}

	public void setFadeInValue(float f) {
		fadeInValue = f;
	}
	public void setFadeOutValue(float f) {
		fadeOutValue = f;
	}
}

