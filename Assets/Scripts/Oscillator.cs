using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f; // Time taken to complete one full cycle, default is 2 second

    float movementFactor; // 0 for not moved, 1 for fully moved.

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (period <= Mathf.Epsilon)    // protect againt period is equal to zero
        {
            return;
        }

        float cycles = Time.time / period;  // Number of cycles which it have gone through as the game progresses 

        const float tau = Mathf.PI * 2f;    // 360 deg
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to 1

        movementFactor = (rawSinWave / 2f) + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset; 
	}
}
