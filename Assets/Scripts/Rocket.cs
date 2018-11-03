using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thrusterSound;
    [SerializeField] float thrustSpeed = 100f;
    [SerializeField] float rotationSpeed = 10f;

    enum State {Alive, Dying, Transcending };
    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thrusterSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Rotate();
            Thrust();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);    // parameterise time
                break;
            default:
                print("Hit Something");
                state = State.Dying;
                Invoke("LoadFirstLevel", 2f);   // parameterise time
                break;

        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
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
