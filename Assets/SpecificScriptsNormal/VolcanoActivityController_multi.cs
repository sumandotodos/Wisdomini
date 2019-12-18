using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class VolcanoActivityController_multi : Task {
	
	public UIFaderScript myTurnFader;
	public UIFaderScript notMyTurnFader;
	public GameObject myTurnCanvas;
	public GameObject notMyTurnCanvas;
	public GameController_multi gameController;
	public RawImage yesGlo;
	public RawImage noGlo;
	public PlayerActivityController_multi playerActivityController;
	public NotMyTurnController_multi notMyTurnController;

	public ValorationController_multi valorationController;

	public int nVotesPlus1, nVotesMinus1;

	int state = 0;

	bool isWaitingForVotation;
	bool votationResult;
	bool isMyTurn;

	void Start () {
	
	}

	public void startVolcanoTask(Task w) {

		w.isWaitingForTaskToComplete = true;
		waiter = w;
		state = 0;
		noGlo.enabled = false;
		yesGlo.enabled = false;

		if (gameController.localPlayerN == gameController.playerTurn) { // if it is 'my' turn

			isMyTurn = true;

			myTurnCanvas.SetActive (true);
			notMyTurnCanvas.SetActive (false);

			gameController.networkAgent.broadcast ("startvolcanovotation");
			//gameController.endGameAgreeYes ();  volcano player does not vote!

			myTurnFader.fadeIn ();

		} else {

			isMyTurn = false;

			myTurnCanvas.SetActive (false);
			notMyTurnCanvas.SetActive (true);

			notMyTurnFader.fadeIn ();

		}
		
			
		// both go to state 0


	}


	/* touch interface callbacks */

	public void volcanoVoteYES() {

		gameController.endGameAgreeYes ();
		yesGlo.enabled = true;
		noGlo.enabled = false;

	}

	public void volcanoVoteNO() {

		gameController.endGameAgreeNo ();
		yesGlo.enabled = false;
		noGlo.enabled = true;

	}

	/* network callbacks */
	
	void Update () 
	{	
		if (state == 0) { // keeping an eye on unanimity

			int firstPlayerPresent = 0;
			while (gameController.playerPresent [firstPlayerPresent] == false) 
			{
				++firstPlayerPresent;
			}

			nVotesPlus1 = 0;
			nVotesMinus1 = 0;

			for (int i = firstPlayerPresent; i < GameController_multi.MaxPlayers; ++i) 
			{
				if (gameController.playerPresent [i]) 
				{
					if (gameController.playerList [i].endGameVote == 1)
						++nVotesPlus1;
					if (gameController.playerList [i].endGameVote == -1)
						++nVotesMinus1;
				}
			}

			if ((nVotesPlus1 == (gameController.nPlayers-1))) 
			{
				state = 1;
			}

			if ((nVotesMinus1 == (gameController.nPlayers-1))) 
			{
				state = 2;
			}
		}

		if (state == 1) { // voted to finish the game				
			playerActivityController.volcanoResult = 1;
			valorationController.screenWidth = Screen.width;
			valorationController.screenHeight = Screen.height;
			notifyFinishTask (); // back to playerActivity
		}

		if (state == 2) { // voted to resume the game
			// reset volcano votation list
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
				gameController.playerList [i].endGameVote = 0;
			}
			
			playerActivityController.volcanoResult = -1;
			valorationController.screenWidth = Screen.width;
			valorationController.screenHeight = Screen.height;
			notifyFinishTask (); // back to playerActivity
		}
	}
}

