using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSON2StringBank : MonoBehaviour {

	[HideInInspector]
	public bool fileLoaded = false;

	public string outputFolder;
	public string stringBankName;

	[HideInInspector]
	public string jsondata;


	public void loadFile(string conts) {
		jsondata = conts;
		fileLoaded = true;
	}
}
