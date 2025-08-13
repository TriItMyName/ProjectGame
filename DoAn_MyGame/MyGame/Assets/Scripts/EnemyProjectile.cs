using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private CheckWall checkWall;
    private float lifeTimer;

    private void Awake()
    {
        checkWall = GetComponentInChildren<CheckWall>();
    }

    public void ActivateProjectile()
    {
        lifeTimer = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        float moveSpeed = speed * Time.deltaTime;
        transform.Translate(-moveSpeed, 0, 0);

        lifeTimer += Time.deltaTime;
        if (lifeTimer > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || checkWall.CheckHitWall())
        {
            gameObject.SetActive(false);
        }
    }
}
