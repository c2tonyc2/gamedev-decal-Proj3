using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float dampTime;

	private Vector3 offset; 
	private Camera camera;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		player = transform.parent.gameObject;
		offset = transform.position - player.transform.position;
		camera = gameObject.GetComponent<Camera>();
		dampTime = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		Vector3 point = camera.WorldToViewportPoint(player.transform.position + offset);
		Vector3 delta = player.transform.position + offset - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
//		transform.position = new Vector3(player.transform.position.x, 0 , 0) + offset;
	}
}
