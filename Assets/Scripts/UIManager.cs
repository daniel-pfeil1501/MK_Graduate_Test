using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameStateMananger))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameStateMananger gameStateMananger;

    private Color colour;
    [SerializeField] private float alphaIncreaseRate;

    [SerializeField] private Text gameOverText;
    [SerializeField] private Text runDistanceText;
    [SerializeField] private Text collectedText;
    [SerializeField] private Text climbsCollectedText;
    [SerializeField] private Text catchUpsCollectedText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas gameOverCanvas;

    private bool gameOver;
    private int score;

    private void Start()
    {
        score = 0;
        gameOver = false;
        gameStateMananger.gameOverEvent += OnGameOver;
        gameStateMananger.startEvent += OnGameStart;
        gameStateMananger.restartEvent += OnGameRestart;
        gameStateMananger.mainMenuEvent += OnMainMenu;

        gameOverCanvas.enabled = false;

        colour = gameOverText.color;
        colour.a = 0;
    }

    private void Update()
    {
        if (colour.a < 1)
        {
            colour.a += alphaIncreaseRate * Time.deltaTime;
            gameOverText.color = colour;
        }
        else
        {
            CheckIfHighScore();
        }

    }

    public void OnGameOver()
    {
        gameOver = true;
        gameOverCanvas.enabled = true;
    }

    public void OnGameStart()
    {
        score = 0;
        highScoreText.enabled = false;
        mainMenuCanvas.enabled = false;
    }

    public void OnGameRestart()
    {
        score = 0;
        highScoreText.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void OnMainMenu()
    {
        gameOverCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }

    public void SetRunDistance(int dist)
    {
        runDistanceText.enabled = true;
        runDistanceText.text = "You ran: " + dist + " meters.";
        score += dist;
    }

    public void SetItemsCollected(int[] amount)
    {
        collectedText.enabled = true;
        climbsCollectedText.enabled = true;
        catchUpsCollectedText.enabled = true;
        climbsCollectedText.text = "x " + amount[0].ToString();
        catchUpsCollectedText.text = "x " + amount[1].ToString();

        score += amount[0] * 10;
        score += amount[1] * 10;
    }

    private void CheckIfHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        scoreText.text = "Score: " + score.ToString();

        if(score > highScore)
        {
            highScoreText.enabled = true;
            PlayerPrefs.SetInt("HighScore", score);
        }
    }



}
