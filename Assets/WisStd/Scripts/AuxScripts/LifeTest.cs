using UnityEngine;
using System.Collections;


public class LifeTest : MonoBehaviour {

	public LifeTestNode[] guerreros;
	public LifeTestNode[] maestras;
	public LifeTestNode[] filosofos;
	public LifeTestNode[] sabios;
	public LifeTestNode[] exploradoras;
	public LifeTestNode[] magas;
	public LifeTestNode[] yoguis;

	public LifeTestNode[] arrayByHero(int h) {
		switch (h) {
		case GameController_multi.WARRIOR:
			return guerreros;
		case GameController_multi.MASTER:
			return maestras;
		case GameController_multi.PHILOSOPHER:
			return filosofos;
		case GameController_multi.SAGE:
			return sabios;
		case GameController_multi.EXPLORER:
			return exploradoras;
		case GameController_multi.WIZARD:
			return magas;
		case GameController_multi.YOGI:
			return yoguis;
		}
		return null;
	}
}

