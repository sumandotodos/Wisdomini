using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestDeConectividadController : MonoBehaviour {

	public string RetryScene;
	public string GoOnScene;

	public Text FailReasonText;

	const string ReasonText_OFF = "Bluetooth apagado: actívalo para poder jugar";
	const string ReasonText_NOAVAL = "Bluetooth no disponible, no puedes jugar con este dispositivo";
	const string ReasonText_NOCAPAB = "Sistema bluetooth no compatible, no puedes jugar con este dispositivo";

	public UIScaleFader bottonScaler;
	public UIScaleFader continueScaler;

	public RawImage httpIndicatorRI;
	public RawImage bleIndicatorRI;
	public FGBetterNetworkAgent networkController;

	public Texture greenIndicator;
	public Texture redIndicator;
	public Texture orangeIndicator;

	// Use this for initialization
	void Start () {
		StartCoroutine ("testCoRo");	
	}

	const string server = "apps.flygames.org";

	public IEnumerator testCoRo() {
		bool httpsPass = false;
		bool blePass = false;
		yield return  new WaitForSeconds(0.25f);

		// HTTPS test
		WWW www;
		yield return www = new WWW ("https://" + server);
		if (www.error != null) {
			httpIndicatorRI.texture = redIndicator;
		} else {
			httpIndicatorRI.texture = greenIndicator;
			httpsPass = true;
		}
		yield return  new WaitForSeconds(0.5f);


		// Bluetooth LE Test
		int btCapabilities = networkController.managers[FGBetterNetworkAgent.Module_BLE].QueryCapabilities();
		if (btCapabilities == BLEManager.k_OK) {
			bleIndicatorRI.texture = greenIndicator;
			blePass = true;
		} else if (btCapabilities == BLEManager.k_OFF) {
			bleIndicatorRI.texture = orangeIndicator;
			FailReasonText.text = ReasonText_OFF;
			blePass = false;
		} else if (btCapabilities == BLEManager.k_UNAVAILABLE) {
			bleIndicatorRI.texture = redIndicator;
			FailReasonText.text = ReasonText_NOAVAL;
			blePass = false;
		} else if (btCapabilities == BLEManager.k_UNCAPABLE) {
			bleIndicatorRI.texture = redIndicator;
			FailReasonText.text = ReasonText_NOCAPAB;
			blePass = false;
		}
		yield return new WaitForSeconds(0.5f);


		if (httpsPass && blePass) {
			yield return new WaitForSeconds (2.5f);
			goOn ();
		} else {
			bottonScaler.scaleIn ();
			continueScaler.scaleIn ();
		}


	}

	public void goOn() {
		SceneManager.LoadScene (GoOnScene);
	}

	public void retry() {
		SceneManager.LoadScene (RetryScene);
	}
}
