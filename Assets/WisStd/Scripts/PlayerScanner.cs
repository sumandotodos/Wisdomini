using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum PlayerColor { Brown, Yellow, Green, Blue };
public enum ClassifyColor { Brown, Yellow, Green, Blue, White, Other };

public class PlayerScanner : Task, PressReleaseButtonListener {

	public MasterController_multi masterController;

	public int DetectedN;
	public int GameController_multiN;
	public RawImage raw;
	public Texture blackTex;
	//WebCamTexture liveTex;
	public float width, height;
	float windowWidth, windowHeight;
	//public Text rText, gText, bText;
	//public Text detectedPlayerText;
	//public Text maxColorText;
	public ClassifyColor detectedColor;
	//public Text pixelCountText;
	float elapsedTime;
	const float detectTime = 2.0f;
	public AudioClip pipSound;
	public AudioClip allSetSound;
	public AudioManager aManager;
	ClassifyColor candidatePlayer;
	public Canvas canvas;
	int intDetectedPlayer;
	public GameController_multi gameController;
	public RawImage backdrop;
	Material backdropMat;
	public Button retryButton;
	public Button allSetButton;
	public Text debugInfo;
	List<int>reportedColor;
	public RawImage[] playerYinYang;
	bool isWaitingForConfirmation = false;
	bool canUseColor = false;
	public UIFaderScript fader;
	public Texture[] backdropVersions;
	public int state = 0; // 0: idle
				   // 1: detecting
	bool working = false;


	public void startGetPlayerTask(Task w) {
		
		waiter = w;
		w.isWaitingForTaskToComplete = true;

		working = false;

		canvas.enabled = true;

		#if UNITY_IOS
		raw.transform.localScale = new Vector3 (1, -1, 1);
		#endif

	

	

        width = 0;
        height = 0;

		elapsedTime = 0.0f;

		retryButton.interactable = false;
		allSetButton.interactable = false;

		candidatePlayer = ClassifyColor.Other;

		if (!gameController.isMaster) {
			allSetButton.gameObject.SetActive (false);
		}

		backdropMat = backdrop.material;

		state = 1;
		intDetectedPlayer = -1;
		detectedColor = ClassifyColor.Other;

		for (int i = 0; i < playerYinYang.Length; ++i) {
			playerYinYang [i].enabled = false;
		}

		fader.fadeIn ();
	}

	public int getDetectedPlayer() {

		return intDetectedPlayer;

	}

	// Use this for initialization
	void Start () {

		//canvas.enabled = false;




	
	}
	
	// Update is called once per frame
	void Update () {

		

	
	}

	// network callback from message "unreportcolor"
	public void unreportColor(int color) {

		playerYinYang [color].enabled = false;

	}

	// network callback from message "reportcolor"
	public void reportColor(int color) {

		playerYinYang [color].enabled = true;

	}

	public void confirmColor(int c) {

		if (c == intDetectedPlayer) { // this message will be broadcast, we have to filter out
			isWaitingForConfirmation = false;
			canUseColor = true;
		}
		playerYinYang [c].enabled = true;


	}

	public void unconfirmColor(int c) {

		if (c == intDetectedPlayer) {
			isWaitingForConfirmation = false;
			canUseColor = false;
		}

	}

	public void startGame() { // called by network command 'startgame' issued by master's playerScanner

		

	}

	// network callback: "querycoloravailable"
	public void queryColorAvailable(int color) {

		if (playerYinYang [color].enabled == true) {
			gameController.networkAgent.broadcast ("colornotavailable:" + color + ":");
		} else {
			playerYinYang [color].enabled = true;
			gameController.networkAgent.broadcast ("coloravailable:" + color + ":");
		}

	}


	bool pixelIsWhite(float r, float g, float b) {
		return ((Mathf.Abs(r-g)<0.1f) && (Mathf.Abs(r-b)<0.1f) && (Mathf.Abs(g-b)<0.1f));
	}

	ClassifyColor classifyColor(float r, float g, float b) {
		ColorUtils.hsv newHsv = ColorUtils.rgb2hsv (r, g, b);
		return classifyColor (newHsv);
	}

	ClassifyColor classifyColor(ColorUtils.hsv color) {

		if (color.v < 0.1f)
			return ClassifyColor.Other;

		if (color.s < 0.1f)
			return ClassifyColor.White;

		//if(color.s < 0.25f) return ClassifyColor.Other;
		//if (color.v < 0.25f)
		//	return ClassifyColor.Other;

		if (color.h < 20.0f)
			return ClassifyColor.Brown;
		if (color.h < 65.0f)
			return ClassifyColor.Yellow;
		if (color.h < 190.0f)
			return ClassifyColor.Green;
		if (color.h < 270.0f)
			return ClassifyColor.Blue;

		return ClassifyColor.Other;

	}






	/*
	 * 
	 * 
	 * button callback functions
	 * 
	 * 
	 */

	public void retryCallback() {
		state = 1; // keep detecting
		retryButton.interactable = false;
		elapsedTime = 0.0f;
		if (!gameController.isMaster) {
			gameController.networkAgent.sendCommand (0, "unreportcolor:" + intDetectedPlayer + ":");
			unreportColor (intDetectedPlayer);
		} else {
			unreportColor (intDetectedPlayer);
			gameController.networkAgent.broadcast ("unreportcolor:" + intDetectedPlayer + ":");
		}
		backdrop.texture = backdropVersions[4];
		playerYinYang [intDetectedPlayer].enabled = false;

	}

	public void allSetCallback() {

		// only master has allSet

		// tell everybody to start the game for real

		gameController.networkAgent.broadcast ("startgame");
		startGame ();
		// all players must have different colors. Check that.

	}


	// button presses

	public void onButtonPress() {
		
	}

	public void onButtonRelease() {
		
	}
}

