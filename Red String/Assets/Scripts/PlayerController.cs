using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class PlayerController : MonoBehaviour {
	public float staggerForce = 300.0f;
	public int dropThreshold = -20;

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

	private float pseudoX;
	private float maxTugForce;
	private float currentTugForce;
	private float incrementTugForce;
	public float tugCooldown;
	private float tugTime;
	public Image tugBar;

	private bool invincible;
	private Vector3 lastPosition;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
		soulMateRb = soulMate.GetComponent<Rigidbody2D> ();
        animator = this.GetComponent<Animator> ();

        theScale = transform.localScale;

		pseudoX = 20;
		tugTime = Time.time;
		currentTugForce = 0;
		maxTugForce = 6000;
		incrementTugForce = 70;

		moveForce = 200;
		jumpForce = 18;
		maxHorizontalSpeed = 5;
		maxVerticalSpeed = 20;

		invincible = false;
    }

    void Flip()
    {
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update () 
	{
		tugBar.fillAmount = currentTugForce / maxTugForce;
        isGrounded = Physics2D.OverlapCircle(
            groundCheckPoint.position, 
            groundCheckRadius, 
            whatIsGround
        );

		if (currentTugForce < maxTugForce && Input.GetKey (tug) && tugTime < Time.time) {
			currentTugForce += incrementTugForce;
			maxHorizontalSpeed = 5 * (1 - currentTugForce/maxTugForce);
		} else if (Input.GetKeyUp (tug) && tugTime < Time.time)
		{
			Vector2 tugDirection = new Vector2 (
				transform.position.x - soulMate.transform.position.x,
				transform.position.y - soulMate.transform.position.y
			);
			tugDirection.x = pseudoX * Mathf.Sign (tugDirection.x);

			soulMateRb.AddForce (tugDirection.normalized * Mathf.Min(currentTugForce, maxTugForce));

			tugTime += tugCooldown;
			currentTugForce = 0;
			maxHorizontalSpeed = 5;
		}

        if (Input.GetKeyDown(jump) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

		// remember last grounded position for respawn
		if (isGrounded) {
			lastPosition = transform.position;
		}
	}

	void FixedUpdate()
	{
		float x = Input.GetAxis (horizontalAxis);

		if (!invincible) {
			if (x * rb.velocity.x < maxHorizontalSpeed) {
				rb.AddForce (Vector2.right * x * moveForce);
			}

			if (Mathf.Abs (rb.velocity.x) > maxHorizontalSpeed) {
				rb.velocity = new Vector2 (maxHorizontalSpeed * Mathf.Sign (rb.velocity.x), rb.velocity.y);
			}

			if (Mathf.Abs (rb.velocity.y) > maxVerticalSpeed) {
				rb.velocity = new Vector2 (rb.velocity.x, maxVerticalSpeed * Mathf.Sign (rb.velocity.y));
			}

			if (x < 0) {
				animator.SetBool ("Left", true);
				if (faceRight)
					Flip ();
				faceRight = false;
			} else if (x > 0) {
				animator.SetBool ("Right", true);
				if (!faceRight)
					Flip ();
				faceRight = true;
			}
		}
		if (rb.velocity.x == 0) {
			animator.SetBool ("Right", false);
			animator.SetBool ("Left", false);
		}
		// dropping respawn
		if (transform.position.y < dropThreshold) {
			// how to get a good respawn position?
			transform.position = lastPosition + new Vector3(0, 1, 0);
			rb.velocity = new Vector2 (0, 0);
			invincible = true;
			Invoke("resetInvincibility", 1);
		}

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!invincible)
		{
			if (collision.gameObject.layer == 10)
			{
				invincible = true;
				Vector2 dir = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
				dir = -dir.normalized;
//				print ("Im invincible");
//				print (staggerForce);
				rb.AddForce(dir * staggerForce);
				Invoke("resetInvincibility", 1);
			}
		}
	}

	void resetInvincibility()
	{
		invincible = false;
	}


}
