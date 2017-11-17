using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;

	public float attackDist = 100;
	public float verticalAttackForce = 300;
	public float horizontalAttackForce = 300;
	public float moveForce = 10;
	public float leftWall = 0;
	public float rightWall = 0;
	private bool left = false;
	private bool hasJumped = false;
	private bool isJumping = false;
	private Rigidbody2D rb;

	public Transform groundCheckPoint;
	public float groundCheckRadius;
	public bool isGrounded = true;
	public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find ("Player 1");
		player2 = GameObject.Find ("Player 2");
		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.freezeRotation = true;
		rb.velocity = new Vector3 (moveForce, 0, 0);
	}

	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.OverlapCircle(
			groundCheckPoint.position, 
			groundCheckRadius, 
			whatIsGround
		);
		print (isGrounded);
		// raycast forward or check distance to player
		float p1Dist = Vector3.Distance(player1.transform.position, gameObject.transform.position);
		float p2Dist = Vector3.Distance(player2.transform.position, gameObject.transform.position);
		if (p1Dist < attackDist && !hasJumped) {
			// jump at player
//			print("WOW");
			hasJumped = true;
			rb.velocity = new Vector3 (0, 0, 0);
			isGrounded = false;
			if (gameObject.transform.position.x - player1.transform.position.x > 0) {
				rb.AddForce (new Vector3 (-horizontalAttackForce, verticalAttackForce, 0));
			} else {
				rb.AddForce (new Vector3 (horizontalAttackForce, verticalAttackForce, 0));
			}
		} else if (p2Dist < attackDist && !hasJumped) {
			hasJumped = true;
			rb.velocity = new Vector3 (0, 0, 0);
			if (gameObject.transform.position.x - player2.transform.position.x > 0) {
				rb.AddForce (new Vector3 (-horizontalAttackForce, verticalAttackForce, 0));
			} else {
				rb.AddForce (new Vector3 (horizontalAttackForce, verticalAttackForce, 0));
			}
		} else {
			if (isGrounded) {
				if (left) {
					if (gameObject.transform.position.x <= leftWall) {
						left = false;
						rb.velocity = new Vector3 (moveForce, 0, 0);
					} 
					rb.velocity = new Vector3 (-moveForce, 0, 0);
				} else {
					if (gameObject.transform.position.x >= rightWall) {
						left = true;
						rb.velocity = new Vector3 (-moveForce, 0, 0);
					}
					rb.velocity = new Vector3 (moveForce, 0, 0);
				}
			}
		}
	}
}
