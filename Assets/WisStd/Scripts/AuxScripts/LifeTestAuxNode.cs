using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LifeTestAuxNode : MonoBehaviour {

	public List <int> sabiduria;
	public List <int> sabio;

	public LifeTestAuxNode() {
		sabiduria = new List<int>();
		sabio = new List<int>();
	}

}
