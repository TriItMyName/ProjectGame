using UnityEngine;

public class CheckBossGetDame : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private float lastHitTime = -1f;
    public float hitCooldown = 2f;

    private void Awake()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    public bool CanTakeDamage()
    {
        if (playerRb == null) return false;

        bool isFalling = playerRb.linearVelocity.y <= 0f;
        bool isAbove = playerRb.transform.position.y > transform.position.y + 0.05f;
        bool isCooldownOver = Time.time - lastHitTime >= hitCooldown;

        if (isFalling && isAbove && isCooldownOver)
        {
            lastHitTime = Time.time;
            return true;
        }

        return false;
    }
}

