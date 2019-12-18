﻿//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//
//public enum EaseType { linear, tanh, cubicOut, boingOut, boingOutMore };
//
//public class UIBetterScaleFader : FGProgram {
//
//	public float prevValue;
//	public float value;
//
//	public float scaleValue;
//	public float scaleTarget;
//
//	public SoftFloat scale;
//	bool started = false;
//
//	public float maxScale = 1.0f;
//	public float minScale = 0.0f;
//	public float speed = 1.0f;
//
//	public float linSpaceTarget;
//	public float linSpaceOrigin;
//	public float linSpaceValue;
//
//	public EaseType easeType;
//
//	public bool startScaledOut = true;
//
//	int state = 0; 	// 0: idle;
//					// 1: fading
//
//	private void updateScale() {
//		this.transform.localScale = new Vector3 (scaleValue, scaleValue, scaleValue);
//	}
//
//	public void reset() {
//		if (startScaledOut) {
//			scale.setValueImmediate (minScale);
//			scaleValue = scaleTarget = minScale;
//		} else {
//			scale.setValueImmediate (maxScale);
//			scaleValue = scaleTarget = maxScale;
//		}
//		state = 0;
//		updateScale ();
//	}
//
//	public void Start() {
//		if (started == true)
//			return;
//		started = true;
//		scale = new SoftFloat ();
//		switch (easeType) {
//		case EaseType.linear:
//			break;
//		case EaseType.boingOut:
//			scale.setTransformation (TweenTransforms.boingOut);
//			break;
//		case EaseType.cubicOut:
//			scale.setTransformation (TweenTransforms.cubicOut);
//			break;
//		case EaseType.tanh:
//			scale.setTransformation (TweenTransforms.tanh);
//			break;
//		}
//
//
//		scale.setSpeed (speed);
//
//		reset ();
//
//	}
//
//	public void setEasyType(EaseType t) {
//		scale.setEasyType (t);
//	}
//
//	public void scaleIn() {
//		state = 1;
//		scale.setValue (maxScale);
//	}
//
//	public void scaleOut() {
//		state = 1;
//		scale.setValue (minScale);
//	}
//
//	public void scaleOutImmediately() {
//		state = 1;
//		scale.setValueImmediate (minScale);
//	}
//
//	public void scaleInImmediately() {
//		state = 1;
//		scale.setValueImmediate (maxScale);
//	}
//
////	public void scaleInTask(FGProgram waiter) {
////		registerWaiter (waiter);
////		scaleIn ();
////	}
////
////	public void scaleOutTask(FGProgram waiter) {
////		registerWaiter (waiter);
////		scaleOut ();
////	}
//
//	void Update() {
//
//		linSpaceValue = scale.linSpaceValue;
//		linSpaceOrigin = scale.linSpaceOrigin;
//		linSpaceTarget = scale.linSpaceTarget;
//
//		scaleValue = scale.getValue ();
//		//scaleTarget = scale.linSpaceTarget;
//
//		//update (); // update program
//		prevValue = scale.prevValue;
//		value = scale.getValue ();//.value;
//		if(state == 1) {
//			if (!scale.update ()) {
//				notifyFinish ();
//				state = 0;
//			} 
//			scaleValue = scale.getValue ();
//			updateScale ();
//
//		}
//
//	}
//
////	public void notifyFinishExternal() {
////		for (int i = 0; i < waiters.Count; ++i) {
////			waiters [i].waitFinish ();
////		}
////		waiters = new List<FGProgram>();
////	}
//		
//}
