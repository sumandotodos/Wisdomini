using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GalleryController : Task {

	public FGTable labelsTable;
	public RawImage ri;
	public string fullImageBasePath;
	public AdjustSize frameAdjust;
	public Text labelText;
	public UITextFader labelFader;
	public UIFaderScript fader;


	bool started = false;
	public bool showing = false;
	// Use this for initialization
	public void Start () {
		if (started)
			return;
		started = true;
		show ();
	}

	public void show() {
		if (showing)
			return;
		showing = true;
		labelFader.Start ();
		labelText.text = "";
		fader.Start ();
		fader.setFadeValue (1f);
		fader.fadeIn ();
		indexAtFront = Random.Range(0, labelsTable.nRows());
		refreshImage ();
	}
	

	void updateLabel () {
		state = 1;
		labelFader.fadeOutTask (this);
	}

	int state = 0;
	void Update() {
		if(state == 0) { // idle
		}
		if (state == 1) { // waiting for text to fadeout
			if (!isWaitingForTaskToComplete) {
				state = 0;
				labelText.text = (string)labelsTable.getElement (0, showingIndex);
				labelFader.fadeIn ();
			}
		}
	}

	int indexAtFront = 0;

	int showingIndex=-1;
	private void refreshImage() {



		if (indexAtFront != showingIndex) {
			string DaPath = fullImageBasePath + indexAtFront;
			Texture newSpr = Resources.Load<Texture> (DaPath);
			if (ri.texture != null) {
				Resources.UnloadAsset (ri.texture);
				Resources.UnloadUnusedAssets ();
				System.GC.Collect ();
			}
			showingIndex = indexAtFront;
			updateLabel ();

			//lienzo.sprite = newSpr;
			ri.texture = newSpr;

			frameAdjust.imageIndex = indexAtFront;
			frameAdjust.updateSize ();



		}
			
	}


	public void touch() {
	
		int newimageindex = Random.Range(0, labelsTable.nRows());
		while (newimageindex == showingIndex) {
			newimageindex = Random.Range(0, labelsTable.nRows());
		}
		indexAtFront = newimageindex;
		refreshImage ();

	}

	public void resetGallery() {
		showing = false;
	}

}

