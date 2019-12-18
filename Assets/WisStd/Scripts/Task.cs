using UnityEngine;
using System.Collections;

public class Task : MonoBehaviour {

	[HideInInspector]
	public bool isWaitingForTaskToComplete = false;
	[HideInInspector]
	public Task waiter = null;

	[HideInInspector]
	public bool bReturnValue;
	[HideInInspector]
	public int iReturnValue;
	[HideInInspector]
	public float fReturnValue;
	[HideInInspector]
	public string sReturnValue;

	public void notifyFinishTask() {
		if (waiter != null) {
			waiter.isWaitingForTaskToComplete = false;
			waiter = null;
		}
	}

	public void returnInteger(int i) {
		iReturnValue = i;
	}

	public void returnFloat(float f) {
		fReturnValue = f;
	}

	public void returnString(string s) {
		sReturnValue = s;
	}

	public void returnBool(bool b) {
		bReturnValue = b;
	}

}
