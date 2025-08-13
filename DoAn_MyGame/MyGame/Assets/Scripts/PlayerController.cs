using System.Collections;
using UnityEngine;

public class PlayerControllers : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDoubleJumpForce;
    [SerializeField] private float jumpExtraForce = 20f;
    [SerializeField] private float maxJumpHoldTime = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask oneWayPlatformLayer;
    [SerializeField] private Transform oneWay;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float MaxJumpCount = 2;
    [SerializeField] private float wallJumpHorizontalForce;
    [SerializeField] private float wallJumpVerticalForce;
    [SerializeField] private float wallJumpCooldown;
    [SerializeField] private float wallSlideGravity;

    private float jumpBufferCounter = 0f;
    private Animator animator;
    private bool isGrounded;
    private bool isOnWall;
    private bool isOneWay;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;
    public bool isOnMovingFlatform;
    public Rigidbody2D platformRb;
    private float currentSpeed = 0f;
    private float jumpHoldTimer = 0f;
    private int jumpCount = 0;
    private float wallJumpTimer = 0.1f;
    private BoxCollider2D boxCollider;
    private bool canDoubleJump = false;
    private bool justWallJumped = false;
    private bool isWallJumping = false;
    private float horizontalInputRaw;
    private float horizontalInput;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
        audioManager = Object.FindFirstObjectByType<AudioManager>();
    }

    void Update()
    {
        if (gameManager.isGameOver() || gameManager.isGameWin())
            return;

        horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        horizontalInput = Input.GetAxis("Horizontal");

        if (wallJumpTimer > 0)
            wallJumpTimer -= Time.deltaTime;

        if (OnWall() && !isGrounded && wallJumpTimer <= 0)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 1;
        }

        HandleWallSlide();
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        if (isWallJumping)
            return;

        if (horizontalInputRaw != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, horizontalInputRaw * moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        Vector2 playerVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        if (isOnMovingFlatform && platformRb != null)
        {
            playerVelocity += new Vector2(platformRb.linearVelocity.x, 0);
        }

        rb.linearVelocity = playerVelocity;

        if (currentSpeed > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (currentSpeed < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        // Jump input
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (IsOnAnyGround())
        {
            jumpHoldTimer = maxJumpHoldTime;
            jumpCount = 0;
            canDoubleJump = true;
        }

        if (jumpBufferCounter > 0f && OnWall() && !IsOnAnyGround() && wallJumpTimer <= 0)
        {
            audioManager.PlayJumpSound();
            float direction = -Mathf.Sign(transform.localScale.x); 
            bool isJumpingOpposite = (horizontalInputRaw != 0 && Mathf.Sign(horizontalInputRaw) != Mathf.Sign(transform.localScale.x));

            if (isJumpingOpposite)
            {
                // Nhảy mạnh hơn để qua tường
                rb.linearVelocity = new Vector2(direction * wallJumpHorizontalForce, wallJumpVerticalForce);
            }
            else
            {
                // Nhảy nhẹ, chỉ bật ra khỏi tường
                rb.linearVelocity = new Vector2(direction * wallJumpHorizontalForce * 0.7f, wallJumpVerticalForce * 0.8f);
            }

            transform.localScale = new Vector3(direction, 1, 1);
            wallJumpTimer = wallJumpCooldown;
            jumpBufferCounter = 0f;
            jumpHoldTimer = maxJumpHoldTime;
            canDoubleJump = false;

            StartCoroutine(PreventWallStick());
            StartCoroutine(StopWallJumpAfterDelay());
            return;
        }



        if (jumpBufferCounter > 0f && IsOnAnyGround())
        {
            audioManager.PlayJumpSound();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            jumpHoldTimer = maxJumpHoldTime;
            jumpCount++;

            StartCoroutine(WaitForDoubleJump());
        }
        else if (jumpBufferCounter > 0f && !IsOnAnyGround() && canDoubleJump && jumpCount < MaxJumpCount)
        {
            audioManager.PlayJumpSound();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpDoubleJumpForce);
            jumpBufferCounter = 0f;
            jumpHoldTimer = maxJumpHoldTime;
            jumpCount++;
            canDoubleJump = false;
        }

        // Nhảy giữ nút
        if (Input.GetButton("Jump") && !IsOnAnyGround() && jumpHoldTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + jumpExtraForce * Time.deltaTime);
            jumpHoldTimer -= Time.deltaTime;
        }
    }

    private void HandleWallSlide()
    {
        isOnWall = OnWall() && !IsOnAnyGround();

        if (isOnWall)
        {
            rb.gravityScale = wallSlideGravity;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }


    private bool OnWall()
    {
        if (justWallJumped)
            return false;

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return hit.collider != null;
    }

    private bool IsOnAnyGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        isOneWay = Physics2D.OverlapCircle(oneWay.position, 0.2f, oneWayPlatformLayer);
        return isGrounded || isOneWay;
    }


    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(horizontalInput) > 0.1f;
        bool isJumping = !(isGrounded || isOneWay);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    public void Bounce(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    private IEnumerator WaitForDoubleJump()
    {
        yield return new WaitForSeconds(0.1f);
        canDoubleJump = true;
    }
    private IEnumerator PreventWallStick()
    {
        justWallJumped = true;
        yield return new WaitForSeconds(0.15f);
        justWallJumped = false;
    }
    private IEnumerator StopWallJumpAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isWallJumping = false;
    }

}
