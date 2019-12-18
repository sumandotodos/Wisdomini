using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorController : MonoBehaviour {

	public string StandardBootstrapScene;
	public string KidsBootstrapScene;
	public string MonoBootstrapScene;
	public string KidsMonoBootstrapScene;
	public AudioManager audioManager;
	public AudioClip clickSound;
	public UIFaderScript fader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickOnWisStandard() {
		audioManager.playSound (clickSound);
		fader.fadeOut ();
		SceneManager.LoadSceneAsync (StandardBootstrapScene);
	}

	public void clickOnWisKids() {
		audioManager.playSound (clickSound);
		fader.fadeOut ();
		SceneManager.LoadSceneAsync (KidsBootstrapScene);
	}

	public void clickOnWisMono() {
		audioManager.playSound (clickSound);
		fader.fadeOut ();
		SceneManager.LoadSceneAsync (MonoBootstrapScene);
	}

	public void clickOnWisKidsMono() {
		audioManager.playSound (clickSound);
		fader.fadeOut ();
		SceneManager.LoadSceneAsync (KidsMonoBootstrapScene);
	}
}
