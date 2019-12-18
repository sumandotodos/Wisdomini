using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuruIngest : MonoBehaviour {

	[HideInInspector]
	public bool fileLoaded = false;

	[HideInInspector]
	public string fileContents;

	public string TypeOfTest;

	public string outputFolder;

	public void loadFile(string contents) {
		fileContents = contents;
		fileLoaded = true;
	}



}
