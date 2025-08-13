using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool isInvincible = false;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;


        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(GetHurt());
        }
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
    }

    IEnumerator GetHurt()
    {
        isInvincible = true;
        Physics2D.IgnoreLayerCollision(6, 8);
        GetComponent<Animator>().SetLayerWeight(1, 1);
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().SetLayerWeight(1, 0);
        Physics2D.IgnoreLayerCollision(6, 8, false);
        isInvincible = false;
    }
}
