using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thruster;
    bool thruster_play;
    bool thruster_ToggleChange;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        thruster_play = true;
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();

	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!thruster.isPlaying) {
                thruster.Play();
            }

        }
        else
        {
            thruster.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);

        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
}
