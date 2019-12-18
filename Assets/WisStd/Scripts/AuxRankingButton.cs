using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AuxRankingButton : MonoBehaviour {

	public int id;
	public RankingController_multi rankingController;

	public void touchCallback() {
		rankingController.clickOnItem (id);
	}
}

