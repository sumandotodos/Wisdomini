using UnityEngine;
using System.Collections;


public class AppIconHelper_multi : MonoBehaviour {

	public int wisdom;
	public int individual;

		public NotMySchoolController_multi eventDispatcher;

	public void onClickEvent() {

		eventDispatcher.clickAppIcon (wisdom, individual);

	}
}

