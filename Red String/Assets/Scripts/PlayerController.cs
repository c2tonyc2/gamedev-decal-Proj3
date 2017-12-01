using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public Image tugCoolingBar;

	private bool invincible;
	public float invincibleDuration;
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
		maxTugForce = 20000;
		incrementTugForce = 200;

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
		if (tugTime < Time.time) {
			changeTugAlpha (1f);
		}

		tugBar.fillAmount = Mathf.Min(1, currentTugForce / maxTugForce);
        isGrounded = Physics2D.OverlapCircle(
            groundCheckPoint.position, 
            groundCheckRadius, 
            whatIsGround
        );
		print (isGrounded);

		if (currentTugForce < maxTugForce && Input.GetKey (tug) && tugTime < Time.time) {
			currentTugForce += incrementTugForce;
            maxHorizontalSpeed = 5 * (1 - currentTugForce / maxTugForce);
            animator.SetBool("tugging", true);
        } else if (Input.GetKeyUp (tug) && tugTime < Time.time)
		{
			Vector2 tugDirection = new Vector2 (
				transform.position.x - soulMate.transform.position.x,
				transform.position.y - soulMate.transform.position.y
			);
			tugDirection.x = pseudoX * Mathf.Sign (tugDirection.x);

			soulMateRb.AddForce (5 * tugDirection.normalized * Mathf.Min(currentTugForce, maxTugForce));

			tugTime += tugCooldown;
			changeTugAlpha (0.5f);

			currentTugForce = 0;
			maxHorizontalSpeed = 5;

            animator.SetBool("tugging", false);
        }

        if (Input.GetKeyDown(jump) && isGrounded)
        {
			GetComponent<AudioSource> ().Play ();
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

            //check if jumping
            if (isGrounded) {
                animator.SetBool("onGround", true);
            }
            else
            {
                animator.SetBool("onGround", false);
            }

            //checking for movement
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
			startInvincible ();
		}

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!invincible)
		{
			if (collision.gameObject.layer == 10)
			{
				Vector2 dir = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
				dir = -dir.normalized;
				rb.AddForce(dir * staggerForce);
				startInvincible ();
			}
		}
	}

	void startInvincible()
	{
		StopAllCoroutines ();
		invincible = true;
		Invoke("endInvincibility", invincibleDuration);
		StartCoroutine(FlashSprite());
	}

	void endInvincibility()
	{
		invincible = false;
		StopAllCoroutines();
		GetComponent<SpriteRenderer> ().enabled = true;
	}

	IEnumerator FlashSprite()
	{
		while(true)
		{
			GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds(.02f);
			GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds(.02f);
		}
	}

	void changeTugAlpha(float newAlpha) {
		Color modified = tugCoolingBar.color;
		modified.a = newAlpha;
		tugCoolingBar.color = modified;
	}
		
}
