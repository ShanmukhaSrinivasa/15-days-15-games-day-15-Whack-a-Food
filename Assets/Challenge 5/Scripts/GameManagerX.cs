using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class GameManagerX : MonoBehaviour
{
    public static GameManagerX Instance;

    public List<GameObject> targetPrefabs;

    private int score;
    private int highScore;
    [SerializeField] float remainingTime;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private TextMeshProUGUI highScoreCount;
    [SerializeField] private TextMeshProUGUI goScoreCount;
    [SerializeField] private TextMeshProUGUI goHighScoreCount;
    [SerializeField] private TextMeshProUGUI timerText;
    private float spawnRate = 1.5f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square

    [Header("Canvas Groups")]
    [SerializeField] public CanvasGroup startGameCG;
    [SerializeField] public CanvasGroup gameCG;
    [SerializeField] public CanvasGroup gameOverCG;

    [SerializeField] private Image redImageOverlay;
    private float flashDuration = 0.5f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        ShowCG(gameCG);
        HideCG(startGameCG);
        HideCG(gameOverCG);

        highScore = PlayerPrefs.GetInt("highScore", 0);
        highScoreCount.text = highScore.ToString();

        spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        UpdateScore(0);
    }

    private void Update()
    {
        if (isGameActive)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if(remainingTime < 0)
            {
                remainingTime = 0;
                GameOver();
                timerText.color = Color.red;
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
            
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreCount.text = score.ToString();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
            highScoreCount.text = highScore.ToString();
        }
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        HideCG(gameCG);
        ShowCG(gameOverCG);
        isGameActive = false;
        goScoreCount.text = score.ToString();
        goHighScoreCount.text = highScore.ToString();
    }

    public void RedFlash()
    {
        StartCoroutine(RedFlashRoutine());
    }

    IEnumerator RedFlashRoutine()
    {
        redImageOverlay.color = new Color(1, 0, 0, 0.15f);

        yield return new WaitForSeconds(flashDuration);

        redImageOverlay.color = new Color(1, 0, 0, 0f);
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void HideCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

}
