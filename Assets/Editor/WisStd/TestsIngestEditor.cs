#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[CustomEditor(typeof(TestIngest))]
public class TestIngestEditor : Editor  {
	

	public override void OnInspectorGUI() {

		DrawDefaultInspector ();



		TestIngest ingestRef = (TestIngest)target;


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

		string strIndex = cl.Substring (cl.Length-1, 1);
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


	public void parse(TestIngest ingestRef) {

		List<GameObject> prefabList = new List<GameObject> ();

		List<int> sabioList;
		List<int> sabiduriaList;
		List<LifeTestAuxNode> auxNodeList;
		List<GameObject> auxNodeGOList;
		List<string> stringList;
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
		//	<name of the set>
		//	-
		//	<question 1>
		//	/
		//	<answer 1 to question 1>
		//	/	
		//	<answer 2 to question 1>
		//	...etc
		//	-
		//	<question 2>
		//	/
		//	<answer 1 to question 2>
		//	...etc



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
		stringList = new List<string> ();
		sabioList = new List<int> ();
		sabiduriaList = new List<int> ();
		sb = new StringBank ();
		auxNodeList = new List<LifeTestAuxNode> ();
		auxNodeGOList = new List<GameObject> ();
		currentSabio = 0;
		currentSabiduria = 0;

		LifeTestAuxNode auxNode = new LifeTestAuxNode();

		while (parser.charAtHead () == '+') {


			//prefabList = new List<GameObject> ();

			parser.setParserMode (ParserMode.begin);
			parser.scanToChar ('+'); // go past '+'
			parser.scanToNextLine (); // onto next line

			turn = 0;
			while (parser.charAtHead () == '-') {

				parser.scanToChar ('-'); // go past '-'
				parser.setParserMode (ParserMode.end); // move end head until the end of the line
				parser.scanWhileNotNextLine ();
				string text = parser.extract ();
				parser.setParserMode (ParserMode.begin);
				parser.scanToNextLine ();
			
				if (turn == 0) { // text test

					stringList.Add (text);

				}

				if(turn > 0) { 

					string[] partes = text.Split (':');
					thisSabiduria = classToInteger(partes[0]);
					thisSabio = individualToInteger (partes [1]);

					if (turn == 1) { // main sabio and sabiduria

						currentSabiduria = thisSabiduria;
						currentSabio = thisSabio;
						auxNodeGO = new GameObject ();
						auxNode = auxNodeGO.AddComponent<LifeTestAuxNode> ();

					} 

					else { // optional sabio and sabiduria


						auxNode.sabio.Add(thisSabio);
						auxNode.sabiduria.Add (thisSabiduria);

					}

				}

				++turn;

			}

			if (ingestRef.outputTempNodes) {
				prefab = PrefabUtility.CreateEmptyPrefab ("Assets/Prefabs/" + ingestRef.outputFolder + "/LifeTestAuxNode" + numberOfAuxNodes + ".prefab");
				auxGO = PrefabUtility.ReplacePrefab (auxNodeGO, prefab, ReplacePrefabOptions.ConnectToPrefab);
				auxNodeGOList.Add (auxGO);
			}
			++numberOfAuxNodes;



			// we found something different from '-' check if we must create a new LifeTestNode or we remain on the same
			if((prevSabio != -1) && ((currentSabiduria != prevSabiduria) || (currentSabio != prevSabio))) {
				sb.phrase = new string[stringList.Count-1];
				for(int i = 0; i<stringList.Count-1; ++i) {
					sb.phrase[i] = stringList[i];
				}
				// create prefab
				newStringBankGO = new GameObject();
				newStringBank = newStringBankGO.AddComponent<StringBank> ();
				newStringBank.reset ();
				newStringBank.phrase = new string[stringList.Count-1];
				for (int i = 0; i < stringList.Count-1; ++i) {
					newStringBank.phrase [i] = stringList [i]; // copy all strings to new stringbank object
				}
				newStringBank.extra = "LifeTest" + ntests;
				prefab = PrefabUtility.CreateEmptyPrefab ("Assets/Prefabs/StringBanks/" + ingestRef.outputFolder + "/StringBank(LifeTest" + ntests + ").prefab");
				GO = PrefabUtility.ReplacePrefab (newStringBankGO, prefab, ReplacePrefabOptions.ConnectToPrefab);

				if (ingestRef.outputTempNodes) {
					
					newNode.stringBank = GO.GetComponent<StringBank> (); // connect prefabbed stringBank to newNode
					newNode.others = new LifeTestAuxNode[stringList.Count-1];
					for (int i = 0; i < stringList.Count - 1; ++i) {
						newNode.others [i] = auxNodeGOList [i].GetComponent<LifeTestAuxNode>();
					}
					newNode.sabio = prevSabio;
					newNode.sabiduria = prevSabiduria;


					prefab = PrefabUtility.CreateEmptyPrefab ("Assets/Prefabs/" + ingestRef.outputFolder + "/LifeTestNode" + ntests + ".prefab");
					PrefabUtility.ReplacePrefab (newNodeGO, prefab, ReplacePrefabOptions.ConnectToPrefab);
				}

				DestroyImmediate (newStringBankGO);
				DestroyImmediate (newNodeGO);

				++ntests;

				// creation of new storage for next node
				newNodeGO = new GameObject ();
				newNode = newNodeGO.AddComponent<LifeTestNode> ();
				newNode.init ();
				string saveMe = stringList [stringList.Count - 1];
				stringList = new List<string> ();
				stringList.Add (saveMe);
				GameObject saveMeToo;
				if (ingestRef.outputTempNodes) {
					saveMeToo = auxNodeGOList [auxNodeGOList.Count - 1];
					auxNodeGOList = new List<GameObject> ();
					auxNodeGOList.Add (saveMeToo);
				}
				sb = new StringBank ();

			}

			prevSabio = currentSabio;
			prevSabiduria = currentSabiduria;


		} // EndOf of while '+'



	}

}


#endif