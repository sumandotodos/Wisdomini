using UnityEngine;
using System.Collections;


public class UIMagnet : MonoBehaviour {

	public float radius;
	public GameObject[] magnets;
	UIDragIcon drag;
	int affectingMagnet;

	// Use this for initialization
	void Start () {
	
		radius = radius * (Screen.width / 300.0f);
		drag = this.GetComponent<UIDragIcon> ();
		affectingMagnet = -1;

	}
	
	// Update is called once per frame
	void Update () {

		int i;
		float minDistance = 100000.0f;
		int closestMagnet = 0;
		// check if a magnet is affecting this object
		for (i = 0; i < magnets.Length; ++i) {
			float distToMagnet = (magnets [i].transform.position - this.transform.position).magnitude;
			if (distToMagnet < minDistance) {
				closestMagnet = i;
				minDistance = distToMagnet;
			}
		}

		//if (!drag.autopilot) {
		if (minDistance < radius) {
			drag.autopilotTo (magnets [closestMagnet].transform.position, closestMagnet);
		} else {
			drag.autopilot = false;
		}
		//}

		/*
		if ((distToMagnet < radius) && (affectingMagnet != i)) {
			drag.autopilotTo (magnets [i].transform.position);
			affectingMagnet = i;
			break;
		}
		if(i==magnets.Length) affectingMagnet = -1;
		*/
	
	}
}

