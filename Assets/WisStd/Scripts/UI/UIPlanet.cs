using UnityEngine;
using System.Collections;

public class UIPlanet : MonoBehaviour {

	public float radius;
	public float angularSpeed;
	public float initialPhase;

	public float speedMultiplier = 1.0f;

	float phase;

	public bool active = true;

	// Use this for initialization
	void Start () {
	
		phase = initialPhase;
		active = true;

	}
	
	// Update is called once per frame
	void Update () {

		if (!active)
			return;
		phase += angularSpeed * Time.deltaTime * speedMultiplier;
		this.transform.localPosition = new Vector2 (radius * Mathf.Cos (phase), radius * Mathf.Sin (phase));
	
	}
}
