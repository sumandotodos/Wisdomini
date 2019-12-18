using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIChoosePlayerOKButton : MonoBehaviour, ButtonPressListener {

	public CarouselController carouselController;
	public PlayerSelectController_multi playerSelectController;

	public void buttonPress() 
	{
		int player = 0;
		player = carouselController.whichPlayer ();
		Debug.Log ("<color=purple>Requesting player " + player + "</color>");
		if (player != -1) {
			playerSelectController.buttonPress (player);
		}
	}

	void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}
}
	