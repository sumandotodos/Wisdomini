using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PurchaseController2 : MonoBehaviour {

	public GameController_multi gameController;
	public TitlesController_multiNormal titleController;

	public void failTransaction() {
		Debug.Log ("Transaction failed for some reason");
	}
}

