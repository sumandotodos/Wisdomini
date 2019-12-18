using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	// this will have channels and stuff...

	AudioSource aSource;

	public void playSound(AudioClip c) {
		aSource.PlayOneShot (c);
	}

	// Use this for initialization
	void Start () {
	
		aSource = this.GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
