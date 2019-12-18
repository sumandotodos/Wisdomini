using UnityEngine;
using System.Collections;


/*
 * 
 * This class serves as a wrapper for heroId parameter... sort of
 * 
 */

public class HeroClick : MonoBehaviour {

	public LifeTestActivityController_multi controller;
	public DossierControllerLite_multi controller2;
	public int heroId;

	int penalty = 0;

	public void setPenalty() {
		penalty = 1;
	}

	public void clickCallback() {
		if (penalty == 0) {
			if(controller!=null)
			controller.clickOnHero (heroId);
			if (controller2 != null)
				controller2.clickOnElement (heroId);
		}
		if (penalty > 0)
			--penalty;
	}

}
	