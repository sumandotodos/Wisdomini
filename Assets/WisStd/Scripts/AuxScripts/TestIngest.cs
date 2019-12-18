using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestIngest : MonoBehaviour {

	[HideInInspector]
	public bool fileLoaded = false;
	public bool outputTempNodes = true;

	[HideInInspector]
	public string fileContents;

	public string outputFolder;

	public void loadFile(string contents) {
		fileContents = contents;
		fileLoaded = true;
	}



}
