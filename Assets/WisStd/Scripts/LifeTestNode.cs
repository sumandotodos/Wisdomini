using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LifeTestNode : MonoBehaviour {

	public int sabiduria; // category of hero
	public int sabio;     // individual
	public StringBank stringBank;
	public LifeTestAuxNode[] others;
	//public List<int> others_sabiduria; // we unwrap in order no to have to build a wrapper class, since
	//public List<int> others_sabio;     //  List<class> is not serializable :/

	public void init() {
		
		sabiduria = sabio = 0;
		stringBank = new StringBank ();
		//others = new List<LifeTestAuxNode> ();

	}

	public LifeTestNode() {

		init ();

	}

}
	
