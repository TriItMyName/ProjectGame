using System.Collections;
using UnityEngine;

public class CheckGetDamePlayer : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private bool isPlayerInTrigger = false;
    private bool hasBeenHit = false;
    private bool canCheckDame = false;

    private void Awake()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        canCheckDame = true;
    }

    public bool CheckDame()
    {
        if (isPlayerInTrigger && playerRb != null && canCheckDame)
        {
            if (playerRb.linearVelocity.y <= 0 && playerRb.transform.position.y > transform.position.y + 0.05f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenHit)
        {
            isPlayerInTrigger = true;

            if (CheckDame())
            {
                hasBeenHit = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenHit)
        {
            if (CheckDame())
            {
                hasBeenHit = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }



    public void ResetState()
    {
        isPlayerInTrigger = false;
        hasBeenHit = false;
    }
}
