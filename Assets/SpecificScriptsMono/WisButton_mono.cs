using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class WisButton_mono : MonoBehaviour {

	public int buttonId;
	public PlayerActivityController_mono playerActivityController;
	public MasterController masterController;

	public Texture deactivatedImage;
	public Texture activatedImage;
	public AudioClip sound;
	RawImage image;

	bool activated;

	public void clickCallback() {

		playerActivityController.setButtonPressed (buttonId);


		masterController.playSound (sound);

		activated = !activated;
		if (activated) {
			image.texture = activatedImage;
		} else {
			image.texture = deactivatedImage;
		}

	}

	// Use this for initialization
	void Start () {
	
		activated = false;
		image = this.GetComponent<RawImage> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate() {
		activated = true;
		if (image != null) {
			image.texture = activatedImage;
		}
	}

	public void deactivate() {
		activated = false;
		if (image != null) {
			image.texture = deactivatedImage;
		}
	}

	public bool isActivated() {
		return activated;
	}
}


