using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WisStr {

public class UIGoldCounter : MonoBehaviour {

	public GameController_multi gameController;
	float timer;
	const float delay = 0.1f;
	int state;
	int localGold;
	Text theText;

	// Use this for initialization
	void Start () {
		theText = this.GetComponent<Text> ();
		state = 1;
		localGold = gameController.playerList [gameController.localPlayerN].gold;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {

		}

		if (state == 1) {
			timer += Time.deltaTime;
			if (timer > delay) {
				timer = 0.0f;
				if (localGold > gameController.playerList [gameController.localPlayerN].gold) {
					--localGold;
				} else if (localGold < gameController.playerList [gameController.localPlayerN].gold) {
					++localGold;
				} else
					state = 1;
			}
			theText.text = "" + localGold;
		}
	}
}

}