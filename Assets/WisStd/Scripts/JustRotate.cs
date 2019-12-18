using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotate : MonoBehaviour {

	public float speed = 40.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (new Vector3 (0, 0, speed * Time.deltaTime));
	}
}
