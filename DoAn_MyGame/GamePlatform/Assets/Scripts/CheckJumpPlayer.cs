using System.Collections;
using UnityEngine;

public class CheckJumpPlayer : MonoBehaviour
{   
    [SerializeField]private float bounceForce;
    public Rigidbody2D playerRb;
    public PlayerControllers playerControllers;
    private bool isPlayerInTrigger = false;
    private bool canCheckDame = false;
    

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerControllers = playerRb.GetComponent<PlayerControllers>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        canCheckDame = true;
    }

    // Update is called once per frame
    public bool CheckJump()
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

    public void UpdateAnimation( bool isBounce, bool isIdle)
    {
        animator.SetBool("IsBounce", isBounce);
        animator.SetBool("IsIdle", isIdle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            if (CheckJump())
            {
                playerControllers.Bounce(bounceForce);
                UpdateAnimation(true,false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            UpdateAnimation(false, true);
        }
    }
}
