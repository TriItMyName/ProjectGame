using UnityEngine;

public class TrapFire : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D damageCollider;
    [SerializeField] private float switchTime;

    private bool isOn = false;
    private float timer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        SetTrapState(isOn);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= switchTime)
        {
            isOn = !isOn;
            SetTrapState(isOn);
            timer = 0f;
        }
    }

    private void SetTrapState(bool on)
    {
        if (animator != null)
            animator.SetBool("IsOn", on);
        if (damageCollider != null)
            damageCollider.enabled = on;
    }
}
