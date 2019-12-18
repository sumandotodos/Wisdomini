#define PLAYINEDITOR

#if PLAYINEDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class Item {

	public string item;

}

[System.Serializable]
public class PreStringBank {

	public List<Item> list;

}

[CustomEditor(typeof(JSON2StringBank))]
public class JSON2StringBankEditor : Editor  {


	public override void OnInspectorGUI() {

		DrawDefaultInspector ();



		JSON2StringBank ingestRef = (JSON2StringBank)target;


		if (GUILayout.Button ("Load file")) {
			string path = EditorUtility.OpenFilePanel ("Choose file", "", "txt");
			StreamReader fileIn = new StreamReader (path);
			string contents = fileIn.ReadToEnd ();
			ingestRef.loadFile (contents);

		}

		if (ingestRef.fileLoaded) {
			if (GUILayout.Button ("Convert")) {

				json2stringbank (ingestRef);

			}
		}

	}

	public void json2stringbank(JSON2StringBank refr) {

		PreStringBank psb = JsonUtility.FromJson<PreStringBank> (refr.jsondata);

		int a = psb.list.Count;

		GameObject sbGO = new GameObject ();
		StringBank sb = sbGO.AddComponent<StringBank> ();
		sb.phrase = new string[psb.list.Count];
		for (int i = 0; i < psb.list.Count; ++i) {
			sb.phrase [i] = psb.list [i].item;
		}
		sb.extra = refr.stringBankName;
		Object prefab = PrefabUtility.CreateEmptyPrefab ("Assets" + refr.outputFolder + "(" + sb.extra + ").prefab");
		PrefabUtility.ReplacePrefab (sb.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
		DestroyImmediate (sbGO);


	}

}
	

#endif