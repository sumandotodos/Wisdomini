using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroAmount : MonoBehaviour {

	public Text theText;

	// Use this for initialization
	void Start () {
		theText = this.gameObject.GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
