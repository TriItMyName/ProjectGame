using UnityEngine;
using UnityEngine.Timeline;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] float lifeTime;
    private Rigidbody2D rb;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
        rb.linearVelocity = direction.normalized * force;

       
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        checkWall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void checkWall()
    {
        if (Physics2D.OverlapCircle(this.transform.position, 0.1f, LayerMask))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.1f);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
}
