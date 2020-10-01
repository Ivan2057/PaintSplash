using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	private Rigidbody myRigidbody;
	public float Force = 100;

	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (Input.GetKey(KeyCode.A)) {
			myRigidbody.AddForce (Force*Time.deltaTime,0,0);
		}
		if (Input.GetKey(KeyCode.D)) {
			myRigidbody.AddForce (-Force*Time.deltaTime,0,0);
		}
		if (Input.GetKey(KeyCode.W)) {
			myRigidbody.AddForce (0,0,-Force*Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S)) {
			myRigidbody.AddForce (0,0,Force*Time.deltaTime);
		}
	}
}
