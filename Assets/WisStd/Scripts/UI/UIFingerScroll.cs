using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFingerScroll : MonoBehaviour {

	public Text text;

	public MonoBehaviour scrollCallbackScript;
	public string scrollCallbackMethod;

	// up is positive, down is negative,
	// as the text 'scrolls up', you reach more negative coordinates
	public float prevY;
	public float prevPrevY;
	public float touchY;
	public float scroll;
	public float Y;
	public float initialLocalY;
	bool firstTimeInit = true;

	bool isEnabled = true;

	public float textFieldHeight = 560.0f;

	const float LogicalHeight = 1280;

	public float charactersPerLine = 1.0f;
	public float characterHeight = 1.0f;

	public float numberOfLines;
	public float maxHeight;

	public float interfaceMinY = 400.0f;

	public float maxScroll;

	float ratio;

	public float speed, accel;


	public float force;

	public float dampingFactor = 0.99f;

	const float speedCutOff = 0.05f;

	public float YDiff;

	float timer;

	bool touching;

	const float SampleTime = 0.2f;

	public void setEnabled(bool en) {
		isEnabled = en;
	}

	public void init() {

		timer = 0.0f;

		if (firstTimeInit) {
			initialLocalY = this.transform.localPosition.y;
			firstTimeInit = false;
		} else {
			Vector3 pos = this.transform.localPosition;
			pos.y = initialLocalY;
			this.transform.localPosition = pos;
		}
		scroll = 0.0f;
		Y = 0.0f;
		this.transform.localPosition = new Vector2 (0, initialLocalY + Y);

		ratio = LogicalHeight / Screen.height;

		text = this.GetComponent<Text> ();

		numberOfLines = text.text.Length / charactersPerLine;
		maxHeight = numberOfLines * characterHeight;

		maxScroll = maxHeight - textFieldHeight;
		if (maxScroll < 0.0f)
			maxScroll = 0.0f;

		touching = false;

	}

	// Use this for initialization
	void Start () {

		init ();

	}
	
	// Update is called once per frame
	void Update () {

		if (!isEnabled)
			return;

		if (Input.GetMouseButtonDown (0)) {
	
			if(Input.mousePosition.y > interfaceMinY * ratio) {
				touchY = Input.mousePosition.y;// - scroll;
				touching = true;
			}

		}
		if (touching == true) { // while we are touching

			float diff = (Input.mousePosition.y - touchY) * ratio;
			Y = scroll + diff;
			if (Y < 0.0f)
				Y = -Mathf.Sqrt (-Y);
			if (Y > maxScroll) {
				Y = maxScroll + Mathf.Sqrt (Y - maxScroll);
				if (scrollCallbackScript != null) {
					scrollCallbackScript.Invoke (scrollCallbackMethod, 0.0f);
				}
			}
			this.transform.localPosition = new Vector2 (0, initialLocalY + Y);

		}
		if (touching && Input.GetMouseButtonUp (0)) {


			float diff = ((Input.mousePosition.y - prevY)*ratio);
			YDiff = diff;
			speed = diff;
			scroll = Y;
			touching = false;
			//scroll += (Input.mousePosition.y - touchY)*ratio;
			/*if (scroll < 0.0f)
				scroll = 0.0f;
			if (scroll > maxScroll)
				scroll = maxScroll;
				*/

		}

		timer += Time.deltaTime;
		if (timer > SampleTime) {
			timer = 0.0f;
			prevY = Input.mousePosition.y;
		}


		/* dynamics */


		if (!Input.GetMouseButton (0)) {
			scroll += speed * Time.deltaTime;
			speed += accel * Time.deltaTime;

			if (Mathf.Abs(speed) < speedCutOff)
				speed = 0.0f;
			if (scroll > maxScroll) {
				accel = -force * (scroll - maxScroll);
			} else if (scroll < 0.0f) {
				accel = force * (-scroll);
			} else
				accel = 0.0f;
			this.transform.localPosition = new Vector2 (0, initialLocalY + scroll);

			speed = speed * dampingFactor;

		}


	}
}
