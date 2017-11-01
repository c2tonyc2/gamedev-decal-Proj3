using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;
	public GameObject soulMate;
	private Rigidbody2D soulMateRb;

    public float moveSpeed;
    public float jumpForce;

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode tug;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public bool isGrounded;
    public LayerMask whatIsGround;

    private Animator animator;
    private Vector3 theScale;
    public bool faceRight;

	public float tugForce;
	public float tugCooldown;
	public float tugTime;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
		soulMateRb = soulMate.GetComponent<Rigidbody2D> ();
        animator = this.GetComponent<Animator> ();

        theScale = transform.localScale;

		tugTime = Time.time;
    }

    void Flip()
    {
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update () {

        isGrounded = Physics2D.OverlapCircle(
            groundCheckPoint.position, 
            groundCheckRadius, 
            whatIsGround
        );

		if (Input.GetKey(left))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

            animator.SetBool("Left", true);
            if (faceRight)
                Flip();
            faceRight = false;
        } else if (Input.GetKey(right))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            animator.SetBool("Right", true);
            if (!faceRight)
                Flip();
            faceRight = true;
        }
//		else
//        {
//            rb.velocity = new Vector2(0, rb.velocity.y);
//            animator.SetBool("Left", false);
//            animator.SetBool("Right", false);
//        }

		if (Input.GetKeyDown (tug) && tugTime < Time.time)
		{
			Vector2 tugDirection = new Vector2 (
				                       transform.position.x - soulMate.transform.position.x,
				                       transform.position.y - soulMate.transform.position.y
			                       );
			soulMate.GetComponent<Rigidbody2D> ().AddForce (tugDirection * tugForce);
			tugTime += tugCooldown;
		}

        if (Input.GetKeyDown(jump) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
	}
}
