using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class GyroInput {

	private static GyroInput instance;


	private GyroInput() {
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		//Screen.orientation = ScreenOrientation.LandscapeLeft;
		Input.gyro.enabled = true;

	}

	public static GyroInput getInstance() {
		if (GyroInput.instance == null) {
			GyroInput.instance = new GyroInput ();
		}
		return instance;
	}

	public float getTilt() {
		const float MULTIPLIER = -10.5f;

		switch (Application.platform) {
		case RuntimePlatform.Android:
                return Input.acceleration.x * -1;// * MULTIPLIER;
		case RuntimePlatform.IPhonePlayer:
                return Input.gyro.attitude.y * MULTIPLIER;
		default:
			const float MINIFIER = 0.1f;
			return Input.GetAxis ("Horizontal") * MINIFIER;
		}
	}
}

