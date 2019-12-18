using UnityEngine;
using System.Collections;

public class UIFlash : MonoBehaviour {

	float scale;
	int direction;
	public float maxScale;
	public float speed;
	float initialScale;

	// Use this for initialization
	void Start () {
	
		scale = 0.0f;
		direction = 0;
		initialScale = this.transform.localScale.x;
		this.transform.localScale = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {

		if (direction == 0) {

			scale += speed * Time.deltaTime;
			if (scale > maxScale) {
				direction = 1;
			}

		} else if (direction == 1) {

			scale -= speed * Time.deltaTime;
			if (scale < 0.0f) {
				Destroy (this.gameObject);
				scale = 0.0f;
			}

		}

		this.transform.localScale = initialScale * scale * Vector3.one;
	
	}
}
