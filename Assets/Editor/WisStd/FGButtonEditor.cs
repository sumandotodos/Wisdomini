//#define PLAYINEDITOR

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(FGButton))]
public class FGButtonEditor : Editor  {


	public override void OnInspectorGUI() {

		DrawDefaultInspector ();

		FGButton butRef = (FGButton)target;

		GameObject RosettaGO = GameObject.Find ("RosettaWrapper");
		if (RosettaGO != null)
			butRef.rosetta = RosettaGO.GetComponent<RosettaWrapper> ().rosetta;

		if (GUILayout.Button ("Upload to rosetta")) {
			if (butRef.rosetta != null) {
				//butRef.rosetta.registerString (butRef.key, butRef.text);
				Object prefab = PrefabUtility.CreateEmptyPrefab ("Assets/Prefabs/Rosetta_new.prefab");
				PrefabUtility.ReplacePrefab (butRef.rosetta.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);


			}
		}

	}

}


#endif