using UnityEngine;
using System.Collections;


public enum DragIconType { hand, bulb };

public class UIDragIcon : MonoBehaviour {

	Vector2 pickupPos;
	bool picked;
	public bool autopilot;
	Vector2 autopilotTarget;
	int autopilotIndex;
	public Vector2 diff;
	public DragIconType iconType;

	public ValorationController_multi valorationController_N;

	public float scaleBoost;
	const float scaleBoostSpeed = 2.0f;
	const float maxScale = 1.3f;
	const float minScale = 1.0f;
	float initialScale;
	const float autopilotSpeed = 6.0f;
	const float maxSpeedRadius = 10.0f;
	const float threshold = 1.0f;

	bool active = true;

	bool finishPathLock = false;

	// Use this for initialization
	void Start () {
	
		picked = false;
		autopilot = false;
		initialScale = this.transform.localScale.x;
		active = true;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (picked) {
			Vector2 currentPos = Input.mousePosition;
			this.transform.position = currentPos;
			if (Input.GetMouseButtonUp (0)) {
				picked = false;
				if (!autopilot) {
					valorationController_N.setAward (iconType, ValorationController_multi.PlayerNone);
				}
				this.transform.localScale = new Vector3 (minScale*initialScale, minScale*initialScale, minScale*initialScale);
			}
		} // end of if(picked)


		else if (autopilot) {
			Vector2 currentPos = this.transform.position;
			diff = autopilotTarget - currentPos;
			float dist = diff.magnitude;
			diff.Normalize ();
			if (dist > maxSpeedRadius)
				diff *= autopilotSpeed;
			else {
				diff *= (autopilotSpeed) * (dist / maxSpeedRadius);
			}
			if (diff.magnitude < threshold) { // finish autopilot: get to destination
				this.transform.position = autopilotTarget;
				autopilot = false;
				if ((valorationController_N != null) && finishPathLock == false) {
					valorationController_N.setAward (iconType, autopilotIndex);
					finishPathLock = true;
				}
			} else {
				this.transform.position += new Vector3 (diff.x, diff.y);
				finishPathLock = false;
			}
				
		} // end of if(autopilot)

	}

	public void setActive(bool ac) {
		active = ac;
	}

	public void pickup() {

		if (active) {
			pickupPos = Input.mousePosition;
			picked = true;
			this.transform.localScale = new Vector3 (maxScale * initialScale, maxScale * initialScale, maxScale * initialScale);
		}

	}

	public void autopilotTo(Vector2 target, int targetIndex) {

		autopilot = true;
		autopilotIndex = targetIndex;
		autopilotTarget = target;

	}
}


