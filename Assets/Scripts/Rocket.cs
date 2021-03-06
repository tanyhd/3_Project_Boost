using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float thrustSpeed = 100f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    [SerializeField] bool collisionDisabled = false;
    enum State {Alive, Dying, Transcending };
    State state = State.Alive;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
        if (Debug.isDebugBuild) // Check the system to see if it is in development build to see if it should respond to development keys
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))    // Use GetKeyDown to just return one time when key is press
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle on/off state
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;

        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            LoadFirstLevel();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A) && (Input.GetKey(KeyCode.D) == false)) // Rotate Left
        {
            RotateManually(rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) == false))  // Rotate Right
        {
            RotateManually(-rotationSpeed * Time.deltaTime);
        }

    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;    // take manuel control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);  // Vector3.forward is rotation in the Z direction
        rigidBody.freezeRotation = false;    // resume physics control of rotation
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime); // Vector3.up is in the Y direction, .AddRelativeForce add force which is always relative to the object position
        if (!audioSource.isPlaying)   // so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
