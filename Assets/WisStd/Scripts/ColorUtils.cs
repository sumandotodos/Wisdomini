using UnityEngine;
using System.Collections;



public class ColorUtils : MonoBehaviour {

	public struct rgb {
		public float r;       // percent
		public float g;       // percent
		public float b;       // percent
	};

	public struct hsv {
		public float h;       // angle in degrees
		public float s;       // percent
		public float v;       // percent
	};

	public static hsv rgb2hsv(float r, float g, float b) {
		rgb inColor;
		inColor.r = r;
		inColor.g = g;
		inColor.b = b;
		return rgb2hsv (inColor);
	}

	public static hsv rgb2hsv(rgb col)
	{
		hsv         res;
		float      min, max, delta;

		min = col.r < col.g ? col.r : col.g;
		min = min  < col.b ? min  : col.b;

		max = col.r > col.g ? col.r : col.g;
		max = max  > col.b ? max  : col.b;

		res.v = max;                                // v
		delta = max - min;
		if (delta < 0.00001)
		{
			res.s = 0;
			res.h = 0; // undefined, maybe nan?
			return res;
		}
		if( max > 0.0 ) { // NOTE: if Max is == 0, this divide would cause a crash
			res.s = (delta / max);                  // s
		} else {
			// if max is 0, then r = g = b = 0              
			// s = 0, v is undefined
			res.s = 0.0f;
			res.h = 0.0f;                            // its now undefined
			return res;
		}
		if( col.r >= max )                           // > is bogus, just keeps compilor happy
			res.h = ( col.g - col.b ) / delta;        // between yellow & magenta
		else
			if( col.g >= max )
				res.h = 2.0f + ( col.b - col.r ) / delta;  // between cyan & yellow
			else
				res.h = 4.0f + ( col.r - col.g ) / delta;  // between magenta & cyan

		res.h *= 60.0f;                              // degrees

		if( res.h < 0.0 )
			res.h += 360.0f;

		return res;
	}


	rgb hsv2rgb(hsv col)
	{
		float      hh, p, q, t, ff;
		long        i;
		rgb         res;

		if(col.s <= 0.0) {       // < is bogus, just shuts up warnings
			res.r = col.v;
			res.g = col.v;
			res.b = col.v;
			return res;
		}
		hh = col.h;
		if(hh >= 360.0) hh = 0.0f;
		hh /= 60.0f;
		i = (long)hh;
		ff = hh - i;
		p = col.v * (1.0f - col.s);
		q = col.v * (1.0f - (col.s * ff));
		t = col.v * (1.0f - (col.s * (1.0f - ff)));

		switch(i) {
		case 0:
			res.r = col.v;
			res.g = t;
			res.b = p;
			break;
		case 1:
			res.r = q;
			res.g = col.v;
			res.b = p;
			break;
		case 2:
			res.r = p;
			res.g = col.v;
			res.b = t;
			break;

		case 3:
			res.r = p;
			res.g = q;
			res.b = col.v;
			break;
		case 4:
			res.r = t;
			res.g = p;
			res.b = col.v;
			break;
		case 5:
		default:
			res.r = col.v;
			res.g = p;
			res.b = q;
			break;
		}
		return res;     
	}

}
