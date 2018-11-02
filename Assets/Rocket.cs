using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))    
        {
            rigidBody.AddRelativeForce(Vector3.up); // Vector3.up is in the Y direction, .AddRelativeForce add force which is always relative to the object position
        }
        if (Input.GetKey(KeyCode.A) && (Input.GetKey(KeyCode.D) == false))
        {
            print("Rotating Left");
        }
        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) == false))
        {
            print("Rotating Right");
        }
    }
}
