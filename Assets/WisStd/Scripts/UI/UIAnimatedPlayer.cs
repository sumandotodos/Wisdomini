using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIAnimatedPlayer : MonoBehaviour {

	public Sprite[] walking;
	public Sprite standing;

	public int playerNumber;
	public FinishActivityController_multi finishActivityController_N;

	Image image;
	int frame;
	public float animationSpeed;
	float time;
	Vector3 originalScale = new Vector3(0.8f, 0.8f, 0.8f); // this is a little big ugly

	float x, h;
	float initialX;
	float hSpeed;
	float maxH;
	float playerH;
	float deltaX;

	public bool isStanding;

	int state = 0;

	public void Start ()
	{	
		time = 0.0f;
		frame = 0;
		image = this.GetComponent<Image> ();
		state = 0;
		//originalScale = this.transform.localScale;
	}

	public void autopilotWithParams(float X, float dX, float H, float HSpeed, float MaxH, float PlayerH) {
		x = initialX = X;
		deltaX = dX;
		h = H;
		hSpeed = HSpeed * (Screen.width / 195.0f); // WARNING 'magic' factor
		maxH = MaxH;
		playerH = PlayerH;
		state = 1;
	}
	
	void Update () 
	{	
		if (!isStanding) {
			time += Time.deltaTime;
			if (time > (1.0f / animationSpeed)) {
				time = 0.0f;
				frame = (frame + 1) % walking.Length;
				image.sprite = walking [frame];
			}
		} else {
			image.sprite = standing;
		}

		if (state == 0) { // idling

		} 

		else if (state == 1) {
			x = initialX + deltaX - (12-6*h/maxH)*Mathf.Cos (h / maxH * 3.1416f);
			h += hSpeed * Time.deltaTime;
			this.transform.position = new Vector3 (x, h, 0);

			float antiscale = 0.5f * h / maxH;

			float scale = 1.0f - antiscale;

			this.transform.localScale = originalScale * scale;
			if (h > playerH) {
				state = 2;

				this.isStanding = true;
			}
		} 	

		else if (state == 2) 
		{

		}

	}

	/* event callback */
	public void clickOnPlayer() 
	{
		if (finishActivityController_N != null) {
			finishActivityController_N.clickOnPlayer (playerNumber);
		}
	}
}
	