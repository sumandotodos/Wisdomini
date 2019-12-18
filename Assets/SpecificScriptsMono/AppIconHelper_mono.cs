using UnityEngine;
using System.Collections;


public class AppIconHelper_mono : MonoBehaviour {

	public int wisdom;
	public int individual;

	public SchoolActivityController_mono eventDispatcher;

	public void onClickEvent() {

		eventDispatcher.clickAppIcon (wisdom, individual);

	}
}

