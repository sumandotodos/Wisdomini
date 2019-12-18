using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextureSwitch : MonoBehaviour {

	public Texture[] inTex;
	public Texture outTex;
	public RawImage outComponent_N;

	public int channel;

	public void setChannel(int c) {

		if (c < inTex.Length) {
			outTex = inTex [c];
			if (outComponent_N != null) {
				outComponent_N.texture = outTex;
			}
		}
	
	}


}
