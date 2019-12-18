using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MasterController : Task {

	public AudioClip changeLangAudioClip;

	public float screenWidth, screenHeight;
	public string startActivity;

	public GameObject waitThing;
	public static MasterController theInstance;
	public Toggle showHelpToggle;

	public Text DebugText_N;

	public abstract void enableWait ();
	public abstract void disableWait ();
	public abstract void playSound (AudioClip clip);
	public abstract void hardReset ();

	static int logEntries = 0;
	public void Log(string s) {
		if (DebugText_N != null) {
			DebugText_N.text += (s + "\n");
		}
	}
	public static void StaticLog(string s) {
		return;
		logEntries++;
		if (logEntries >= 240) {
			logEntries = 0;
			if (theInstance.DebugText_N != null) {
				theInstance.DebugText_N.text = "";
			}
		}
		theInstance.Log (s);
	}


}
