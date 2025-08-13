using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool IsGameOver = false;
    private bool IsGameWin = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScoreText();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void AddScore(int scoreToAdd)
    {
        if (!IsGameOver && !IsGameWin)
        {
            score += scoreToAdd;
            UpdateScoreText();
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        IsGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        IsGameOver = false;
        Time.timeScale = 1;
        score = 0;
        UpdateScoreText();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void GameWin()
    {
        IsGameWin = true;
        score = 0;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
    }

    public bool isGameOver()
    {
        return IsGameOver;
    }

    public bool isGameWin()
    {
        return IsGameWin;
    }
}
