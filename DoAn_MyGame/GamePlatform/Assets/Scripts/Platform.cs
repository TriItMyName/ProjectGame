using UnityEngine;

public class Platform : MonoBehaviour
{
    PlayerControllers playerControllers;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length > 0)
        {
            playerControllers = playerObjects[0].GetComponent<PlayerControllers>();
        }
        rb = GetComponent<Rigidbody2D>();
    
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
