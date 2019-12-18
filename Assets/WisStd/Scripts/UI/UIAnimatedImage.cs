using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimatedImage : MonoBehaviour {

	public Texture[] image;
	RawImage theImage;
	int currentFrame;
	public bool autostart = true;
	int state = 0;
	float time;
	public float animationSpeed;
	public int frameOffset;

	void Start () 
	{	
		currentFrame = frameOffset % image.Length;
		theImage = this.GetComponent<RawImage> ();
		theImage.texture = image [0];
		time = 0.0f;
		state = 0;
		if (autostart)
			state = 1;
	}
	
	void Update () 
	{
		if (state == 0) 
		{

		}
		if (state == 1) 
		{
			time += Time.deltaTime;
			if (time > (1.0f / animationSpeed)) {
				currentFrame = (currentFrame + 1) % image.Length;
				theImage.texture = image [currentFrame];
				time = 0.0f;
			}
		}
	}
}
