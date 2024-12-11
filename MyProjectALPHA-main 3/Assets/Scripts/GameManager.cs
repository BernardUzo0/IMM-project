using System.Collections;
using UnityEngine;
using TMPro;  // Add this for TextMesh Pro
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    private float incrementInterval = 0.026f;
    private bool isGameOver = false;
    private bool isGameStarted = false;

    public Button restartButton;
    public Button startButton;

    public float speedMultiplier = 1.50f;

    void Start()
    {
        score = 0;
        UpdateScoreUI();
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }

    private IEnumerator IncrementScore()
    {
        while (!isGameOver && isGameStarted)
        {
            yield return new WaitForSeconds(incrementInterval);
            score += 1;
            UpdateScoreUI();

            if (score % 350 == 0)
            {
                IncreaseGameSpeed();
            }
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    private void IncreaseGameSpeed()
    {
        speedMultiplier += 0.7f; // Increase game speed
        Debug.Log("Speed increased! Current multiplier: " + speedMultiplier);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        speedMultiplier = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<PlayerController>().ResetPlayer();
    }

    public void StartGame()
    {
        isGameStarted = true;
        startButton.gameObject.SetActive(false);
        isGameOver = false;
        score = 0;
        UpdateScoreUI();
        StartCoroutine(IncrementScore());

        FindObjectOfType<PlayerController>().EnablePlayerAnimator(); // Enable animations
    }

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    public int Score // Public property to get the score
    {
        get { return score; }
    }
}
