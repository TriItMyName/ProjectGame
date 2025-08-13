using System.Runtime.CompilerServices;
using UnityEngine;

public class TrapSaw : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private Vector3 target;
    private Rigidbody2D rb;
    Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = pointB.position;
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
}
