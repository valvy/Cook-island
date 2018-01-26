using System.Collections;
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

    private float rotation;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(new Vector3(0,1,0) * speed * Time.deltaTime);


        Debug.Log(transform.eulerAngles);
        /*
        if(transform.eulerAngles.z < range && rotation > 0)
        {
            transform.Rotate(new Vector3(0, 0, rotation * rotationSpeed * Time.deltaTime));
        }
        else if(transform.eulerAngles.z > (360-range) && rotation < 0)
        {
            transform.Rotate(new Vector3(0, 0, rotation * rotationSpeed * Time.deltaTime));
        }
        */
        transform.Rotate(new Vector3(0, 0, rotation * rotationSpeed * Time.deltaTime));

		float horizontal = GyroInput.getInstance().getTilt();

        UpdateRotation(horizontal);

    }

    public void UpdateRotation(float rotation)
    {
        this.rotation = rotation;
    }
}
