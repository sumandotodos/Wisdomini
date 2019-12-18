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



// player colors:   0 blue       1 yellow     2 green      3 brown
// forest colors:   0 magenta    1 cyan       2 deepblue   3 orange

public class GameController_mono : Task {



	public GameObject galleryCanvas;

	public GameObject notMyTurnCanvas;

	public TipSaveData tipSaveData;

	public QuickSaveInfo quickSaveInfo;
	public QuickSaveData quickSaveData;

	public GameObject rankingCanvas;
	public GameObject dossierCanvas;

	public string randomChallenge;
	public string datetimeOfGame;


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


	public int warriorOwner = -1;
	public int explorerOwner = -1;
	public int wizardOwner = -1;

	public int creativityAwardPlayer;
	public int empathyAwardPlayer;



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

	public PlayerActivityController_mono playerActivityController;
	public NotMyTurnController_mono notMyTurnController;
	public NotMySchoolController_mono notMyTurnSchoolController;
	public SchoolActivityController_mono myTurnSchoolController;
	public NotMyTurnGuruActivityController_mono notMyTurnGuruActivityController_mono;
	public ValorationController_mono valorationController;
	public LifeTestActivityController_mono lifeTestController;
	public LifeTestVoteActivityController_mono lifeTestVoteController;
	public LastChanceController_mono lastChanceController;
	public SeedToPlayerController_mono seedToPlayerController;
	public GuruActivityController_mono guruActivityController;


	public MasterController_mono masterController;
	public MasterController_kidsmono masterController_kids;

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
		for (int i = 0; i < GameController_mono.MAXFORESTS; ++i) {
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
		for (int i = 0; i < GameController_mono.MAXFORESTS; ++i) {
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

		creativityAwardedPlayer = ValorationController_mono.PlayerNone;
		empathyAwardedPlayer = ValorationController_mono.PlayerNone;

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




		// rotate clockwise
		playerTurn = 0;//(playerTurn + (MaxPlayers - 1)) % MaxPlayers;


		//dossierCanvas.SetActive (false);
		//rankingCanvas.SetActive (false);

	}

	public void setStartActivity(string act) {
		if(masterController!=null)
			masterController.startActivity = act;
		if (masterController_kids != null)
			masterController_kids.startActivity = act;
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
		return ValorationController_mono.PlayerNone;
	}

	// calculate bumis

	public void calculateBumis() {

		for (int p = 0; p < GameController_mono.MaxPlayers; ++p) {
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
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].nInitiations > maxInitiations) {
				playersWithMax = 1;
				maxInitiations = playerList [i].nInitiations;
			} else if (playerList [i].nInitiations == maxInitiations) {
				++playersWithMax;
			}
		}


		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if ((playerList [i].nInitiations == maxInitiations) && (maxInitiations > 0)) {
					playerList [i].totalBumis += (MOSTINITIATIONSSCORE/playersWithMax);
					playerList [i].mostInitiationsScore = (MOSTINITIATIONSSCORE/playersWithMax);
				}
			}
		//}



		for (int p = 0; p < GameController_mono.MaxPlayers; ++p) {

			// get first to volcano player
			if (playerList [p].firstToVolcano == true) {
				playerList [p].totalBumis += FIRSTTOVOLCANOSCORE;
				playerList [p].firstToVolcanoScore = FIRSTTOVOLCANOSCORE;
			}

		}



		int maxHeroes = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].totalHeroes > maxHeroes) {
				playersWithMax = 1;
				maxHeroes = playerList [i].totalHeroes;
			}
			else if (playerList [i].totalHeroes == maxHeroes) {
				++playersWithMax;
			}
		}

		//if(playersWithMax == 1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if ((playerList [i].totalHeroes == maxHeroes) && (maxHeroes > 7)) {
					playerList [i].totalBumis += (MOSTHEROESSCORE/playersWithMax);
					playerList [i].mostHeroesScore = (MOSTHEROESSCORE/playersWithMax);
				}
			}
		//}


		int maxInitiationDifferentClasses = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].initiationsDifferentClasses > maxInitiationDifferentClasses) {
				playersWithMax = 1;
				maxInitiationDifferentClasses = playerList [i].initiationsDifferentClasses;
			} else if (playerList [i].initiationsDifferentClasses == maxInitiationDifferentClasses) {
				++playersWithMax;
			}
		}

		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if ((playerList [i].initiationsDifferentClasses == maxInitiationDifferentClasses) && (maxInitiationDifferentClasses > 0)) {
					playerList [i].totalBumis += (MOSTCLASSESSCORE/playersWithMax);
					playerList [i].mostInitiationClassesScore = (MOSTCLASSESSCORE/playersWithMax);
				}
			}
		//}



		int maxGompas = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].nGompas > maxGompas) {
				playersWithMax = 1;
				maxGompas = playerList [i].nGompas;
			} else if (playerList [i].nGompas == maxGompas) {
				++playersWithMax;
			}
		}

		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if ((playerList [i].nGompas == maxGompas) && (maxGompas > 0)) {
					playerList [i].totalBumis += (MOSTGOMPAS/playersWithMax);
					playerList [i].mostGompasScore = (MOSTGOMPAS/playersWithMax);
				}
			}
		//}


		int maxSchools = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].nSchools > maxSchools) {
				playersWithMax = 1;
				maxSchools = playerList [i].nSchools;
			} else if (playerList [i].nSchools == maxSchools) {
				++playersWithMax;
			}
		}

		//if (playersWithMax == 1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if ((playerList [i].nSchools == maxSchools) && (maxSchools > 0)) {
					playerList [i].totalBumis += (MOSTSCHOOLS/playersWithMax);
					playerList [i].mostSchoolsScore = (MOSTSCHOOLS/playersWithMax);
				}
			}
		//}


		/*int maxCreativity = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].creativityAwards > maxCreativity) {
				playersWithMax = 1;
				maxCreativity = playerList [i].creativityAwards;
			}
			else if (playerList [i].creativityAwards == maxCreativity) {
				++playersWithMax;
			}
		}

		if (playersWithMax==1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
				if (playerList [i].creativityAwards == maxCreativity) {
					playerList [i].totalBumis += CREATIVITYSCORE;
					playerList [i].creativityScore = CREATIVITYSCORE;
				}
			}
		}


		int maxEmpathy = 0;
		playersWithMax = 1;
		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			if (playerList [i].empathyAwards > maxEmpathy) {
				playersWithMax = 1;
				maxEmpathy = playerList [i].empathyAwards;
			}
			else if (playerList [i].empathyAwards == maxEmpathy) {
				++playersWithMax;
			}
		}

		if (playersWithMax == 1) {
			for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
				if (playerList [i].empathyAwards == maxEmpathy) {
					playerList [i].totalBumis += EMPATHYSCORE;
					playerList [i].empathyScore = EMPATHYSCORE;
				}
			}
		}*/




		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
			playerList [i].totalBumis += playerList [i].trainings;
			//playerList [i].bumis = playerList [i].trainings;
		}

		for (int i = 0; i < GameController_mono.MaxPlayers; ++i) {
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






	int playerNFromLogin(string login) {

		for(int i = 0; i<MaxPlayers; ++i) {
			if (playerList [i].login.Equals (login)) {
				return i;
			}
		}
		return -1;

	}


	/* behaviour methods */

	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (KeyCode.J)) {
			for (int i = 0; i < nPlayers; i++) {
				addGold (i, 500);
			}
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
				
				SceneManager.LoadScene ("Scenes/Master");
				state = 0;
			}
		}


		if (state == 200) { // trying to reconnect
			serverDisconnectInterface.SetActive(true);
			state = 201;
			timer = 0.0f;
		}




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
				if(masterController!=null)
				masterController.startActivity = "ResetGame";
				if(masterController_kids!=null)
				masterController_kids.startActivity = "ResetGame";
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

																	 // is just resuming, freeing the dismissed player resources

	}

	public void resetGameCallback() {

		// vote for game resetting
		playerList[localPlayerN].resetGameVote = 1;
		// tell others our intention

		tryingToResetGame = true;

	}

	public void endGameAgreeYes() {

		playerList [localPlayerN].endGameVote = 1;


	}

	public void endGameAgreeNo() {

		playerList [localPlayerN].endGameVote = -1;


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


		formatter.Serialize (file, quickSaveInfo);
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
			file.Close ();
			return (quickSaveInfo.numberOfPlayers != 0);

		} else {
			quickSaveInfo = new QuickSaveInfo ();
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
