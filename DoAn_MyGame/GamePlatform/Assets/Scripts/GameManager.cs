using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false;
    private bool isGameWin = false;

    void Start()
    {
        ResetUI();
        UpdateScoreText();
    }

    public void GoToMenu()
    {
        ResetGameState();
        SceneManager.LoadScene("Menu");
    }

    public void AddScore(int scoreToAdd)
    {
        if (isGameOver || isGameWin) return;
        score += scoreToAdd;
        UpdateScoreText();
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void RestartLevel()
    {
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        ResetGameState();
        string nextLevelName = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(nextLevelName);
    }

    public void GameWin()
    {
        isGameWin = true;
        score = 0;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
    }

    public bool IsGameOver() => isGameOver;
    public bool IsGameWin() => isGameWin;

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private void ResetUI()
    {
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    private void ResetGameState()
    {
        isGameOver = false;
        isGameWin = false;
        score = 0;
        Time.timeScale = 1;
        UpdateScoreText();
        ResetUI();
    }
}
