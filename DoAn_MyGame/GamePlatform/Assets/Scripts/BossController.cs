using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float flySpeed = 10f;
    public float attackCooldown = 2f;

    [Header("Position")]
    public GameObject defaultPositionObject;

    [Header("Components")]
    public HealthBar_Boss healthBar;
    public CheckBossGetDame checkBossGetDame;
    public Animator animator;
    public Rigidbody2D playerRb;
    public Rigidbody2D rb;
    PlayerControllers playerController;

    private bool isAttacking = false;
    private bool isReturning = false;
    private bool isInvulnerable = false;
    private float invulnerableTime = 5f;
    public BoxCollider2D damageAreaCollider;

    public void Awake()
    {
        checkBossGetDame = GetComponentInChildren<CheckBossGetDame>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = playerRb.GetComponent<PlayerControllers>();
        healthBar = GetComponentInChildren<HealthBar_Boss>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (defaultPositionObject == null)
        {
            Debug.LogError("defaultPositionObject is not assigned in Inspector!");
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        StartCoroutine(SpawnAndIdle());
    }

    void Update()
    {

    }
    private IEnumerator SpawnAndIdle()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(AttackCycle());
    }

    private IEnumerator AttackCycle()
    {
        while (currentHealth > 0)
        {
            yield return StartCoroutine(FlyAttack());
            yield return new WaitForSeconds(attackCooldown);

            yield return StartCoroutine(ReturnToDefaultPosition());

            yield return StartCoroutine(ShootAttack());
            yield return new WaitForSeconds(attackCooldown - 1);

            yield return StartCoroutine(ReturnToDefaultPosition());
        }
    }

    private IEnumerator FlyAttack()
    {
        if (isAttacking || isReturning || isInvulnerable) yield break;

        isAttacking = true;
        animator.SetTrigger("doFlyAttack");
        FlipBoss();
        yield return new WaitForSeconds(0.2f);

        Vector2 dir = (playerRb.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * flySpeed;

        yield return new WaitForSeconds(0.7f);
        rb.linearVelocity = Vector2.zero;
        isAttacking = false;
    }

    private IEnumerator ShootAttack()
    {
        if (isAttacking || isReturning || isInvulnerable) yield break;

        isAttacking = true;
        animator.SetTrigger("doAttack");
        FlipBoss();
        yield return new WaitForSeconds(0.5f);

        if (projectilePrefab && firePoint && playerRb)
        {
            Vector2 direction = (playerRb.position - (Vector2)firePoint.position).normalized;
            GameObject newBullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
            else
            {
                newBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private IEnumerator ReturnToDefaultPosition()
    {
        if (isReturning) yield break;

        isReturning = true;
        if (defaultPositionObject == null) yield break;

        Vector2 defaultPos2D = new Vector2(defaultPositionObject.transform.position.x, defaultPositionObject.transform.position.y);
        Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = defaultPos2D - currentPos2D;

        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), defaultPos2D) > 0.1f)
        {
            defaultPos2D = new Vector2(defaultPositionObject.transform.position.x, defaultPositionObject.transform.position.y);
            currentPos2D = new Vector2(transform.position.x, transform.position.y);
            targetPosition = defaultPos2D - currentPos2D;
            rb.linearVelocity = targetPosition.normalized * (flySpeed + 5);
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector3(defaultPos2D.x, defaultPos2D.y, transform.position.z);
        isReturning = false;
    }

    void TakeDame(int takedame)
    {
        if (isInvulnerable) return;

        currentHealth -= takedame;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerableCooldown());
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        this.enabled = false;
        StartCoroutine(DestroyAfterDelay(1.5f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && checkBossGetDame.CanTakeDamage())
        {
            TakeDame(10);
            playerController.Bounce(5f);

            StartCoroutine(DisableDamageAreaForSeconds(5f));

            if (!isReturning)
            {
                StartCoroutine(ReturnToDefaultPosition());
            }
        }
    }
    private IEnumerator InvulnerableCooldown()
    {
        isInvulnerable = true;
        isAttacking = true;

        yield return new WaitForSeconds(invulnerableTime);

        isInvulnerable = false;
        isAttacking = false;
    }
    private void FlipBoss()
    {
        if (playerRb == null) return;

        float direction = playerRb.position.x - transform.position.x;
        Vector3 localScale = transform.localScale;
        if (direction > 0)
            localScale.x = -Mathf.Abs(localScale.x);
        else if (direction < 0)
            localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private IEnumerator DisableDamageAreaForSeconds(float seconds)
    {
        if (damageAreaCollider != null)
            damageAreaCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        if (damageAreaCollider != null)
            damageAreaCollider.enabled = true;
    }
}