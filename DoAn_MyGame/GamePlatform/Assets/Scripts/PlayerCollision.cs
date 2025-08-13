using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private HealthManager health;
    private CheckGetDamePlayer checkDame;

    private void Awake()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        audioManager = Object.FindFirstObjectByType<AudioManager>();
        health = Object.FindFirstObjectByType<HealthManager>();
        checkDame = Object.FindFirstObjectByType<CheckGetDamePlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            audioManager.PlayCoinSound();
            gameManager.AddScore(10);
        }
        else if (collision.CompareTag("Trap"))
        {
            if (health != null)
            {
                health.TakeDamage(1);
                audioManager.PlayHitSound();
                if (health.currentHealth <= 0)
                {
                    gameManager.GameOver();
                }
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (health != null)
            {
                health.TakeDamage(1);
                audioManager.PlayHitSound();
                if (health.currentHealth <= 0)
                {
                    gameManager.GameOver();
                }
            }
        }
        else if (collision.CompareTag("Bullet"))
        {
            if (health != null)
            {
                health.TakeDamage(1);
                audioManager.PlayHitSound();
                if (health.currentHealth <= 0)
                {
                    gameManager.GameOver();
                }
            }
        }
        else if (collision.CompareTag("Boss"))
        {
            if (health != null)
            {
                health.TakeDamage(1);
                audioManager.PlayHitSound();
                if (health.currentHealth <= 0)
                {
                    gameManager.GameOver();
                }
            }
        }
        else if (collision.CompareTag("Water"))
        {
            if (health != null)
            {
                health.TakeDamage(1);
                audioManager.PlayHitSound();
                if (health.currentHealth <= 0)
                {
                    gameManager.GameOver();
                }
            }
        }
        else if (collision.CompareTag("Heart"))
        {
            if (health != null)
            {
                health.Heal(1);
                audioManager.PlayHealSound();
                Destroy(collision.gameObject);
            }
        }
        else if (collision.CompareTag("Key"))
        {
            UnClockNewLevel();
            Destroy(collision.gameObject);
            gameManager.GameWin();
        }
    }

    void UnClockNewLevel()
    {
        Debug.Log("UnClockNewLevel called");
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}

