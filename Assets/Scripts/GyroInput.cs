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
		Input.gyro.enabled = true;

	}

	public static GyroInput getInstance(){
		if (GyroInput.instance == null) {
			GyroInput.instance = new GyroInput ();
		}
		return instance;

	}

	public float getTilt() {
		const float MULTIPLIER = 10;
		switch (Application.platform) {
		case RuntimePlatform.Android:
			return Input.acceleration.y * MULTIPLIER;
		case RuntimePlatform.IPhonePlayer:
			return Input.gyro.attitude.y * MULTIPLIER;
		default:
			return Input.GetAxis ("Horizontal");
		}
	}
}

