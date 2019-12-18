using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;


/*
 * 
 * 
 * This class handles the game mechanics, player intercomunication, etc...
 * 
 * 
 * 
 * 
 */

public class Notification {

	public const int CONSTRUYEGOMPA = 0;
	public const int CONSTRUYEESCUELA = 1;
	public const int GANAOROS = 2;
	public const int CIMAVOLCÁN = 3;
	public const int TRIPLESABIDURIA = 4;
	public const int TIENE3EXPLORADORAS = 5;
	public const int TIENE4MAGAS = 6;
	public const int TIENE5GUERREROS = 7;
	public const int COMPRABOSQUE = 8;
	public const int LOGRABUMI = 9;
	public const int LOGRAINICIACION = 10;
	public const int LOGRA = 11;
	public const int CONSIGUESEMILLA = 12;
	public const int LOGRAHEROE = 13;
	public const int SEENFRENTA = 14;
	public const int VENCESOMBRAS = 15;
	public const int DOMINADOPORSOMBRAS = 16;
	public const int TIENEAHORA7HEROES = 17;
	public const int REPRESENTANTEDE = 18;
	public const int COMPRAESCUELA = 19;
	public const int COMPRAGOMPA = 20;
	public const int PIERDEOROS = 21;
	public const int CONSIGUESEMILLAS = 22;
	public const int LOGRAHEROES = 23;
	public const int PIERDESEMILLA = 24;
	public const int GANACARTA = 25;
	public const int GANACARTAYSEMILLA = 26;
	public const int GANAUNORO = 27;
	public const int CONSIGUEUNASOLASEMILLA = 28;


}

[Serializable]
public class TurnBackup {

	public List<string>notifications;
	public int turn;
	public int playerTurn;
	public List<Player> playerList;

	public List<string> dismissedTips;


	public bool guruPickedOrange = false;
	public bool guruPickedYellow = false;
	public bool guruPickedRed = false;
	public bool guruPickedPurple = false;
	public bool guruPickedGreen = false;
	public bool guruPickedBlue = false;
	public bool guruPickedBrown = false;

	public int[] forestOwner;

	public int warriorOwner = -1;
	public int explorerOwner = -1;
	public int wizardOwner = -1;

	public TurnBackup() {

		dismissedTips = new List<string> ();
		playerList = new List<Player> ();
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			playerList.Add (new Player ());
		}
		forestOwner = new int[GameController_multi.MAXFORESTS];
		for (int i = 0; i < forestOwner.Length; ++i) {
			forestOwner [i] = -1;
		}

	}

}

/* Let's not do this like this
public abstract class GameAction {

	public GameController_multi gameController;
	public string actionName;
	public int affectedPlayer;
	abstract public void execute ();

}

public class AddGoldAction:GameAction {

	public int amount;
	override public void execute() {

		gameController.addGold (affectedPlayer, amount);

	}

}
*/

class GameEvent {

	int player;
	int eventId;

	public string toString() {
		return "";
	}

}


// player colors:   0 blue       1 yellow     2 green      3 brown
// forest colors:   0 magenta    1 cyan       2 deepblue   3 orange

public class GameController_multi : Task {

	public PlayerSelectController_multi playerSelectController;

	public GameObject galleryCanvas;
	public GalleryController galleryController;

	public GameObject notMyTurnCanvas;

	public TipSaveData tipSaveData;

	public QuickSaveInfo quickSaveInfo;
	public QuickSaveData quickSaveData;

	public GameObject rankingCanvas;
	public GameObject dossierCanvas;

	public string randomChallenge;
	public string datetimeOfGame;

	//public JoinGameActivityController joinNewGameController_multi;
	//public NewGameActivityController createNewGameController_multi;

	public UIEnableImageOnTimeout noConnectionIcon_N;

	public const int WARRIOR = 0;
	public const int MASTER = 1;
	public const int PHILOSOPHER = 2;
	public const int SAGE = 3;
	public const int EXPLORER = 4;
	public const int WIZARD = 5;
	public const int YOGI = 6;

	public const int ORANGE = 0;
	public const int TEAL = 1;
	public const int PURPLE = 2;
	public const int DARKBLUE = 3;

	public const int BLUE = 7;
	public const int YELLOW = 1;
	public const int GREEN = 6;
	public const int BROWN = 3;
	public const int WHITE = 4;
	public const int RED = 5;

	public const int MAXFORESTS = 4;

	public const int MAXGOMPAS = 10;
	public const int MAXSCHOOLS = 8;

	public AudioClip dingSound;

	public RosettaWrapper rosettaWrap;
	public StringBank playerNames;
	public StringBank notifications;
	public StringBank forestNames;
	public StringBank classNames;
	public StringBank classNamesLos;
	public StringBank articulo;

	public const int MOSTINITIATIONSSCORE = 7;
	public const int FIRSTTOVOLCANOSCORE = 7;
	public const int MOSTHEROESSCORE = 5;
	public const int MOSTCLASSESSCORE = 3;
	public const int MOSTGOMPAS = 2;
	public const int MOSTSCHOOLS = 2;
	public const int CREATIVITYSCORE = 3;
	public const int EMPATHYSCORE = 3;





	/* game state */

	TurnBackup turnBackup;




	/* Turn state */
	// common

	public List<string> dismissedTips;

	List<int> forestProperty; // forestProperty[2] == 3  means the yogi has bought the deepblue forest

	public bool guruPickedOrange = false;
	public bool guruPickedYellow = false;
	public bool guruPickedRed = false;
	public bool guruPickedPurple = false;
	public bool guruPickedGreen = false;
	public bool guruPickedBlue = false;
	public bool guruPickedBrown = false;

	// players
	public List<Player> playerList;
	public List<bool> playerPresent;

	[HideInInspector]
	public int[] forestOwner;

	/* End of turnstate */




	// current turn actions. Need to be commited.
	//List<GameAction> turnActions;

	//bool isWaitingForSynchronization;

	int creativityAwardedPlayer;
	int empathyAwardedPlayer;

	public const int MaxPlayers = 6;
	public const int MaxCharacters = 6;
	public const int MaxEnergies = 7;

	//[HideInInspector]
	public int turn; // current turn, from 0 to ...
	//[HideInInspector]
	public int playerTurn; // current player turn, modulus MaxPlayers

	[HideInInspector]
	public bool isMaster = false;

	[HideInInspector]
	public int nPlayers = 1;

	public int reportedPlayers = 0;
	public int awardedPlayers = 0;

	public string localUserLogin;
	public string localUserPass;
	public string localUserEMail;
	[HideInInspector]
	public int localPlayerN;

	public PlayerScanner scannerController;
	public ContinueGameController continueGameController_multi;

	public int warriorOwner = -1;
	public int explorerOwner = -1;
	public int wizardOwner = -1;

	public int creativityAwardPlayer;
	public int empathyAwardPlayer;

	public FGBetterNetworkAgent networkAgent;

	const float reconnectRetryInterval = 10.0f;

	[HideInInspector]
	public int turnVotesOK;
	[HideInInspector]
	public int turnVotesNO;
	public List <int>PlayerTurnVotation;
	public List<string>notificationList;

	public Text debugText;
	public Text otherPlayersText;
	// states 
		// voting
	bool tryingToEndGame = false;
	bool tryingToResetGame = false;
	bool tryingToDismissPlayer = false;
	bool playerDisconnected = false;
	float timer = 0.0f;
	[HideInInspector]
	public int state = 0;

	[HideInInspector]
	public string qrCodeContents;
	[HideInInspector]
	public Texture qrEncodedSessionInfo;

	public RawImage globalQRSessionInfoImage;

	[HideInInspector]
	public int gameState; // 0: titles    1: synching players    2: playing


	/*
	 * 
	 * Common interface reference
	 * 
	 */
	public GameObject volcanoVotationInterface;
	public GameObject serverDisconnectInterface;
	public GameObject playerDisconnectInterface;
	public Text playerDisconnectText;
	public Text dismissPlayerText;
	public UIFaderScript globalFader;

	/*
	 *
	 * These references are needed to pass messages from network commands
	 * 
	 */
	//public NewGameActivityController newGameController_multi;
	//public JoinGameActivityController joinGameController_multi;
	public PlayerActivityController_multi playerActivityController;
	public NotMyTurnController_multi notMyTurnController;
	public NotMySchoolController_multi notMyTurnSchoolController;
	public SchoolActivityController_multi myTurnSchoolController;
	public NotMyTurnGuruActivityController_multi notMyTurnGuruActivityController_multi;
	public ValorationController_multi valorationController;
	public LifeTestActivityController_multi lifeTestController;
	public LifeTestVoteActivityController_multi lifeTestVoteController;
	public LastChanceController_multi lastChanceController;
	public SeedToPlayerController_multi seedToPlayerController;
	public GuruActivityController_multi guruActivityController;


	public MasterController masterController;

	public string gameRoom;
	public string gameHost;

	public string challenge;
	public string datetime;

	public int syncedPlayers = 0;
	public int creativityVotes = 0;
	public int empathyVotes = 0;


	/* notification related methods */

	// the full service
	public void addNotification(int idx, string p1, string p2, string p3, int female) {

		string notif = getNotificationText (idx);
		addNotification (notif, p1, p2, p3, female);
		notif = notif.Replace(" ", "_");
		p1 = p1.Replace (" ", "_");
		p2 = p2.Replace (" ", "_");
		p3 = p3.Replace (" ", "_");
		networkAgent.broadcast("notification:" + notif + ":" + p1 + ":" + p2+ ":" + p3 + ":" + female + ":");

	}

	public void addNotification(string text, string p1, string p2, string p3, int female) {

		// substitute parameters
		text = text.Replace("_", " "); // remove underscores
		p1 = p1.Replace("_", " ");
		p2 = p2.Replace("_", " ");
		p3 = p3.Replace("_", " ");
		text = text.Replace ("<0>", getPlayerArticulo (female));
		text = text.Replace ("<1>", p1);
		text = text.Replace ("<2>", p2);
		text = text.Replace ("<3>", p3);

		notificationList.Add (text);

		notMyTurnController.updateNotifications ();

	}

	public void saveTurn() {

		turnBackup.turn = turn;
		turnBackup.playerTurn = playerTurn;

		for (int i = 0; i < MaxPlayers; ++i) {
			if (playerPresent [i])
				playerList [i].backup (turnBackup.playerList [i]);
		}
		turnBackup.notifications = new List<string> (notificationList);
		for (int i = 0; i < GameController_multi.MAXFORESTS; ++i) {
			turnBackup.forestOwner [i] = forestOwner [i];
		}
		turnBackup.wizardOwner = wizardOwner;
		turnBackup.explorerOwner = explorerOwner;
		turnBackup.warriorOwner = warriorOwner;

		turnBackup.guruPickedOrange = guruPickedOrange;
		turnBackup.guruPickedYellow = guruPickedYellow;
		turnBackup.guruPickedPurple = guruPickedPurple;
		turnBackup.guruPickedRed = guruPickedRed;
		turnBackup.guruPickedGreen = guruPickedGreen;
		turnBackup.guruPickedBlue = guruPickedBlue;
		turnBackup.guruPickedBrown = guruPickedBrown;
	}

	public void rollbackTurn() {

		turn = turnBackup.turn;
		playerTurn = turnBackup.playerTurn;
		for (int i = 0; i < MaxPlayers; ++i) {
			if (playerPresent [i])
				turnBackup.playerList [i].backup (playerList [i]);
		}
		notificationList = new List<string> (turnBackup.notifications);
		for (int i = 0; i < GameController_multi.MAXFORESTS; ++i) {
			forestOwner [i] = turnBackup.forestOwner [i];
		}
		wizardOwner = turnBackup.wizardOwner;
		explorerOwner = turnBackup.explorerOwner;
		warriorOwner = turnBackup.warriorOwner;

		guruPickedOrange = turnBackup.guruPickedOrange;
		guruPickedYellow = turnBackup.guruPickedYellow;
		guruPickedPurple = turnBackup.guruPickedPurple;
		guruPickedRed = turnBackup.guruPickedRed;
		guruPickedBlue = turnBackup.guruPickedBlue;
		guruPickedGreen = turnBackup.guruPickedGreen;
		guruPickedBrown = turnBackup.guruPickedBrown;

	}

	public string getClassNameLos(int cl) {
		return classNamesLos.getString (cl);
	}

	public string getClassName(int cl) {
		return classNames.getString (cl);
	}

	public int getPlayerFemality(int p) {
		if (p == 2 || p == 3 || p == 5)
			return 1;
		return 0;
	}

	public int getPlayerFemality() {
		if (localPlayerN == 2 || localPlayerN == 3 || localPlayerN == 5)
			return 1;
		return 0;
	}

	public string getPlayerArticulo(int femality) {
		if (femality == 1)
			return articulo.getString (1);
		else
			return articulo.getString (0);
	}

	public string getNotificationText(int notif) {
		return notifications.getString (notif);
	}

	public string getPlayerName(int pl) {
		return playerNames.getString (pl);
	}

	public string getForestName(int frst) {
		return forestNames.getString (frst);
	}


	public int individualsInWisdom(int wis) {

		switch (wis) {
		case WARRIOR:
			return 5;
		case MASTER:
			return 7;
		case PHILOSOPHER:
			return 7;
		case SAGE:
			return 7;
		case EXPLORER:
			return 3;
		case WIZARD:
			return 4;
		case YOGI:
			return 7;
		default:
			return 7;
		}

	}

	// reset the game
	public void reset() {
		
	}

	public void resetGame() {

		nPlayers = 1;
		guruPickedOrange = false;
		guruPickedYellow = false;
		guruPickedRed = false;
		guruPickedPurple = false;
		guruPickedGreen = false;
		guruPickedBlue = false;
		guruPickedBrown = false;
		isMaster = false;
		reportedPlayers = 0;
		awardedPlayers = 0;
		warriorOwner = -1;
		explorerOwner = -1;
		wizardOwner = -1;
		state = 0;
		timer = 0.0f;

		syncedPlayers = 0;
		creativityVotes = 0;
		empathyVotes = 0;

		playerList = new List<Player> ();
		dismissedTips = new List<string> ();
		PlayerTurnVotation = new List<int> ();
		playerPresent = new List<bool> ();
		notificationList = new List<string> ();

		for (int i = 0; i < MaxPlayers; ++i) {
		
			playerPresent.Add (false);
			Player p = new Player ();
			playerList.Add (p);
			PlayerTurnVotation.Add (0);
	
		}

		playerTurn = 0;

		forestOwner = new int[MAXFORESTS];
		for (int i = 0; i < forestOwner.Length; ++i) {
			forestOwner [i] = -1;
		}

		creativityAwardedPlayer = ValorationController_multi.PlayerNone;
		empathyAwardedPlayer = ValorationController_multi.PlayerNone;

		volcanoVotationInterface.SetActive (false); // hide votation interface
		playerDisconnectInterface.SetActive (false);
		serverDisconnectInterface.SetActive (false);

		reportedPlayers = 0;
		awardedPlayers = 0;

		turnBackup = new TurnBackup ();
	}

	// Use this for initialization
	public void Start () {
	
	
		localUserLogin = "";
		localUserPass = "";


		playerNames.rosetta = rosettaWrap.rosetta;
		notifications.rosetta = rosettaWrap.rosetta;
		forestNames.rosetta = rosettaWrap.rosetta;
		classNames.rosetta = rosettaWrap.rosetta;
		classNamesLos.rosetta = rosettaWrap.rosetta;
		articulo.rosetta = rosettaWrap.rosetta;
		playerNames.reset ();
		notifications.reset ();
		forestNames.reset ();
		classNames.reset ();
		classNamesLos.reset ();
		articulo.reset ();

		resetGame ();

	}

	public void firstTurn() {
		// rotate anti-clockwise
		playerTurn = (playerTurn + 1) % MaxPlayers;
		while (playerPresent [playerTurn] == false) {
			playerTurn = (playerTurn + 1) % MaxPlayers;
		}
	}

	public void nextTurn() {

		galleryController.resetGallery ();
		galleryCanvas.SetActive (false);

		// rotate clockwise
		playerTurn = (playerTurn + (MaxPlayers - 1)) % MaxPlayers;
		while (playerPresent [playerTurn] == false) {
			playerTurn = (playerTurn + (MaxPlayers - 1)) % MaxPlayers;
		}
		saveTurn ();

		saveQuickSaveData ();
		saveQuickSaveInfo ();
		saveTipSaveData ();

		dossierCanvas.SetActive (false);
		rankingCanvas.SetActive (false);

	}

	public void setStartActivity(string act) {
		
		masterController.startActivity = act;

	}



	/* login/room setup methods */

	public void setUserLogin(string user) {

		localUserLogin = user;

	}

	public void setGameHost(string gh) {
		gameHost = gh;
	}

	public void setGameRoom(string gr) {
		gameRoom = gr;
	}

	public string getUserLogin() {

		return localUserLogin;

	}

	public void setUserPass(string pass) {

		localUserPass = pass;

	}

	public string getUserPass() {

		return localUserPass;

	}

	public void addPlayer(string room, string login) {

	}

	public void setNPlayers(int n) {
		nPlayers = n;
	}

	public int getNPlayers() {
		return nPlayers;
	}
	





	/* game mechanics methods */

	static public bool isPrincipalWisdom(int wis) {

		if ((wis == 1) || (wis == 2) || (wis == 3) || (wis == 6))
			return true;
		return false;

	}

	static public int heroMaxAmount(int wis) {

		if (isPrincipalWisdom (wis))
			return 7;
		if (wis == 0) {
			return 5;
		}
		if (wis == 4) {
			return 3;
		}
		if (wis == 5) {
			return 4;
		}
		return 0;

	}

	public bool heroMaxedOut(int h) {
		if (playerList [localPlayerN].heroAmount [h] == heroMaxAmount (h))
			return true;
		else
			return false;
	}

	public int numberOfMaxedHeroes() {
		int res = 0;
		for (int i = 0; i < 7; ++i) {
			if (this.heroMaxedOut (i))
				++res;
		}
		return res;
	}

	public bool principalsMaxedOut() {

		if ((playerList [localPlayerN].heroAmount [1] == 7) &&
		    (playerList [localPlayerN].heroAmount [2] == 7) &&
		    (playerList [localPlayerN].heroAmount [3] == 7) &&
		    (playerList [localPlayerN].heroAmount [6] == 7)) {
			return true;
		} else
			return false;
	}

	public bool secondariesMaxedOut() {
		if ((playerList [localPlayerN].heroAmount [0] == 5) &&
			(playerList [localPlayerN].heroAmount [4] == 3) &&
			(playerList [localPlayerN].heroAmount [5] == 4)) {
			return true;
		} else
			return false;
	}

	public bool allMaxedOut() {
		return principalsMaxedOut () && secondariesMaxedOut ();
	}

	public void startTurn() {

		++turn;
		playerTurn = (playerTurn + 1) % playerList.Count;

	}

	/*
	public void commitTurn() {
		// execute all actions
		for (int i = 0; i < turnActions.Count; ++i) {
			turnActions [i].execute ();
		}
	}*/



	public void addGold(int nPlayer, int amount) {

		playerList [nPlayer].gold += amount;

	}

	public void clearGold(int nPlayer) {
		playerList [nPlayer].gold = 0;
	}



	public Color colorFromPlayerN(int n) {

		switch (n) {
		case 0:
			return new Color (0, 0.4f, 1, 1);

		case 1:
			return new Color (1, 0.85f, 0, 1);

		case 2:
			return new Color (0.1f, 1, 0.2f, 1);

		case 3:
			return new Color (0.65f, 0.35f, 0.15f, 1);

		}
		return Color.black;

	}

	// called from network manager
	//  as response to setplayerpresent
	public void setPlayerPresent(int pl) {
		
		++reportedPlayers;
		playerPresent [pl] = true;

	}

	public void buyForest(int f, int pl, int price) {
		forestOwner [f] = pl;
		playerList [pl].gold -= price;
	}

	public void build(int type, int pl, int price) {
		if (type == 0) {
			playerList [pl].nGompas++;
		} else
			playerList [pl].nSchools++;
		playerList [pl].gold -= price;
	}

	public void setAward(DragIconType iconType, int player) {
		if (iconType == DragIconType.bulb) {
			creativityAwardedPlayer = player;
		} else if (iconType == DragIconType.hand) {
			empathyAwardedPlayer = player;
		}
	}

	public int getAward(DragIconType iconType) {
		if (iconType == DragIconType.bulb) {
			return creativityAwardedPlayer;
		} else if (iconType == DragIconType.hand) {
			return empathyAwardedPlayer;
		}
		return ValorationController_multi.PlayerNone;
	}

	// calculate bumis

	public void calculateBumis() {

		for (int p = 0; p < GameController_multi.MaxPlayers; ++p) {
			// for each player...

			playerList [p].totalBumis = 0;
			playerList [p].mostGompasScore = 0;
			playerList [p].mostHeroesScore = 0;
			playerList [p].mostInitiationsScore = 0;
			playerList [p].mostInitiationClassesScore = 0;
			playerList [p].mostSchoolsScore = 0;
			playerList [p].firstToVolcanoScore = 0;
			playerList[p].empathyScore = 0;
			playerList [p].creativityScore = 0;


		}

		// get player with the most initiations
		int maxInitiations = 0;
		int playersWithMax = 1;
		int maxPlayer = -1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].nInitiations > maxInitiations) {
				playersWithMax = 1;
				maxInitiations = playerList [i].nInitiations;
			} else if (playerList [i].nInitiations == maxInitiations) {
				++playersWithMax;
			}
		}


		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if ((playerList [i].nInitiations == maxInitiations) && (maxInitiations > 0)) {
					playerList [i].totalBumis += (MOSTINITIATIONSSCORE/playersWithMax);
					playerList [i].mostInitiationsScore = (MOSTINITIATIONSSCORE/playersWithMax);
				}
			}
		//}



		for (int p = 0; p < GameController_multi.MaxPlayers; ++p) {

			// get first to volcano player
			if (playerList [p].firstToVolcano == true) {
				playerList [p].totalBumis += FIRSTTOVOLCANOSCORE;
				playerList [p].firstToVolcanoScore = FIRSTTOVOLCANOSCORE;
			}

		}



		int maxHeroes = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].totalHeroes > maxHeroes) {
				playersWithMax = 1;
				maxHeroes = playerList [i].totalHeroes;
			}
			else if (playerList [i].totalHeroes == maxHeroes) {
				++playersWithMax;
			}
		}

		//if(playersWithMax == 1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if ((playerList [i].totalHeroes == maxHeroes) && (maxHeroes > 7)) {
					playerList [i].totalBumis += (MOSTHEROESSCORE/playersWithMax);
					playerList [i].mostHeroesScore = (MOSTHEROESSCORE/playersWithMax);
				}
			}
		//}


		int maxInitiationDifferentClasses = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].initiationsDifferentClasses > maxInitiationDifferentClasses) {
				playersWithMax = 1;
				maxInitiationDifferentClasses = playerList [i].initiationsDifferentClasses;
			} else if (playerList [i].initiationsDifferentClasses == maxInitiationDifferentClasses) {
				++playersWithMax;
			}
		}

		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if ((playerList [i].initiationsDifferentClasses == maxInitiationDifferentClasses) && (maxInitiationDifferentClasses > 0)) {
					playerList [i].totalBumis += (MOSTCLASSESSCORE/playersWithMax);
					playerList [i].mostInitiationClassesScore = (MOSTCLASSESSCORE/playersWithMax);
				}
			}
		//}



		int maxGompas = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].nGompas > maxGompas) {
				playersWithMax = 1;
				maxGompas = playerList [i].nGompas;
			} else if (playerList [i].nGompas == maxGompas) {
				++playersWithMax;
			}
		}

		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if ((playerList [i].nGompas == maxGompas) && (maxGompas > 0)) {
					playerList [i].totalBumis += (MOSTGOMPAS/playersWithMax);
					playerList [i].mostGompasScore = (MOSTGOMPAS/playersWithMax);
				}
			}
		//}


		int maxSchools = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].nSchools > maxSchools) {
				playersWithMax = 1;
				maxSchools = playerList [i].nSchools;
			} else if (playerList [i].nSchools == maxSchools) {
				++playersWithMax;
			}
		}

		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if ((playerList [i].nSchools == maxSchools) && (maxSchools > 0)) {
					playerList [i].totalBumis += (MOSTSCHOOLS/playersWithMax);
					playerList [i].mostSchoolsScore = (MOSTSCHOOLS/playersWithMax);
				}
			}
		//}


		/*int maxCreativity = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].creativityAwards > maxCreativity) {
				playersWithMax = 1;
				maxCreativity = playerList [i].creativityAwards;
			}
			else if (playerList [i].creativityAwards == maxCreativity) {
				++playersWithMax;
			}
		}

		if (playersWithMax==1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
				if (playerList [i].creativityAwards == maxCreativity) {
					playerList [i].totalBumis += CREATIVITYSCORE;
					playerList [i].creativityScore = CREATIVITYSCORE;
				}
			}
		}


		int maxEmpathy = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (playerList [i].empathyAwards > maxEmpathy) {
				playersWithMax = 1;
				maxEmpathy = playerList [i].empathyAwards;
			}
			else if (playerList [i].empathyAwards == maxEmpathy) {
				++playersWithMax;
			}
		}

		if (playersWithMax == 1) {
			for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
				if (playerList [i].empathyAwards == maxEmpathy) {
					playerList [i].totalBumis += EMPATHYSCORE;
					playerList [i].empathyScore = EMPATHYSCORE;
				}
			}
		}*/




		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			playerList [i].totalBumis += playerList [i].trainings;
			//playerList [i].bumis = playerList [i].trainings;
		}

		for (int i = 0; i < GameController_multi.MaxPlayers; ++i) {
			if (!playerPresent [i])
				playerList [i].totalBumis = -1;
		}

	}


	/* persistance methods */

	public void loadData() {

		if (File.Exists (Application.persistentDataPath + "/save000.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/save000.dat", FileMode.Open);
			SaveData data = (SaveData) formatter.Deserialize (file);
			localUserLogin = data.currentLogin;
			localUserEMail = data.currentEMail;
			localUserPass = data.currentPass;
			file.Close ();

		}

	}

	public void saveData() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/save000.dat", FileMode.Create);

		SaveData data = new SaveData ();
		data.currentLogin = localUserLogin;
		data.currentPass = localUserPass;
		data.currentEMail = localUserEMail;

		formatter.Serialize (file, data);
		file.Close ();
	}


	/*public void resetGame() {

		for (int i = 0; i < MaxPlayers; ++i) {

			playerList [i].endGameVote = 0;
			playerPresent [i] = false;
			nPlayers = 1;

		}

	}*/


	// only executed by master
	public void attemptGrabPlayer(int pl, int whoWantsIt) {

		Debug.Log ("<color=purple>" +whoWantsIt+" is trying to grab player " + pl + "</color>");

		if (playerList [pl].login.Equals ("")) {
			// grant
			//controllerHub.networkController.sendCommand(whoWantsIt, FGNetworkManager.makeClientCommand("takeplayer"));
			networkAgent.broadcast("takeplayer:" + pl + ":" + whoWantsIt);
			playerList [pl].id = whoWantsIt;
			playerPresent [pl] = true;
			if (!whoWantsIt.Equals (localUserLogin)) { 
				playerSelectController.disablePlayer (pl);
			} else {
				//controllerHub.gameController.localNPlayer = pl;
				playerSelectController.takePlayer (pl, whoWantsIt);
			}


		} else {
			// reject
			networkAgent.sendCommand(whoWantsIt, "donttakeplayer:");
		}
	}


	/* network methods */

	public void network_disconnect() {
		networkAgent.disconnect ();
	}


	public void network_processCommand(string comm) {

		comm = comm.Replace ("\n", "");

		string[] commands = comm.Split ('$'); // split back to back commands
		int realLength = comm.Length;

		char[] charcomm = comm.ToCharArray ();
		int nCommands = 0;
		for (int i = 0; i < charcomm.Length; ++i) {
			if (charcomm [i] == '$')
				++nCommands;
		}

		for (int i = 0; i < nCommands; ++i) {

			string command = commands [i];


			int safeIndex = command.IndexOf ('#');
			string[] safeArg;
			bool processThisCommand = true;
			bool safeCommand = false;
			int safeSeqNum = -1;
			int originOfSafeCommand = -1;
			int expectedSeq = -1;

			if (safeIndex != -1) { // safe command
				safeCommand = true;
				safeArg = command.Split('#');
				int.TryParse (safeArg [0], out safeSeqNum);
				command = safeArg [2];
				int.TryParse (safeArg [1], out originOfSafeCommand);
				expectedSeq = networkAgent.receiveSeqFor (originOfSafeCommand);
				if (safeSeqNum != expectedSeq)
					processThisCommand = false;
				//BEGIN NUEVO
				if (safeSeqNum < expectedSeq) {
					networkAgent.sendCommandUnsafe (originOfSafeCommand, "ACK:" + safeSeqNum + ":" + networkAgent.id);
				}
				//END NUEVO

			}

			if (processThisCommand) {

				string[] arg = command.Split (':');

				// playerready is issued by clients trying to join in
				if (command.StartsWith ("playerready")) {
					int pl;
					int.TryParse (arg [1], out pl);
					//newGameController_multi.addPlayer (pl);
				} 

				// buyforest:<forest>:<playern>:<price>
				else if (command.StartsWith ("buyforest")) {

					int pl;
					int f;
					int price;
					int.TryParse (arg [1], out f);
					int.TryParse (arg [2], out pl);
					int.TryParse (arg [3], out price);
					buyForest (f, pl, price);

				} 

				// build:<type of building>:<playern>:<price>
				else if (command.StartsWith ("build")) {

					int pl;
					int f;
					int price;
					int.TryParse (arg [1], out f);
					int.TryParse (arg [2], out pl);
					int.TryParse (arg [3], out price);
					build (f, pl, price);

				} 

				// notification:<text>:<param1>:<param2>:<param3>
				else if (command.StartsWith ("notification")) {

					string text;
					string param1;
					string param2;
					string param3;
					text = arg [1];
					param1 = arg [2];
					param2 = arg [3];
					param3 = arg [4];
					int fem;
					int.TryParse (arg [5], out fem);
					addNotification (text, param1, param2, param3, fem);

				} 

				// setnplayers is issued by NewGameActivityController and accepted by clients
				else if (command.StartsWith ("setnplayers")) {

					int howMany;
					int.TryParse (arg [1], out howMany);
					//joinGameController_multi.setNPlayers (howMany);

				} 

				// sync player
				else if (command.StartsWith ("sync")) {

					++syncedPlayers;

				} 

				// query avaiable color
				else if (command.StartsWith ("querycoloravailable")) {

					int c;
					int.TryParse (arg [1], out c);
					scannerController.queryColorAvailable (c);

				} 

				// tell notMyTurnController to start school Test evaluation
				// to evaluate player p 
				else if (command.StartsWith ("schooltest")) {
					int p, e;
					int.TryParse (arg [1], out p);
					int.TryParse (arg [2], out e);
					notMyTurnController.schoolTest (p, e);

				} else if (command.StartsWith ("gurutest")) {
					int type;
					int question;
					int.TryParse (arg [1], out type);
					int.TryParse (arg [2], out question);
					notMyTurnController.guruTest (type, question);
				}

				// tell turn player to finish school activity
				// to evaluate player p 
				else if (command.StartsWith ("finishschool")) {
					int s;
					int.TryParse (arg [1], out s); // parse score
					int indiv;
					int.TryParse (arg [2], out indiv);
					if (s == 0)
						playerActivityController.obtainedMaru = 1;
					if (s == 1)
						playerActivityController.obtainedTick = 1;
					//notMyTurnController.schoolTest (p, e);
					myTurnSchoolController.finishSchool (s, indiv);


				}

				// addinitiation:<player>:<hero class>
				else if (command.StartsWith ("addseeds")) {

					int pl;
					int c;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out c);
					playerList [pl].addSeeds (c);

				}

				// addinitiation:<player>:<hero class>
				else if (command.StartsWith ("addinitiation")) {

					int pl;
					int c;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out c);
					playerList [pl].addInitiation (c);

				}

				// addtraining:<player>
				else if (command.StartsWith ("addtraining")) {

					int pl;

					int.TryParse (arg [1], out pl);
				
					playerList [pl].addTraining ();

				}


				//gurupicked:<player>:<main wisdom>
				else if (command.StartsWith ("gurupicked")) {
					int pl;
					int w;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out w);
					switch (w) {
					case PURPLE:	
						guruPickedPurple = true;
						break;
					case YELLOW:
						guruPickedYellow = true;
						break;
					case ORANGE:
						guruPickedOrange = true;
						break;
					case RED:
						guruPickedRed = true;
						break;
					case GREEN:
						guruPickedGreen = true;
						break;
					case BLUE:
						guruPickedBlue = true;
						break;
					case BROWN:
						guruPickedBrown = true;
						break;
					}
				}

				//setmainwisdom:<player>:<main wisdom>
				else if (command.StartsWith ("setmainwisdom")) {
					int pl;
					int w;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out w);
					playerList [pl].setMainWisdom (w);
				}

				//addwisdom:<player>:<main wisdom>
				else if (command.StartsWith ("addwisdom")) {
					int pl;
					int w;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out w);
					playerList [pl].addWisdom (w);
				}

				// addhero:<player>:<hero class>
				else if (command.StartsWith ("addhero")) {

					int pl;
					int c;
					int am;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out c);
					int.TryParse (arg [3], out am);
					playerList [pl].addHero (c, am);

				}

				// addseed:<player>:<numero de semillas>
				else if (command.StartsWith ("addseed")) {

					int pl;
					int s;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out s);
					playerList [pl].seeds += s;

				}

				// finishvaloration
				else if (command.StartsWith ("finishvaloration")) {

					valorationController.hasReceivedSignal.enabled = true;
					valorationController.finishValoration ();

				}

				//
				else if (command.StartsWith ("addplayerseed")) {
					seedToPlayerController.nSeedsToPlayers++;
				}

				// award
				else if (command.StartsWith ("receiveaward")) {

					if (arg [1].Equals ("empathy")) {
						valorationController.receiveAward (true, false);
					} else if (arg [1].Equals ("creativity")) {
						valorationController.receiveAward (false, true);
					}



				}

				// colornotavailable:<color>
				else if (command.StartsWith ("colornotavailable")) {

					int c;
					int.TryParse (arg [1], out c);
					scannerController.unconfirmColor (c);

				}

				// coloravailable:<color>
				else if (command.StartsWith ("coloravailable")) {

					int c;
					int.TryParse (arg [1], out c);
					scannerController.confirmColor (c);

				}

				// setplayern:<login>:<playern>
				else if (command.StartsWith ("setplayerloginn")) {

					int pl;
					int.TryParse (arg [2], out pl);
					playerList [pl].login = arg [1];

				} 

				// receive awards
				else if (command.StartsWith ("creativity")) {

					int pl;
					int dif;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out dif);
					playerList [pl].creativityAwards += dif;
					creativityVotes += dif;


				}
				// receive awards
				else if (command.StartsWith ("empathy")) {

					int pl;
					int dif;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out dif);
					playerList [pl].empathyAwards += dif;
					empathyVotes += dif;


				}


				// start the scanning!
				else if (command.StartsWith ("setplayerpresent")) {

					int pl;
					int.TryParse (arg [1], out pl);
					setPlayerPresent (pl);

				} 

				// start the scanning!
				else if (command.StartsWith ("startscan")) {

					//joinGameController_multi.startscan (arg [1], arg [2]);
		
				} 


				// start the game!
				else if (command.StartsWith ("startgame")) {

					scannerController.startGame ();

				} 


				// report scanned color
				//  reportcolor:playern:colorn
				else if (command.StartsWith ("reportcolor")) {

					int c;
					int.TryParse (arg [1], out c);
					scannerController.reportColor (c);

				}

				// unreport scanned color
				//  reportcolor:playern:colorn
				else if (command.StartsWith ("unreportcolor")) {

					int c;
					int.TryParse (arg [1], out c);
					scannerController.unreportColor (c);

				}

			/*
				//  reportcolor:playern:colorn
				else if (command.StartsWith ("addgold")) {

					int pl, a;
					int.TryParse (arg [1], out pl);
					int.TryParse (arg [2], out a);
					addGold (pl, a); // there is really no need to functionize
					// change to playerList[pl].gold += a
					// for coherence
					// if you want to sleep better at night
					// might as well crete functions for all state updates
					//   addGold, addBumi, etc...
					// but I'm afraid the world will remain the same :(

				}*/

				//  reportcolor:playern:colorn
				else if (command.StartsWith ("addbumi")) {

					int pl, a;
					int.TryParse (arg [1], out pl);
					playerList [pl].bumis++;

				}



				// setplayern:<login>:<nplayer>
				// set player number
				else if (command.StartsWith ("setplayern")) {

					if (localUserLogin.Equals (arg [1])) {
						int p;
						int.TryParse (arg [2], out p);
						localPlayerN = p;
					}

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("setturn")) {

					int t;
					int.TryParse (arg [1], out t);
					playerTurn = t;

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("nextturn")) {

					bool b;
					bool.TryParse (arg [1], out b);
					playerActivityController.nextTurn (b);

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("startvolcanovotation")) {

					/*volcanoVotationInterface.SetActive (true);
				for (int i = 0; i < playerList.Count; ++i) {
					playerList [i].endGameVote = 0;
				}
				tryingToEndGame = true;
				*/
					//playerActivityController
					playerActivityController.startVolcano ();

				}

				// setturn:<playerturn>
				else if (command.StartsWith ("firsttovolcano")) {

					int t;
					int.TryParse (arg [1], out t);
					playerList [t].firstToVolcano = true;
					playerList [localPlayerN].volcanoReady = true; // localplayer knows who is first to volcano

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("votevolcano:yes")) {

					int t;
					int.TryParse (arg [2], out t);
					playerList [t].endGameVote = 1;

				}

				// setturn:<playerturn>
				else if (command.StartsWith ("votevolcano:no")) {

					int t;
					int.TryParse (arg [2], out t);
					playerList [t].endGameVote = -1;

				}

				// setturn:<playerturn>
				else if (command.StartsWith ("votereset:yes")) {

					int t;
					int.TryParse (arg [2], out t);
					playerList [t].endGameVote = 1;

				}



				// report:<player>:<life>:<work>... etc...
				else if (command.StartsWith ("report:")) {

					int player;
					int test;
					int work;
					int school;
					int gompa;
					int guru;
					int volcano;
					int build;
					int def;
					int v, y, n, t;
					int mone;
					int immune;
					int.TryParse (arg [1], out player);

					int.TryParse (arg [2], out test);
					int.TryParse (arg [3], out work);
					int.TryParse (arg [4], out school);
					int.TryParse (arg [5], out gompa);
					int.TryParse (arg [6], out guru);
					int.TryParse (arg [7], out volcano);
					int.TryParse (arg [8], out build);
					int.TryParse (arg [9], out mone);
					int.TryParse (arg [10], out v);
					int.TryParse (arg [11], out y);
					int.TryParse (arg [12], out n);
					int.TryParse (arg [13], out t);
					int.TryParse (arg [14], out def);
					int.TryParse (arg [15], out immune);

					notMyTurnController.turnReport (player, test == 1, work == 1, school == 1, gompa == 1, guru == 1, volcano == 1, build == 1,
						mone == 1, 
						v == 1, y == 1, n == 1, t == 1, def == 1, immune == 1);

				}

				//
				else if (command.StartsWith ("userrepeated")) {
					int pl;
					int.TryParse (arg [1], out pl);
					//joinNewGameController_multi.showRepeatedUser (pl);
					//createNewGameController_multi.showRepeatedUser (pl);
				}

				// acquire secondary wisdoms
				else if (command.StartsWith ("secondarywisdoms")) {

					int pl;
					int.TryParse (arg [1], out pl);
					playerList [pl].addSecondaryWisdoms ();

				}

//				// summon life test on other player
//				else if (command.StartsWith ("summon:")) {
//
//					int sbio, sdria, q, sp;
//					int.TryParse (arg [1], out sbio);
//					int.TryParse (arg [2], out sdria);
//					int.TryParse (arg [3], out q);
//					int.TryParse (arg [4], out sp);
//					lifeTestController.startSummonLifeTest (sbio, sdria, q, sp);
//
//				}

				// summon life test on other player
				else if (command.StartsWith ("lifetestvote:")) {

					int who, what;
					int.TryParse (arg [1], out who);
					int.TryParse (arg [2], out what);
					//lifeTestController.votationController.networkClickOnValue(who, what);
					lifeTestController.resultController.receiveVote (who, what);

				}

				// summon life test votation on other player
				else if (command.StartsWith ("startlifetestvote")) {

					int whichClass, individual;
					int.TryParse (arg [1], out whichClass);
					int.TryParse (arg [2], out individual);

					lifeTestVoteController.startLifeTestVote (whichClass, individual);

				}



				// summon life test votation on other player
				else if (command.StartsWith ("endlifetestvote")) {

					lifeTestVoteController.endLifeTestVote ();

				}

				// summon life test votation on other player
				else if (command.StartsWith ("looseseed")) {

					lifeTestVoteController.looseSeed ();

				}

				// summon life test on other player
				else if (command.StartsWith ("nonturnlifetest:")) {

					int sabid;
					int sabio;
					int quest;
					int.TryParse (arg [1], out sabid);
					int.TryParse (arg [2], out sabio);
					int.TryParse (arg [3], out quest);

					lifeTestController.startNonTurnLifeTest (sabid, sabio, quest);

				}

				// summon life test on other player
				else if (command.StartsWith ("summonres:")) {

					int res;
					int.TryParse (arg [1], out res);
					//lifeTestController.startSummonLifeTest (sbio, sdria, q, sp);
					lastChanceController.summonRes (res);

				}

				// cofirm end of turn OK
				else if (command.StartsWith ("confirm:yes")) {

					int pl;
					int.TryParse (arg [2], out pl);
					PlayerTurnVotation [pl] = 1;

				} else if (command.StartsWith ("seedtoplayer")) {
					seedToPlayerController.startSeedToPlayerActivity ();
				}

				// cofirm end of turn NO
				else if (command.StartsWith ("confirm:no")) {

					int pl;
					int.TryParse (arg [2], out pl);
					PlayerTurnVotation [pl] = -1;

				}

				//
				if (command.StartsWith ("takeplayer")) {
					int np;
					int.TryParse (arg [1], out np);
					int who;
					int.TryParse (arg [2], out who);
					playerSelectController.takePlayer (np, who);
				}
				//
				else if (command.StartsWith ("donttakeplayer")) {
					playerSelectController.dontTakePlayer ();
				}
				if (command.StartsWith ("grabplayer")) {
					int pl;
					int.TryParse (arg [1], out pl);
					int who;
					int.TryParse (arg [2], out who);
					attemptGrabPlayer (pl, who);
				}

				//
				else if (command.StartsWith ("showgallery")) {
					rankingCanvas.SetActive (false);
					dossierCanvas.SetActive (false);
					notMyTurnCanvas.SetActive (false);
					galleryCanvas.SetActive (true);
					galleryCanvas.GetComponentInChildren<GalleryController> ().show ();
				}


				// set secondary hero owner
				//   setheroowner:<hero>:<player>
				else if (command.StartsWith ("setheroowner")) {

					int h;
					int pl;
					int.TryParse (arg [1], out h);
					int.TryParse (arg [2], out pl);
					if (h == 0)
						warriorOwner = pl;
					if (h == 1)
						explorerOwner = pl;
					if (h == 2)
						wizardOwner = pl;

				}

				// playerreconnect:<playerturn>
				//	request from a player that has previously lost it's connection to recover it's data
				else if (command.StartsWith ("playerreconnect")) {

//					int player = playerNFromLogin (arg [1]);
//					if (player == -1)
//						return; // some error, do nothing
//
//
//
//					++nPlayers; // one player less
//					state = 100;

				}


				// setturn:<playerturn>
				else if (command.StartsWith ("playerdisconnect")) {

					//if ((arg [1] == null) || arg [1].Equals ("<null>"))
					//	return; // just ignore these
				
					//playerDisconnectInterface.SetActive (true);
					//playerDisconnectText.text = "El jugador " + arg [1] + " se ha desconectado del servidor";
					// show qr code with session info for recovery
					//globalQRSessionInfoImage.texture = qrEncodedSessionInfo;
					//--nPlayers; // one player less
					//if (nPlayers == 1) {
					//	dismissPlayerText.enabled = false; // can't just dismiss, can't play alone!
					//}
					//state = 100;

				}


//				// synch:<player>:<turn>
//				// player <player> declares has updated it's state to turn <turn>
//				else if (command.StartsWith ("synch")) {
//
//
//
//				}
//
				// synch:<player>:<turn>
				// player <player> declares has updated it's state to turn <turn>
				else if (command.StartsWith ("nuke")) {



						masterController.hardReset ();



				}

				// synch:<player>:<turn>
				// player <player> declares has updated it's state to turn <turn>
				else if (command.StartsWith ("polo")) {

					String aaa = ":3";

					if (noConnectionIcon_N != null) {
						noConnectionIcon_N.keepAlive ();
					}

					networkAgent.poloElapsedTime = 0.0f;

				}



				// addgold:<player>:<amount>
				else if (command.StartsWith ("addgold")) {

					int p; 
					int a;
					int.TryParse (arg [1], out p);
					int.TryParse (arg [2], out a);
					playerList [p].gold += a;


				}

				// cleargold:<player>:<amount>
				else if (command.StartsWith ("cleargold")) {

					int p; 
					int a;
					int.TryParse (arg [1], out p);
					int.TryParse (arg [2], out a);
					playerList [p].gold = 0;


				}


				else if (command.StartsWith ("roomuuid")) { // hard coded
					if (!continueGameController_multi.tryingToContinue) {
						localUserLogin = arg [1];
					}
				}

				else if (command.StartsWith ("reportcontinue")) {
					int otherUser;
					int.TryParse (arg [1], out otherUser);
					int ttl;
					int.TryParse (arg [3], out ttl);
					continueGameController_multi.ReportContinue (otherUser, arg [2], ttl);

				}

				if (command.StartsWith ("roomplayers")) {

					int id = 1;
					while (!arg [id].Equals ("null")) {
						otherPlayersText.text += (arg [id] + "\n");
						int pid;
						int.TryParse (arg [id], out pid);
						networkAgent.receiveSeqFor (pid); // register roommate for broadcast
						id++;

					}

				}
				if (command.StartsWith ("ACK")) {

					//string[] fields = newData.Split (':');
					int sq;
					int.TryParse (arg [1], out sq);
					string fromUser = arg [2].TrimEnd ('$', '\n');
					int fromUserId;
					int.TryParse (fromUser, out fromUserId);
					networkAgent.ack (sq, fromUserId);
					if (fromUser.IndexOf ('$') != -1) {
						//Debug.Log ("STRANGE ORIGIN:  " + fromUser);
					}


				}
				if (command.StartsWith ("polo")) {
					networkAgent.poloElapsedTime = 0.0f;
				}

				// just for fun
				else if (command.StartsWith ("ding")) {
					

						masterController.playSound (dingSound);

				}

				if (safeCommand) { // if safe command, acknowledge processing

					networkAgent.incReceiveSeqFor (originOfSafeCommand);
					networkAgent.sendCommandUnsafe (originOfSafeCommand, "ACK:" + safeSeqNum + ":" + localUserLogin);



				}
			}

		}

		// clear the dirty array!!!
		for (int i = 0; i < commands.Length; ++i) {
			commands [i] = "";
		}
			

	}


	int playerNFromLogin(string login) {

		for(int i = 0; i<MaxPlayers; ++i) {
			if (playerList [i].login.Equals (login)) {
				return i;
			}
		}
		return -1;

	}

	public void setPlayerLogin(int playern, string login) {
		playerList [playern].login = login;
	}


	/* behaviour methods */

	// Update is called once per frame
	float remainingTimeToShowSendData = 2.0f;
	void Update () {


		if (Input.GetKeyDown (KeyCode.J)) {
			for (int i = 0; i < nPlayers; i++) {
				addGold (i, 500);
			}
		}

		// manage send data icon
		if (networkAgent.sendList.Count > 0) {
			if (remainingTimeToShowSendData > 0.0f) {
				remainingTimeToShowSendData -= Time.deltaTime;
				if (remainingTimeToShowSendData <= 0.0f) {
					networkAgent.showSendDataIcon ();
				}
			}
		} else {
			if (remainingTimeToShowSendData <= 0.0f) {
				networkAgent.hideSendDataIcon ();
			}
			remainingTimeToShowSendData = 2.0f;
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			
		}
		debugText.text = "nPlayers: " + nPlayers + "\n" +
						"This player: " + localPlayerN + "\n" +
						"This turn: " + playerTurn + "\n" +
			"PP[0] = " + playerPresent[0] + "\nPP[1] = " + playerPresent[1] + 
			"\nPP[2] = " + playerPresent[2] + "\nPP[3] = " + playerPresent[3] + 
			"\nState = " + state;

		if (state == 0) { // idle

		}

		/*
		 * 
		 * One of the players has been disconnected
		 * 
		 */
		if (state == 100) {
			/*timer += Time.deltaTime;
			if (timer > 3.0f) {
				masterController.startActivity = "ResetGame";
				globalFader.fadeOutTask (this);
				state = 101;
			}*/
		}
		if (state == 101) { // waiting for fadeout
			if (!isWaitingForTaskToComplete) {
				networkAgent.disconnect ();
				SceneManager.LoadScene ("Scenes/Master");
				state = 0;
			}
		}

		/*
		if (state == 200) { // trying to reconnect
			serverDisconnectInterface.SetActive(true);
			state = 201;
			timer = 0.0f;
		}
		if (state == 201) {
			timer += Time.deltaTime;
			if (timer > reconnectRetryInterval) {
				timer = 0.0f;
				if (networkAgent.connect () == 0) {
					state = 0; // stop trying to reconnect
					serverDisconnectInterface.SetActive(false);
					if (gameState < 2) {
						SceneManager.LoadScene ("Scenes/Master"); // reset the entire game
					} else {
						// if the game has actually started, each player knows his/her own login and roomname, 
						//  so we can just re-hook into the server, and hope for the best!
						network_initGame (gameRoom); 
					}
				};
			}
		}

		*/

		/*
		 * 
		 * A player is attempting to end the game
		 * 
		 */

		if (tryingToEndGame) {
			int n = 0;
			for (int i = 0; i < playerList.Count; ++i) {
				if (playerList [i].endGameVote == 1)
					++n;
			}

			if (n == nPlayers) {
				// finish the game

			}

		}

		if (tryingToResetGame) {
			int n = 0;
			for (int i = 0; i < playerList.Count; ++i) {
				if (playerList [i].resetGameVote == 1)
					++n;
			}

			if (n == nPlayers) {
				// finish the game
				masterController.startActivity = "ResetGame";
				globalFader.fadeOutTask (this);
				state = 101; // wait for fadeout before resetting the game
			}

		}

		if (tryingToDismissPlayer) {
			int n = 0;
			for (int i = 0; i < playerList.Count; ++i) {
				if (playerList [i].resetGameVote == 1)
					++n;
			}

			if (n == nPlayers) {
				// dismiss the player and resume
				playerDisconnectInterface.SetActive(false);
			}

		}


	}





	/* callback methods */

		// these two callbacks are 'possibly' called from player disconnection dialogue 
	public void dismissPlayerCallback() {

		// vote for game resetting
		playerList[localPlayerN].dismissPlayerVote = 1;
		// tell others our intention
		networkAgent.broadcast ("votedismiss:yes:" + localPlayerN + ":"); // note: dismissing a player
																	 // is just resuming, freeing the dismissed player resources

	}

	public void resetGameCallback() {

		// vote for game resetting
		playerList[localPlayerN].resetGameVote = 1;
		// tell others our intention
		networkAgent.broadcast ("votereset:yes:" + localPlayerN + ":");
		tryingToResetGame = true;

	}

	public void endGameAgreeYes() {

		playerList [localPlayerN].endGameVote = 1;
		networkAgent.broadcast ("votevolcano:yes:" + localPlayerN + ":");

	}

	public void endGameAgreeNo() {

		playerList [localPlayerN].endGameVote = -1;
		networkAgent.broadcast ("votevolcano:no:" + localPlayerN + ":");

	}

	public void resetQuickSaveInfo() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/quicksaveinfo.dat", FileMode.Create);

		QuickSaveInfo quickSaveInfo_ = new QuickSaveInfo ();
		quickSaveInfo_.numberOfPlayers = 0;
		quickSaveInfo_.roomId = "";
		quickSaveInfo_.randomChallenge = "";
		quickSaveInfo_.turn = 0;
		quickSaveInfo_.datetime = "";
		quickSaveInfo_.remainingFreePlays = quickSaveInfo.remainingFreePlays;
		quickSaveInfo_.playcode = quickSaveInfo.playcode;
		quickSaveInfo_.currentTranslation = quickSaveInfo.currentTranslation;

		formatter.Serialize (file, quickSaveInfo_);
		file.Close ();

	}

	public bool checkQuickSaveInfo() {

		if (File.Exists (Application.persistentDataPath + "/quicksaveinfo.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/quicksaveinfo.dat", FileMode.Open);
			QuickSaveInfo data = (QuickSaveInfo)formatter.Deserialize (file);
			quickSaveInfo = data;
			Debug.Log ("Loading localUserLogin : " + data.login);
			gameHost = data.master;
			gameRoom = data.roomId;
			localUserLogin = data.login;
			Debug.Log ("<color=green>Remaining: " + quickSaveInfo.remainingFreePlays + "</color>");
			file.Close ();
			return (quickSaveInfo.numberOfPlayers != 0);

		} else {
			quickSaveInfo = new QuickSaveInfo ();
			Debug.Log ("<color=red>New Quick Save Info </color>");
			return false;
		}

	}

	public void saveQuickSaveInfo() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/quicksaveinfo.dat", FileMode.Create);

		QuickSaveInfo quickSaveInfo_ = new QuickSaveInfo ();
		quickSaveInfo_.numberOfPlayers = nPlayers;
		quickSaveInfo_.roomId = gameRoom;
		quickSaveInfo_.randomChallenge = randomChallenge;
		quickSaveInfo_.turn = turn;
		Debug.Log ("Saving login : " + localUserLogin);
		quickSaveInfo_.login = localUserLogin;
		quickSaveInfo_.master = gameHost;
		quickSaveInfo_.currentTranslation = quickSaveInfo.currentTranslation;
		quickSaveInfo_.datetime = datetimeOfGame;
		quickSaveInfo_.remainingFreePlays = quickSaveInfo.remainingFreePlays;
		quickSaveInfo_.playcode = quickSaveInfo.playcode;

		formatter.Serialize (file, quickSaveInfo_);
		file.Close ();


	}

	public void noConnectionIconSetEnabled(bool en) {
		if (noConnectionIcon_N != null) {
			if (en) {
				noConnectionIcon_N.Start ();
				noConnectionIcon_N.go ();
			} else {
				noConnectionIcon_N.Start ();
				noConnectionIcon_N.stop ();
			}
		}
	}

	// saveload methods
	public bool loadQuickSaveData() {

		if (File.Exists (Application.persistentDataPath + "/quicksavedata.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/quicksavedata.dat", FileMode.Open);
			QuickSaveData data = (QuickSaveData)formatter.Deserialize (file);


			//turnBackup = data.turnBackup;
			for (int i = 0; i < data.forestOwner.Count; ++i) {
				forestOwner [i] = data.forestOwner [i];
			}
			dismissedTips = data.dismissedTips;
			forestProperty = data.forestProperty;
			guruPickedOrange = data.guruPickedOrange;
			guruPickedYellow = data.guruPickedYellow;
			guruPickedRed = data.guruPickedRed;
			guruPickedPurple = data.guruPickedPurple;
			guruPickedGreen = data.guruPickedGreen;
			guruPickedBlue = data.guruPickedBlue;
			guruPickedBrown = data.guruPickedBrown;
			playerList = data.playerList;
			playerPresent = data.playerPresent;
			creativityAwardedPlayer = data.creativityAwardedPlayer;
			empathyAwardedPlayer = data.empathyAwardedPlayer;
			turn = data.turn;
			playerTurn = data.playerTurn;
			isMaster = data.isMaster;
			nPlayers = data.nPlayers;
			reportedPlayers = data.reportedPlayers;
			awardedPlayers = data.awardedPlayers;
			localUserLogin = data.localUserLogin;
			localUserPass = data.localUserPass;
			localPlayerN = data.localPlayerN;
			warriorOwner = data.warriorOwner;
			explorerOwner = data.explorerOwner;
			wizardOwner = data.wizardOwner;
			creativityAwardPlayer = data.creativityAwardPlayer;
			empathyAwardPlayer = data.empathyAwardPlayer;
			turnVotesOK = data.turnVotesOK;
			turnVotesNO = data.turnVotesNO;
			PlayerTurnVotation = data.PlayerTurnVotation;
			notificationList = data.notificationList;
			gameRoom = data.gameRoom;
			gameHost = data.gameHost;
			syncedPlayers = data.syncedPlayers;
			creativityVotes = data.creativityVotes;
			empathyVotes = data.empathyVotes;

		
			file.Close ();
			return true;

		} else 
		return false;

	}

	// saveload methods
	public bool loadTipSaveData() {

		if (File.Exists (Application.persistentDataPath + "/tipsavedata.dat")) {

			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/tipsavedata.dat", FileMode.Open);
			TipSaveData data = (TipSaveData)formatter.Deserialize (file);

			tipSaveData = data;

			file.Close ();
			return true;

		} else {
			tipSaveData = new TipSaveData ();
			tipSaveData.showTips = true;
			tipSaveData.dismissedTips = new List<string> ();
			return false;
		}

	}
	// saveload methods
	public bool saveTipSaveData() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/tipsavedata.dat", FileMode.Create);

		formatter.Serialize (file, tipSaveData);
		file.Close ();

		return true;

	}

	public void saveQuickSaveData() {

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open(Application.persistentDataPath + "/quicksavedata.dat", FileMode.Create);

		QuickSaveData data = new QuickSaveData ();

		//data.turnBackup = turnBackup;
		data.forestOwner = new List<int>();
		for (int i = 0; i < forestOwner.Length; ++i) {
			data.forestOwner.Add (forestOwner [i]);
		}
		data.dismissedTips = dismissedTips;
		data.forestProperty = forestProperty;
		data.guruPickedOrange = guruPickedOrange;
		data.guruPickedYellow = guruPickedYellow;
		data.guruPickedRed = guruPickedRed;
		data.guruPickedPurple = guruPickedPurple;
		data.guruPickedGreen = guruPickedGreen;
		data.guruPickedBlue = guruPickedBlue;
		data.guruPickedBrown = guruPickedBrown;
		data.playerList = playerList;
		data.playerPresent = playerPresent;
		data.creativityAwardedPlayer = creativityAwardedPlayer;
		data.empathyAwardedPlayer = empathyAwardedPlayer;
		data.turn = turn;
		data.playerTurn = playerTurn;
		data.isMaster = isMaster;
		data.nPlayers = nPlayers;
		data.reportedPlayers = reportedPlayers;
		data.awardedPlayers = awardedPlayers;
		data.localUserLogin = localUserLogin;
		data.localUserPass = localUserPass;
		data.localPlayerN = localPlayerN;
		data.warriorOwner = warriorOwner;
		data.explorerOwner = explorerOwner;
		data.wizardOwner = wizardOwner;
		data.creativityAwardPlayer = creativityAwardPlayer;
		data.empathyAwardPlayer = empathyAwardPlayer;
		data.turnVotesOK = turnVotesOK;
		data.turnVotesNO = turnVotesNO;
		data.PlayerTurnVotation = PlayerTurnVotation;
		data.notificationList = notificationList;
		data.gameRoom = gameRoom;
		data.gameHost = gameHost;
		data.syncedPlayers = syncedPlayers;
		data.creativityVotes = creativityVotes;
		data.empathyVotes = empathyVotes;

		formatter.Serialize (file, data);
		file.Close ();

	}

}

[Serializable]
class SaveData {

	public string currentLogin;
	public string currentPass;
	public string currentEMail;

}

[Serializable]
public class MakerGame {
	public string code;
	public string level;
	public string desc;
}

[Serializable]
public class MakerGameData {
	public List<MakerGame> games;
}

[Serializable]
public class TipSaveData {
	public bool showTips;
	public List<string> dismissedTips;
}

[Serializable]
public class QuickSaveInfo {
	

	public int turn;
	public int numberOfPlayers; // 0 means NO quicksaveinfo
	public int remainingFreePlays;
	public string roomId;
	public string randomChallenge;
	public string login;
	public string datetime;
	public string master;
	public string playcode;
	public int currentTranslation;
	public string myServerNetworkAddress;
	public string myNetworkAddress;


	public QuickSaveInfo() {
		roomId = "";
		randomChallenge = "";
		login = "";
		datetime = "";
		master = "";
		playcode = "";
		remainingFreePlays = 2;
		currentTranslation = 0;
	}

}

[Serializable]
public class QuickSaveData {

	//public TurnBackup turnBackup;
	public List<string> dismissedTips;
	public List <int> forestProperty;
	public bool guruPickedGreen;
	public bool guruPickedYellow;
	public bool guruPickedRed;
	public bool guruPickedOrange;
	public bool guruPickedPurple;
	public bool guruPickedBrown;
	public bool guruPickedBlue;
	public List<Player> playerList;
	public List<bool> playerPresent;
	public int creativityAwardedPlayer;
	public int empathyAwardedPlayer;
	public int turn;
	public int playerTurn;
	public bool isMaster;
	public int nPlayers;
	public int reportedPlayers;
	public int awardedPlayers;
	public string localUserLogin;
	public string localUserPass;
	public int localPlayerN;
	public int warriorOwner;
	public int explorerOwner;
	public int wizardOwner;
	public int creativityAwardPlayer;
	public int empathyAwardPlayer;
	public int turnVotesOK;
	public int turnVotesNO;
	public List<int> PlayerTurnVotation;
	public List<string> notificationList;
	public string gameRoom;
	public string gameHost;
	public int syncedPlayers;
	public int creativityVotes;
	public int empathyVotes;
	public List<int>forestOwner;


}

