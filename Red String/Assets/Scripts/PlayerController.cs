using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;
	public GameObject soulMate;
	private Rigidbody2D soulMateRb;

	private float moveForce;
	private float maxHorizontalSpeed;
	private float maxVerticalSpeed;
	private float jumpForce;

    public KeyCode jump;
    public KeyCode tug;
	public string horizontalAxis;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public bool isGrounded;
    public LayerMask whatIsGround;

    private Animator animator;
    private Vector3 theScale;
    public bool faceRight;

	private float maxTugForce;
	private float currentTugForce;
	private float incrementTugForce;
	public float tugCooldown;
	private float tugTime;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
		soulMateRb = soulMate.GetComponent<Rigidbody2D> ();
        animator = this.GetComponent<Animator> ();

        theScale = transform.localScale;

		tugTime = Time.time;
		currentTugForce = 0;
		maxTugForce = 400;
		incrementTugForce = 10;

		moveForce = 200;
		jumpForce = 17;
		maxHorizontalSpeed = 5;
		maxVerticalSpeed = 20;
    }

    void Flip()
    {
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update () 
	{
        isGrounded = Physics2D.OverlapCircle(
            groundCheckPoint.position, 
            groundCheckRadius, 
            whatIsGround
        );

		if (currentTugForce < maxTugForce && Input.GetKey (tug) && tugTime < Time.time) {
			currentTugForce += incrementTugForce;
			maxHorizontalSpeed = 5 * (1 - currentTugForce/maxTugForce);
		} else if ((currentTugForce > maxTugForce || Input.GetKeyUp (tug)) && tugTime < Time.time)
		{
			Vector2 tugDirection = new Vector2 (
				transform.position.x - soulMate.transform.position.x,
				transform.position.y - soulMate.transform.position.y
			);
			soulMate.GetComponent<Rigidbody2D> ().AddForce (tugDirection * Mathf.Min(currentTugForce, maxTugForce));

			tugTime += tugCooldown;
			currentTugForce = 0;
			maxHorizontalSpeed = 5;
		}

        if (Input.GetKeyDown(jump) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
	}

	void FixedUpdate()
	{
		float x = Input.GetAxis (horizontalAxis);

		if (x * rb.velocity.x < maxHorizontalSpeed)
		{
			rb.AddForce (Vector2.right * x * moveForce);
		}

		if (Mathf.Abs (rb.velocity.x) > maxHorizontalSpeed) 
		{
			rb.velocity = new Vector2 (maxHorizontalSpeed * Mathf.Sign (rb.velocity.x) , rb.velocity.y);
		}

		if (Mathf.Abs (rb.velocity.y) > maxVerticalSpeed) 
		{
			rb.velocity = new Vector2 (rb.velocity.x, maxVerticalSpeed * Mathf.Sign (rb.velocity.y));
		}

		if (x < 0)
		{
			animator.SetBool("Left", true);
			if (faceRight)
				Flip();
			faceRight = false;
		} else if (x > 0)
		{
			animator.SetBool("Right", true);
			if (!faceRight)
				Flip();
			faceRight = true;
		}

		if (rb.velocity.x == 0) {
			animator.SetBool ("Right", false);
			animator.SetBool ("Left", false);
		}

	}
}
