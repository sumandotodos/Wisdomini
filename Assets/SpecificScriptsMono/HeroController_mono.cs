using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroController_mono : Task {

	public CircpleDeploy[] hero;

	public UIImageBlinker batsu;
	public UIImageBlinker maru;

	public Texture[] warriorImages;
	public Texture[] warriorIndividuals;
	public Texture[] masterImages;
	public Texture[] masterIndividuals;
	public Texture[] philosopherImages;
	public Texture[] philosopherIndividuals;
	public Texture[] sageImages;
	public Texture[] sageIndividuals;
	public Texture[] explorerImages;
	public Texture[] explorerIndividuals;
	public Texture[] wizardImages;
	public Texture[] wizardIndividuals;
	public Texture[] yogiImages;
	public Texture[] yogiIndividuals;

	public StringBank heroesNames;
	public StringBank heroesDesc;

	public StringBank warriorNames;
	public StringBank warriorAliases;
	public StringBank warriorDesc;

	public StringBank masterNames;
	public StringBank masterAliases;
	public StringBank masterDesc;

	public StringBank philosopherNames;
	public StringBank philosopherAliases;
	public StringBank philosopherDesc;

	public StringBank sageNames;
	public StringBank sageAliases;
	public StringBank sageDesc;

	public StringBank explorerNames;
	public StringBank explorerAliases;
	public StringBank explorerDesc;

	public StringBank wizardNames;
	public StringBank wizardAliases;
	public StringBank wizardDesc;

	public StringBank yogiNames;
	public StringBank yogiAliases;
	public StringBank yogiDesc;

	public RosettaWrapper rosettaWrap;


	public StringBank currentHeroNames;
	public StringBank currentHeroDescs;

	Texture[] currentImages;

	public int nElements;

	public void setNElements(int n) {
		nElements = n;
		for (int i = 0; i < nElements; ++i) {
			hero [i].setIndex (i);
			hero [i].setNElements (nElements);
			hero [i].gameObject.GetComponentInChildren<RawImage> ().enabled = true;
		}
		for (int i = nElements; i < hero.Length; ++i) {
			hero [i].gameObject.GetComponentInChildren<RawImage> ().enabled = false;
		}
	}



	public void extendTask(Task w) {
		hero [0].extendTask (w);
		for (int i = 1; i < nElements; ++i) {
			hero [i].extend ();
		}
	}

	public void extend() {

		for (int i = 0; i < nElements; ++i) {
			hero [i].extend (); 
		}

	}

	public void extend(int id) {
		hero [id].extend ();
	}
	public void retract(int id) {
		hero [id].retract ();
	}

	public void retractTask(Task w) {
		hero [0].retractTask (w);
		for (int i = 1; i < nElements; ++i) {
			hero [i].retract ();
		}
	}

	public void retract() {

		for (int i = 0; i < nElements; ++i) {
			hero [i].retract ();
		}

	}

	public void retrieveTextureAndNameFromClass(int c, out string name, out Texture image) {
		name = "";
		image = null;
		switch (c) {
		case 0:
			image = warriorImages [0];
			name = "Guerreros pacíficos";
			break;
		case 1:
			image = masterImages [0];
			name = "Maestras inmutables";
			break;
		case 4:
			image = explorerImages [0];
			name = "Exploradoras de la auto-aceptación";
			break;
		case 5:
			image = wizardImages [0];
			name = "Magas del corazón";
			break;
			
		}
	}

	public void retrieveTextureAndNameFromIndiv(int c, int i, out string name, out Texture image) {
		name = "";
		image = null;
		switch (c) {
		case 0:
			image = warriorIndividuals [i];
			warriorNames.rosetta = rosettaWrap.rosetta;
			name = warriorNames.getString (i);
			break;
		case 1:
			image = masterIndividuals [i];
			masterNames.rosetta = rosettaWrap.rosetta;
			name = masterNames.getString (i);
			break;
		case 4:
			image = explorerIndividuals [i];
			explorerNames.rosetta = rosettaWrap.rosetta;
			name = explorerNames.getString (i);
			break;
		case 5:
			image = wizardIndividuals [i];
			wizardNames.rosetta = rosettaWrap.rosetta;
			name = wizardNames.getString (i);
			break;

		}
	}

	public int setUpHeroClass(int h) {

		int nIndivs;

		Texture[] heroImages;
		Texture[] individualImages;
		StringBank aliases = warriorAliases;

		individualImages = warriorIndividuals;

		if (h == 0) { // warriors :3
			heroImages = warriorImages;
			individualImages = warriorIndividuals;
			aliases = warriorAliases;
			currentHeroDescs = warriorDesc;
			currentHeroNames = warriorNames;
		}
		else if (h == 1) { // masters 
			heroImages = masterImages;
			individualImages = masterIndividuals;
			aliases = masterAliases;
			currentHeroDescs = masterDesc;
			currentHeroNames = masterNames;
		}
		else if (h == 2) { // philosophers 
			heroImages = philosopherImages;
			individualImages = philosopherIndividuals;
			aliases = philosopherAliases;
			currentHeroDescs = philosopherDesc;
			currentHeroNames = philosopherNames;
		}
		else if (h == 3) { // sages
			heroImages = sageImages;
			individualImages = sageIndividuals;
			aliases = sageAliases;
			currentHeroDescs = sageDesc;
			currentHeroNames = sageNames;
		}
		else if (h == 4) { // explorers
			heroImages = explorerImages;
			individualImages = explorerIndividuals;
			aliases = explorerAliases;
			currentHeroDescs = explorerDesc;
			currentHeroNames = explorerNames;
		}
		else if (h == 5) { // wizards
			heroImages = wizardImages;
			individualImages = wizardIndividuals;
			aliases = wizardAliases;
			currentHeroDescs = wizardDesc;
			currentHeroNames = wizardNames;
		}
		else if (h == 6) { // yogis 
			heroImages = yogiImages;
			individualImages = yogiIndividuals;
			aliases = yogiAliases;
			currentHeroDescs = yogiDesc;
			currentHeroNames = yogiNames;
		}

		aliases.rosetta = rosettaWrap.rosetta;

		nIndivs = individualImages.Length;
		setNElements (individualImages.Length);
		for (int i = 0; i < individualImages.Length; ++i) {
			hero [i].gameObject.GetComponentInChildren<Text> ().enabled = true;
			hero [i].gameObject.GetComponentInChildren<Text> ().text = aliases.getString (i);
			for(int j = 0; j<6; ++j) {
				hero [i].gameObject.GetComponentInChildren<TextureAnimator> ().images [j] = individualImages [i];
				hero [i].gameObject.GetComponentInChildren<RawImage> ().texture = individualImages [i];
			}
		}

		return nIndivs;


	}

	public void setUpHeroes() { 
		for (int i = 0; i < 7; ++i) {
			switch(i) {
			case 0:
				currentImages = warriorImages; // 0 war  1 master   2 phil   3 sages   4 expl   5 wiz   6 yog
				break;
			case 1:
				currentImages = masterImages; // 1
				break;
			case 2:
				currentImages = philosopherImages;
				break;
			case 3:
				currentImages = sageImages;
				break;
			case 4:
				currentImages = explorerImages;
				break;
			case 5:
				currentImages = wizardImages; // 5
				break;
			case 6:
				currentImages = yogiImages;
				break;
	
			}

			hero [i].gameObject.GetComponentInChildren<Text> ().enabled = false;
			hero [i].gameObject.GetComponentInChildren<RawImage> ().texture = currentImages [0];
			for (int j = 0; j < 3; ++j) {
				hero [i].gameObject.GetComponentInChildren<TextureAnimator> ().images [j] = currentImages [j];

			}
			for (int j = 3; j < 6; ++j) {
				hero [i].gameObject.GetComponentInChildren<TextureAnimator> ().images [j] = currentImages [5-j];
			}

		}
	}

	public void hideMaru() {
		maru.stopBlinkHidden ();
	}

	public void reset() {
		maru.stopBlinkHidden ();
		batsu.stopBlinkHidden ();
	}

	void Start() {
		reset ();
	}

	public void showBatsu(Vector2 worldPos) {
		batsu.transform.position = worldPos;
		batsu.startBlink ();
	}

	public void showMaru(Vector3 worldPos) {
		maru.transform.position = worldPos;
		maru.startBlink ();
	}

	public Vector2 coordinatesOfItem(int n) {
		return hero [n].transform.position;
	}


}
