using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){

		if (GUILayout.Button(String.Format("Gyro {0}",GyroInput.getInstance().getTilt() ))) {
				Debug.Log("Hello!");
		}
	}
}
