using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private bool isMovingRight = true;
    private Vector3 startPosition;
    public CheckOnPlatform checkOnPlatform;
    public CheckWall checkWall;
    public CheckGetDamePlayer checkGetDamePlayer;
    public Animator animator;
    private Transform Player;
    private Rigidbody2D playerRb;
    private Rigidbody2D rb;
    private int enemyID;
    PlayerControllers playerController;
    private bool lastIsHit = false;
    private bool allowHit = false;

    private void Awake()
    {   
        rb = GetComponent<Rigidbody2D>();
        enemyID = GetInstanceID();
        checkOnPlatform = GetComponentInChildren<CheckOnPlatform>();
        checkWall = GetComponentInChildren<CheckWall>();
        checkGetDamePlayer = GetComponentInChildren<CheckGetDamePlayer>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.GetComponent<PlayerControllers>();
        animator = GetComponent<Animator>();
        isMovingRight = false;
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
        bool currentIsHit = animator.GetBool("IsHit");
        if (currentIsHit != lastIsHit)
        {
            lastIsHit = currentIsHit;
        }
        CheckFlip();
    }

    void Flip()
    {
        transform.rotation = Quaternion.Euler(0, isMovingRight ? 0 : 180, 0);
        speed = -speed;
        rb.linearVelocity = new Vector2(speed, 0);
        isMovingRight = !isMovingRight;
    }

    void CheckFlip()
    {
        if (checkOnPlatform.CheckGround() || checkWall.CheckHitWall())
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allowHit)
        {
            bool isDamaged = checkGetDamePlayer.CheckDame();
            if (isDamaged)
            {
                animator.SetBool("IsHit", true);
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
