using UnityEngine;

public class MovingFlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    private Vector3 target;

    PlayerControllers playerControllers;
    Rigidbody2D rb;
    Vector3 moveDirection;

    private void Awake()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length > 0)
        {
            playerControllers = playerObjects[0].GetComponent<PlayerControllers>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = pointA.position;
        Direction();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (target == pointA.position)
            {
                target = pointB.position;
                Direction();
            }
            else
            {
                target = pointA.position;
                Direction();
            }
        }
    }

    void Direction()
    {
        moveDirection = (target - transform.position).normalized * speed;
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerControllers.isOnMovingFlatform = true;
            playerControllers.platformRb = rb;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerControllers.isOnMovingFlatform = false;
            playerControllers.platformRb = null;
        }
    }
}
