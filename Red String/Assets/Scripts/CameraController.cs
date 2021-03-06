﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float dampTime;
	public float cameraVelocity;

	private Vector3 offset; 
	private Camera camera;
	private Vector3 velocity;

	// Use this for initialization
	void Start () {
//		player = GameObject.Find ("Player 1");
		offset = transform.position - player.transform.position;
		camera = gameObject.GetComponent<Camera>();
		dampTime = 0.1f;
		cameraVelocity = 1.0f;
		velocity = new Vector3 (cameraVelocity, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		Vector3 point = camera.WorldToViewportPoint(player.transform.position + offset);
		Vector3 delta = player.transform.position + offset - camera.ViewportToWorldPoint(new Vector3(0.5f, point.y, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
//		transform.position = new Vector3(player.transform.position.x, 0 , 0) + offset;
	}
}
