using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;

	public float pseudoX;
	private float anchorX;

	public GameObject playerOwner;

	public float offsetX;
	public float offsetY;

	private Vector3 anchorPos;
	private Vector3 currentPos;

	// Use this for initialization
	void Start () {
		anchorX = 0;
	}

	// Update is called once per frame
	void Update () {
		anchorPos = new Vector3 (anchorX, player1.transform.position.y, 0);
		currentPos = new Vector3 (anchorX + pseudoX, player2.transform.position.y, 0);

		Vector3 midPointVector = (currentPos + anchorPos) / 2;
		Vector3 offset = new Vector3 (offsetX, offsetY, 0);
		transform.position = playerOwner.transform.position + offset;

		Vector3 relative = currentPos - anchorPos;

		float angle = Mathf.Atan2 (relative.y, relative.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = q;
	}
}
