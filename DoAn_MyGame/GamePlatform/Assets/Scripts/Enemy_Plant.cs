using System.Collections;
using UnityEngine;

public class Enemy_Plant : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float agroWidth;
    [SerializeField] float agroHeight;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletPos;
    public CheckGetDamePlayer checkGetDamePlayer;
    private Rigidbody2D playerRb;
    private PlayerControllers playerController;
    private Animator animator;
    private bool lastIsHit = false;
    private GameManager gameManager;

    private void Awake()
    {
        checkGetDamePlayer = GetComponentInChildren<CheckGetDamePlayer>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.GetComponent<PlayerControllers>();
        animator = GetComponent<Animator>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    private void Start()
    {
        if (checkGetDamePlayer != null)
        {
            checkGetDamePlayer.ResetState();
        }
    }

    void Update()
    {
        Rect agroRect = new Rect(
            transform.position.x - agroWidth / 2,
            transform.position.y - agroHeight / 2,
            agroWidth,
            agroHeight
        );

        bool currentIsHit = animator.GetBool("IsHit");
        if (currentIsHit != lastIsHit)
        {
            lastIsHit = currentIsHit;
        }

        if (agroRect.Contains(Player.position))
        {
            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }
    }
    void StopChasingPlayer()
    {
        UpdateAnimation(false, true);
    }

    void ChasePlayer()
    {
        Vector3 currentScale = transform.localScale;
        if (Player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        
        UpdateAnimation(true, false);
    }
    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        BulletScript bulletScript = newBullet.GetComponent<BulletScript>();

        Vector2 shootDirection = transform.localScale.x < 0 ? Vector2.right : Vector2.left;
        bulletScript.SetDirection(shootDirection);
    }

    private void UpdateAnimation(bool isAttack, bool isIdle)
    {
        animator.SetBool("IsAttack", isAttack);
        animator.SetBool("IsIdle", isIdle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(agroWidth, agroHeight, 0));
    }
    



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool isDamaged = checkGetDamePlayer.CheckDame();
            if (isDamaged)
            {
                animator.SetBool("IsHit", true);
                gameManager.AddScore(200);
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

