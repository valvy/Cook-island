using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;


public sealed class GyroInput {

	private static GyroInput instance;


	private GyroInput(){
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.orientation = ScreenOrientation.LandscapeLeft;

	}

	public static GyroInput getInstance(){
		if (GyroInput.instance == null) {
			GyroInput.instance = new GyroInput ();
		}
		return instance;

	}

	public float getTilt() {
		if (Application.isEditor) {
			return Input.GetAxis ("Vertical");
		} else {
			return Input.gyro.attitude.y;
		}

		//
	}
}

