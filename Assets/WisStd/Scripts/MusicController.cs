using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicController : MonoBehaviour {

	public static MusicController theInstance;
	public AudioClip[] track;
	float volume;
	float targetVolume;
	public float maxVolume = 1.0f;

	AudioSource aSource;

	// Use this for initialization
	void Start () {
		theInstance = this;
		aSource = this.GetComponent<AudioSource> ();
		volume = 0;
		targetVolume = 0;
		aSource.volume = 0;
	}

	public static void fadeIn() {
		theInstance.targetVolume = theInstance.maxVolume;
	}

	public static void fadeOut() {
		theInstance.targetVolume = 0.0f;
	}

	public static void playTrack(int n) {
		theInstance.aSource.clip = theInstance.track [n];
		theInstance.aSource.loop = true;
		theInstance.aSource.Play ();
	}

	public static void playTrack(string s) {
		for (int i = 0; i < theInstance.track.Length; ++i) {
			if (theInstance.track [i].name == s) {
				playTrack (i);
				return;
			}
		}
	}

	public static void stop() {
		theInstance.aSource.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Utils.updateSoftVariable (ref volume, targetVolume, 0.5f)) {
			aSource.volume = volume;
		}
	}
}

