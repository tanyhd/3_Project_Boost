using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thrusterSound;
    [SerializeField] float thrustSpeed = 100f;
    [SerializeField] float rotationSpeed = 10f;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thrusterSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Thrust();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");    // do nothing
                break;
            case "Fuel":
                print("Fuel");
                break;
            default:
                print("DEAD");
                break;

        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;    // take manuel control of rotation

        if (Input.GetKey(KeyCode.A) && (Input.GetKey(KeyCode.D) == false)) // Rotate Left
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);  // Vector3.forward is rotation in the Z direction
        }
        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) == false))  // Rotate Right
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        rigidBody.freezeRotation = false;    // resume physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime); // Vector3.up is in the Y direction, .AddRelativeForce add force which is always relative to the object position
            if (!thrusterSound.isPlaying)   // so it doesn't layer
            {
                thrusterSound.Play();
            }
        }
        else
        {
            thrusterSound.Stop();
        }
    }
}
