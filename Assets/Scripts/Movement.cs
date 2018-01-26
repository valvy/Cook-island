﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float range = 20;

    [SerializeField]
    private float timeToMove = 30;

    private float rotation;

    private float timer = 0;

    private bool isMoving = true;
	// Use this for initialization
	void Start ()
    {

	}


    // Update is called once per frame
    void Update ()
    {

        if (isMoving)
        {
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, rotation * rotationSpeed * Time.deltaTime));
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, ClampAngle(transform.eulerAngles.z, -range, range));

            float horizontal = GyroInput.getInstance().getTilt();
            UpdateRotation(horizontal);

            timer += Time.deltaTime;


            if (timer > timeToMove)
            {
                isMoving = false;
                timer = 0;
            }
        }

    }

    public void StartMoving()
    {
        timer = 0;
        isMoving = true;
    }

    public void UpdateRotation(float rotation)
    {
        this.rotation = rotation;
    }

    // Thank you internet
    // https://forum.unity.com/threads/limiting-rotation-with-mathf-clamp.171294/
    private float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }
}
