using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;


[System.Serializable]
public struct Initiation {

	public int heroClass;
	public int heroIndividual;

};

[System.Serializable]
public class Player  {


	/* data that characterizes the player */

	public int color; // 0 to 3
	public int gold; 
	//public TextMesh text;
	public string login = "";
	public int bumis;
	public int id;
	public int seeds;
	public int nGompas;
	public int nSchools;
	public List<int> initiations;
	public int initiationsDifferentClasses;
	public List<int> heroAmount;
	public List<int> wisdoms;
	public bool firstToVolcano;
	public int totalHeroes;
	public int mainWisdom;
	public bool hasSecondaryWisdoms;
	public int nInitiations;

	public int mostInitiationsScore;
	public int firstToVolcanoScore;
	public int mostHeroesScore;
	public int mostInitiationClassesScore;
	public int mostGompasScore;
	public int mostSchoolsScore;
	public int creativityScore;
	public int empathyScore;
	public int trainings;

	public int endGameVote; // -1 no   0 not voted   1 yes
	public int dismissPlayerVote; // -1 no   0 not voted   1 yes
	public int resetGameVote;
	public int bulbVote; // who are we voting
	public int handVote; // who are we voting
	public int creativityAwards;
	public int empathyAwards;
	public int totalBumis;
	public bool volcanoReady; // used to wait until network commands have been processed

	public int spentImmunity = 0;
	public const int nHints = 3;
	public bool[] hintUsed;
	public const int nGreenOrbs = 1;
	public const int nRedOrbs = 2;
	public bool[] orbUsed;

	int sessId; // id for current session

	int state0;
	int substate0;
	float timer0;

	public void reset() 
	{
		hintUsed = new bool[nHints];
		orbUsed = new bool[nRedOrbs + nGreenOrbs];
		for (int i = 0; i < nHints; ++i)
			hintUsed [i] = false;
		for (int i = 0; i < (nRedOrbs + nGreenOrbs); ++i)
			orbUsed [i] = false;
		endGameVote = resetGameVote = dismissPlayerVote = 0;
		bumis = 0;
		totalBumis = 0;
		id = -1;
		firstToVolcano = false;
		seeds = 0;
		totalHeroes = 7; // WEJE
		initiationsDifferentClasses = 0;
		trainings = 0;
		initiations = new List<int> ();
		for (int i = 0; i < 7; ++i) {
			initiations.Add (0);
		}
		nInitiations = 0;
		heroAmount = new List<int> ();
		for(int i = 0; i < 7; ++i) {
			heroAmount.Add(1);
		}
		wisdoms = new List<int> ();
		mainWisdom = -1; // no wisdom, boo..
		hasSecondaryWisdoms = false;
		nGompas = 0;
		nSchools = 0;
		bulbVote = -1; // vote nobody
		handVote = -1; // vote nobody
		creativityAwards = 0;
		empathyAwards = 0;
		volcanoReady = false;
		login = "";
	}

	public int numberOfWisdoms() 
	{
		return wisdoms.Count;
	}

	public void setMainWisdom(int w)
	{
		mainWisdom = w;
	}

	public void addWisdom(int w)
	{
		wisdoms.Add (w);
	}

	public void addSecondaryWisdoms() 
	{
		hasSecondaryWisdoms = true;
		wisdoms.Add (GameController_multi.WARRIOR);
		wisdoms.Add (GameController_multi.WIZARD);
		wisdoms.Add (GameController_multi.EXPLORER);
	}

	public Player()
	{
		reset ();		
	}

	public bool hasWisdom(int w) 
	{
		return wisdoms.Contains (w);
	}

	public void addTraining() 
	{
		++trainings;
	}

	public void addSeed() {
		++seeds;
	}

	public void addSeeds(int s) {
		seeds += s;
	}

	public void addHero(int heroClass, int amount) 
	{
		for (int i = 0; i < amount; ++i)
			addHero (heroClass);
	}

	public void addHero(int heroClass) 
	{
		if (GameController_multi.heroMaxAmount (heroClass) > heroAmount [heroClass]) {
			heroAmount [heroClass]++;
			++totalHeroes;
		}
	}

	public void addInitiation(int heroClass) 
	{
		bool newClass = initiations [heroClass] == 0;

		initiations [heroClass]++;

		if (newClass == true)
			++initiationsDifferentClasses;

		++nInitiations;
	}

	public void backup(Player otherPlayer) 
	{
		otherPlayer.color = color;
		otherPlayer.gold = gold;
		otherPlayer.login = login;
		otherPlayer.bumis = bumis;
		otherPlayer.seeds = seeds;
		otherPlayer.nGompas = nGompas;
		otherPlayer.nSchools = nSchools;
		otherPlayer.initiations = new List<int>(initiations);
		otherPlayer.initiationsDifferentClasses = initiationsDifferentClasses;
		otherPlayer.heroAmount = new List<int> (heroAmount);
		otherPlayer.firstToVolcano = firstToVolcano;
		otherPlayer.totalHeroes = totalHeroes;
		otherPlayer.mainWisdom = mainWisdom;
		otherPlayer.hasSecondaryWisdoms = hasSecondaryWisdoms;
		otherPlayer.nInitiations = nInitiations;
		otherPlayer.mostInitiationsScore = mostInitiationsScore;
		otherPlayer.firstToVolcanoScore = firstToVolcanoScore;
		otherPlayer.mostHeroesScore = mostHeroesScore;
		otherPlayer.mostInitiationClassesScore = mostInitiationClassesScore;
		otherPlayer.mostGompasScore = mostGompasScore;
		otherPlayer.mostSchoolsScore = mostSchoolsScore;
		otherPlayer.wisdoms = wisdoms;
		otherPlayer.trainings = trainings;

		for (int i = 0; i < nHints; ++i)
			otherPlayer.hintUsed [i] = hintUsed[i];

		for (int i = 0; i < (nRedOrbs + nGreenOrbs); ++i)
			otherPlayer.orbUsed [i] = orbUsed[i];
		

		otherPlayer.endGameVote = endGameVote;
		otherPlayer.dismissPlayerVote = dismissPlayerVote;
		otherPlayer.resetGameVote = resetGameVote;

		otherPlayer.totalBumis = totalBumis;
		otherPlayer.volcanoReady = volcanoReady;

		otherPlayer.sessId = sessId;
		/*	

	public int creativityScore;
	public int empathyScore;

	public int endGameVote; // -1 no   0 not voted   1 yes
	public int dismissPlayerVote; // -1 no   0 not voted   1 yes
	public int resetGameVote;
	public int bulbVote; // who are we voting
	public int handVote; // who are we voting
	public int creativityAwards;
	public int empathyAwards;
	public int totalBumis;
	public bool volcanoReady; // used to wait until network commands have been processed

	int sessId; // id for current session

	int state0;
	int substate0;
	float timer0;*/
	}
}

