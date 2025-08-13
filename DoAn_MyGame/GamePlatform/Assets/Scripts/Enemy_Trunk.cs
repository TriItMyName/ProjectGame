using System.Collections;
using UnityEngine;

public class Enemy_Trunk : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] Transform Player;
    [SerializeField] float agroWidth;
    [SerializeField] float agroHeight;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletPos;
    private bool isMovingRight = true;
    private Vector3 startPosition;
    public CheckOnPlatform checkOnPlatform;
    public CheckWall checkWall;
    public CheckGetDamePlayer checkGetDamePlayer;
    public Animator animator;
    private Rigidbody2D playerRb;
    private Rigidbody2D rb;
    PlayerControllers playerController;
    private bool lastIsHit = false;
    private bool allowHit = false;
    private GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkOnPlatform = GetComponentInChildren<CheckOnPlatform>();
        checkWall = GetComponentInChildren<CheckWall>();
        checkGetDamePlayer = GetComponentInChildren<CheckGetDamePlayer>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.GetComponent<PlayerControllers>();
        animator = GetComponent<Animator>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
        isMovingRight = true;
        Flip();
    }

    private IEnumerator Start()
    {
        startPosition = transform.position;
        if (checkGetDamePlayer != null)
        {
            checkGetDamePlayer.ResetState();
        }
        yield return new WaitForSeconds(0.2f);
        allowHit = true;
    }

    void Update()
    {
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

        bool currentIsHit = animator.GetBool("IsHit");
        if (currentIsHit != lastIsHit)
        {
            lastIsHit = currentIsHit;
        }
        CheckFlip();
    }

    void Flip()
    {
        isMovingRight = !isMovingRight;
        Vector3 scale = transform.localScale;
        scale.x = isMovingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
        rb.linearVelocity = new Vector2(isMovingRight ? Mathf.Abs(speed) : -Mathf.Abs(speed), 0);
    }

    void CheckFlip()
    {
        if (checkOnPlatform.CheckGround() || checkWall.CheckHitWall())
        {
            Flip();
        }
    }

    void StopChasingPlayer()
    {
        UpdateAnimation(false, true);
        if (IsInvoking(nameof(Shoot)))
            CancelInvoke(nameof(Shoot));
        rb.linearVelocity = new Vector2(isMovingRight ? Mathf.Abs(speed) : -Mathf.Abs(speed), 0);
    }

    void ChasePlayer()
    {
        Vector3 scale = transform.localScale;
        if (Player.position.x > transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
            isMovingRight = true;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x); 
            isMovingRight = false;
        }
        transform.localScale = scale;

        rb.linearVelocity = Vector2.zero;
        UpdateAnimation(true, false);
    }
    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        BulletScript bulletScript = newBullet.GetComponent<BulletScript>();

        Vector2 shootDirection = transform.localScale.x < 0 ? Vector2.right : Vector2.left;
        bulletScript.SetDirection(shootDirection);
    }
    private void UpdateAnimation(bool isAttack, bool isRun)
    {
        animator.SetBool("IsAttack", isAttack);
        animator.SetBool("IsRun", isRun);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(agroWidth, agroHeight, 0));
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allowHit)
        {
            bool isDamaged = checkGetDamePlayer.CheckDame();
            if (isDamaged)
            {
                animator.SetBool("IsHit", true);
                gameManager.AddScore(500);
                playerController.Bounce(5f);
                StartCoroutine(DestroyAfterAnimation());
            }
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return null;
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        Destroy(gameObject);
    }
}
