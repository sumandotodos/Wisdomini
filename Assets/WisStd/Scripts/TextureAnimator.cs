using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureAnimator : MonoBehaviour {

	public Texture[] images;
	public float animSpeed;
	public bool active = true;
	float time;
	int frame;

	RawImage theImage;

	// Use this for initialization
	void Start () {

		frame = 0;
		theImage = this.GetComponent<RawImage> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (active) {
			time += Time.deltaTime;
			if (time > (1.0f / animSpeed)) {
				time = 0.0f;
				if (images.Length > 0) {
					frame = (frame + 1) % images.Length;
					theImage.texture = images [frame];
				}
			}
		}

	}
}
