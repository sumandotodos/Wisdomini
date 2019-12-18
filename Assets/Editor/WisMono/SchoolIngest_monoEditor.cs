//#define PLAYINEDITOR

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(SchoolIngest_mono))]
public class SchoolIngest_monoEditor : Editor  {
	

	public override void OnInspectorGUI() {

		DrawDefaultInspector ();



		SchoolIngest_mono ingestRef = (SchoolIngest_mono)target;


		if (GUILayout.Button ("Load file")) {
			string path = EditorUtility.OpenFilePanel ("Choose file", "", "txt");
			StreamReader fileIn = new StreamReader (path);
			string contents = fileIn.ReadToEnd ();
			ingestRef.loadFile (contents);

		}
		if (ingestRef.fileLoaded) {
			if (GUILayout.Button ("Process")) {

				parse (ingestRef);

			}
		}

	}

	int individualToInteger(string cl) {

		string strIndex = cl.Substring (cl.Length-2, 1);
		int index;
		int.TryParse(strIndex, out index);
		return index-1;

	}


	int classToInteger(string cl) {

		if (cl.Equals ("guerrero"))
			return 0;
		if (cl.Equals ("maestra"))
			return 1;
		if (cl.Equals ("filosofo"))
			return 2;
		if (cl.Equals ("sabio"))
			return 3;
		if (cl.Equals ("exploradora"))
			return 4;
		if (cl.Equals ("maga"))
			return 5;
		if (cl.Equals ("yogui"))
			return 6;

		return 0; // should NEVER reach this

	}


	public void parse(SchoolIngest_mono ingestRef) {

		List<GameObject> prefabList = new List<GameObject> ();

		List<int> sabioList;
		List<int> sabiduriaList;
		List<LifeTestAuxNode> auxNodeList;



		List<GameObject> auxNodeGOList;


		List<string> stringList; // aquí va a ir preguntas + respuestas


		StringBank sb;
		int currentSabio;
		int currentSabiduria;

		LifeTestNode newNode;

		GameObject GO;
		GameObject auxGO;

		GameObject newNodeGO;
		GameObject newStringBankGO;
		StringBank newStringBank;


		GameObject auxNodeGO;

		// Structure is:

		//  +
		//	<hero>
		//	/
		//	<individual>
		//  [
		//	-<enunciado>
		//	*<respuesta> ] +
		// .. hasta el próximo +
		//
		// así que vamos a generar
		// 1 stringbank por héroe concreto
		// con 2n entradas, n preguntas y n respuestas



		AllTerrainParser parser = new AllTerrainParser (ingestRef.fileContents);
		parser.setParserMode (ParserMode.begin);


		Object prefab;

	
		int turn;

		int numberOfAuxNodes = 0;

		int prevSabiduria, prevSabio;
		prevSabio = prevSabiduria = -1;
		int thisSabiduria;
		int thisSabio;
		int ntests = 1;

		auxNodeGO = null;

		// creation of new storage
		newNodeGO = new GameObject ();
		newNode = newNodeGO.AddComponent<LifeTestNode> ();
		newNode.init ();

		sabioList = new List<int> ();
		sabiduriaList = new List<int> ();
		sb = new StringBank ();
		auxNodeList = new List<LifeTestAuxNode> ();
		auxNodeGOList = new List<GameObject> ();
		currentSabio = 0;
		currentSabiduria = 0;

		int indivInt = 0;
		string lastHero = "";

		LifeTestAuxNode auxNode = new LifeTestAuxNode();

		//--->

		while (parser.charAtHead () == '+') {



			parser.setParserMode (ParserMode.begin);
			parser.scanPastChar ('+');
			parser.setParserMode (ParserMode.begin);
			parser.scanToNextLine ();
			parser.setParserMode (ParserMode.end);
			parser.scanUntilValidChar ();
			string hero = parser.extract (); // name of the hero
			if(hero != lastHero)  { indivInt = -1; lastHero = hero; }

			parser.setParserMode(ParserMode.begin);
			parser.scanToNextLine ();

			while (parser.charAtHead () == '$') {

				stringList = new List<string> (); // clear string list

				indivInt++;

				parser.scanPastChar ('$');
//				parser.setParserMode (ParserMode.begin);
//				parser.scanToNextLine ();
//				parser.setParserMode (ParserMode.end);
//				parser.scanUntilValidChar ();
//				string individual = parser.extract ();
//
//				parser.setParserMode (ParserMode.begin);
				parser.scanToNextLine ();

				while (parser.charAtHead () == '-') {
			
					parser.setParserMode (ParserMode.begin);
					parser.scanPastChar ('-');
					parser.setParserMode (ParserMode.end);
					parser.scanWhileNotCharInList ('*');
					string question = parser.extract ();

					parser.setParserMode (ParserMode.begin);
					parser.scanPastChar ('*');
					parser.setParserMode (ParserMode.end);
					parser.scanWhileNotCharInList ('$', '-', '+');
					string answer = parser.extract ();

					stringList.Add (question);
					stringList.Add (answer);

					parser.setParserMode (ParserMode.begin);
					parser.scanWhileNotCharInList ('$', '-', '+');
					//parser.scanToNextLine ();
			
				}

				char qqq = parser.charAtHead ();

				// no more answer/questions to add

				newStringBankGO = new GameObject ();
				newStringBank = newStringBankGO.AddComponent<StringBank> ();
				newStringBank.reset ();
				newStringBank.phrase = new string[stringList.Count];
				for (int i = 0; i < stringList.Count; ++i) {
					newStringBank.phrase [i] = stringList [i]; // copy all strings to new stringbank object
				}
				newStringBank.extra = "SchoolTest" + hero + indivInt;
				prefab = PrefabUtility.CreateEmptyPrefab ("Assets/Prefabs/StringBanks/" + ingestRef.outputFolder + "/StringBank(SchoolTest" + hero + indivInt + ").prefab");
				GO = PrefabUtility.ReplacePrefab (newStringBankGO, prefab, ReplacePrefabOptions.ConnectToPrefab);
				DestroyImmediate (newStringBankGO);

			} // end of '$'

		} // end of '+'

		//<---



			

	}


}

#endif