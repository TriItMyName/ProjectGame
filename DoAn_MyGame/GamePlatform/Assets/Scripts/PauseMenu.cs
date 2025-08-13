using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
    }

    public void Home()
    {
        ResetPersistentData();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        ResetPersistentData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero; 
            player.transform.position = Vector3.zero; 
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ResetPersistentData()
    {
    }

}
