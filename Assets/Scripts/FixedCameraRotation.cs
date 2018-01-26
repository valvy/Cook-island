using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraRotation : MonoBehaviour {
    private Quaternion initialRotation;
	// Use this for initialization
	void Start () {
        initialRotation = transform.rotation;
	}

    private void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
