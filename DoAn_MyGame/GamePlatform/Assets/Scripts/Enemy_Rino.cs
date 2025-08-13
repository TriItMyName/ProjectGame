using System.Collections;
using UnityEngine;

public class Enemy_Rino : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float agroWidth;
    [SerializeField] float agroHeight;
    [SerializeField] float speed;
    [SerializeField] float hitWallRecoveryTime;
    public CheckGetDamePlayer checkGetDamePlayer;
    private Vector2 currentVelocity;
    public CheckWall checkWall;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private PlayerControllers playerController;
    private Animator Animator;
    private bool isHitWall = false;
    private bool isDead = false;
    private GameManager gameManager;

    private void Awake()
    {
        checkWall = GetComponentInChildren<CheckWall>();
        checkGetDamePlayer = GetComponentInChildren<CheckGetDamePlayer>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.GetComponent<PlayerControllers>();
        Animator = GetComponent<Animator>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    void Start()
    {
        if (checkGetDamePlayer != null)
        {
            checkGetDamePlayer.ResetState();
        }
    }

    void Update()
    {
        if (isDead) return;
        if (isHitWall) return;

        Rect agroRect = new Rect(
            transform.position.x - agroWidth / 2,
            transform.position.y - agroHeight / 2,
            agroWidth,
            agroHeight
        );

        if (agroRect.Contains(Player.position))
        {
            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }


        if (checkWall != null && checkWall.CheckHitWall())

        {
            StartCoroutine(HandleHitWall());
        }
    }

    void StopChasingPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        UpdateAnimation(false, false);
        Animator.SetBool("IsIdle", true);
    }

    void ChasePlayer()
    {
        Vector3 currentScale = transform.localScale;
        Vector2 targetVelocity;

        if (transform.position.x < Player.position.x)
        {
            targetVelocity = new Vector2(speed, 0);
            if (currentScale.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
            }
        }
        else
        {
            targetVelocity = new Vector2(-speed, 0);
            if (currentScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
            }
        }

        Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.2f);
        rb.linearVelocity = smoothedVelocity;

        UpdateAnimation(false, true);
    }

    IEnumerator HandleHitWall()
    {
        isHitWall = true;
        rb.linearVelocity = Vector2.zero;
        UpdateAnimation(true, false);

        Vector2 moveBackDirection = new Vector2(-Mathf.Sign(transform.localScale.x), 0);
        rb.MovePosition(rb.position - moveBackDirection * 0.5f);

        yield return new WaitForSeconds(hitWallRecoveryTime);

        isHitWall = false;
        UpdateAnimation(false, false);
    }

    void UpdateAnimation(bool isHitWall, bool isRunning)
    {
        Animator.SetBool("IsHitWall", isHitWall);
        Animator.SetBool("IsRunning", isRunning);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(agroWidth, agroHeight, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            if (checkGetDamePlayer.CheckDame())
            {
                isDead = true;
                Animator.SetBool("IsHit", true);
                gameManager.AddScore(300);
                playerController.Bounce(5f);
                StartCoroutine(DestroyAfterAnimation());
                return;
            }
            else
            {
                if (!isHitWall)
                    StartCoroutine(HandleHitWall());
            }
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        Destroy(gameObject);
    }
}
