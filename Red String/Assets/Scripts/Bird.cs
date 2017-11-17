using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
	public bool left = false;
	public float speed = 0.05f;
	Vector3 velocity;

	// Use this for initialization
	void Start () {
		velocity = new Vector3(speed, 0, 0);
		if (left) {
			velocity = -velocity;
		}
		gameObject.GetComponent<Rigidbody2D> ().freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity;
	}
}
