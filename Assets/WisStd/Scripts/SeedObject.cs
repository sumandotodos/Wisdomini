using UnityEngine;
using System.Collections;

public class SeedObject : MonoBehaviour {

	public GameObject flashPrefab;
	public GameObject seedPrefab;
	int nSeeds;

	float globalAngle;
	public float radius;
	public float maxRadius;
	const float radiusSpeed = 180.0f;
	public float angleSpeed = 1.33f;

	GameObject[] goPositions;
	GameObject[] go;

	int state0 = 0;
	int state1 = 0; // two slots, please

	int dissolvedSeed = -1;
	float dissolveScale = 1.0f;
	float globalScale;

	public void init(int ns) {
		Debug.Log ("SeedObject.init");
		nSeeds = ns;
		dissolvedSeed = -1;
		radius = 0.0f;
		maxRadius = 80.0f;

		goPositions = new GameObject[nSeeds];
		go = new GameObject[nSeeds]; // create array of seeds
		//CircpleDeploy[] seedInCircle;

		for(int i = 0 ; i<nSeeds; ++i) {
			goPositions [i] = new GameObject ();
			goPositions [i].transform.parent = this.transform;
			goPositions [i].transform.localScale = Vector3.one;
			go [i] = Instantiate (seedPrefab);
			go [i].transform.parent = goPositions[i].transform;
			go [i].transform.localPosition = Vector3.zero;
			globalScale = 2.0f;
			go [i].transform.localScale = new Vector3 (globalScale, globalScale, globalScale);
		}



		state0 = 1;
		state1 = 1;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (state0 == 0) {

		}

		if (state0 == 1) {
			for(int i = 0 ; i<nSeeds; ++i) {
				
				goPositions [i].transform.localPosition = radius * 
					new Vector3(Mathf.Cos (globalAngle + ((float)i) / ((float)nSeeds) * 2.0f * Mathf.PI),
						Mathf.Sin (globalAngle + ((float)i) / ((float)nSeeds) * 2.0f * Mathf.PI),
						0);

				if (i == dissolvedSeed) {
					
					go [i].transform.localScale = new Vector3 (globalScale * dissolveScale, 
						globalScale * dissolveScale, globalScale * dissolveScale);

				}

				if (i < dissolvedSeed) {
					
					go [i].transform.localScale = Vector3.zero;

				}

				if (dissolveScale > 0.05f)
					dissolveScale = dissolveScale * 0.98f;
				else
					dissolveScale = 0.0f;

			}

			globalAngle += angleSpeed * Time.deltaTime;
		}

		// end of slot 0




		if (state1 == 0) {

		}

		if (state1 == 1) {
		
			radius += radiusSpeed * Time.deltaTime;
			if (radius > maxRadius) {
				radius = maxRadius;
				state1 = 2;
			}

		}

		// end of slot 1

	}

	public void dissolveSeed() {


		if (dissolvedSeed == nSeeds - 1)
			return;
		
		dissolveScale = 1.0f;
		++dissolvedSeed;

		// spawn a flash parented to the dissapearing seed
		Transform t = goPositions [dissolvedSeed].transform;
		GameObject flashGO = (GameObject)Instantiate (flashPrefab, t.position, Quaternion.Euler (0, 0, 0));
		//flashGO.GetComponent<UIFlash> ().maxScale = 30.0f;
		flashGO.transform.parent = t;

	}

	public void clearSeeds() {
		dissolveScale = 1.0f;
		dissolvedSeed = nSeeds;
	}

	// clean up the mess!
	public void destroy() {

		for (int i = 0; i < nSeeds; ++i) {
			Destroy (goPositions [i]);
			Destroy (go [i]);
		}
		state0 = 0; // reset states to idling!
		state1 = 0; 

	}
}
