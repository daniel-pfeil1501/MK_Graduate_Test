using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameStateMananger))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameStateMananger gameStateMananger;
    [SerializeField] private ItemManager powerUpManager;
    [SerializeField] private LevelManager levelManager;

    //Main Menu text.

    [SerializeField] private Text mainMenuHighScoreText;

    //Text used for the Game Over UI.

    [SerializeField] private Text gameOverText;
    [SerializeField] private Text runDistanceText;
    [SerializeField] private Text collectedText;
    [SerializeField] private Text climbsCollectedText;
    [SerializeField] private Text catchUpsCollectedText;
    [SerializeField] private Text itemBonusText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    //Elements used in the in game UI.

    [SerializeField] private Text currentDistanceText;
    [SerializeField] private Image climbSpeedBuffIcon;
    [SerializeField] private Slider climbSpeedSlider;
    [SerializeField] private Image catchUpBuffIcon;
    [SerializeField] private Slider catchUpSlider;

    //A canvas for each state of the game that requires one.

    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas inGameCanvas;


    private bool gameOver;
    private int score;


    private void Start()
    {
        score = 0;
        gameOver = false;

        DisplayHighScore();

        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = false;

        climbSpeedBuffIcon.enabled = false;
        climbSpeedSlider.gameObject.SetActive(false);
        catchUpBuffIcon.enabled = false;
        catchUpSlider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        gameStateMananger.gameOverEvent += OnGameOver;
        gameStateMananger.startEvent += OnGameStart;
        gameStateMananger.restartEvent += OnGameRestart;
        gameStateMananger.mainMenuEvent += OnMainMenu;

        powerUpManager.puPickupEvent += PowerUpCollected;
        powerUpManager.expiredEvent += PowerUpExpired;
    }

    private void OnDisable()
    {
        gameStateMananger.gameOverEvent -= OnGameOver;
        gameStateMananger.startEvent -= OnGameStart;
        gameStateMananger.restartEvent -= OnGameRestart;
        gameStateMananger.mainMenuEvent -= OnMainMenu;

        powerUpManager.puPickupEvent -= PowerUpCollected;
        powerUpManager.expiredEvent -= PowerUpExpired;
    }

    private void Update()
    {
        currentDistanceText.text = string.Format("Distance: {0,4}", levelManager.GetCurrentDistance());

        if(gameOver && highScoreText.enabled == false)
        {
            CheckIfHighScore();
        }
    }

    public void OnGameOver()
    {
        gameOver = true;
        currentDistanceText.enabled = false;
        gameOverCanvas.enabled = true;

        catchUpBuffIcon.enabled = false;
        climbSpeedBuffIcon.enabled = false;
        climbSpeedSlider.gameObject.SetActive(false);
        catchUpSlider.gameObject.SetActive(false);
    }

    public void OnGameStart()
    {
        score = 0;
        inGameCanvas.enabled = true;

        currentDistanceText.enabled = true; ;
        highScoreText.enabled = false;
        mainMenuCanvas.enabled = false;
    }

    public void OnGameRestart()
    {
        score = 0;
        currentDistanceText.enabled = true; ;
        highScoreText.enabled = false;
        gameOverCanvas.enabled = false;

        climbSpeedSlider.gameObject.SetActive(false);
        catchUpSlider.gameObject.SetActive(false);
    }

    public void OnMainMenu()
    {
        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }

    public void SetRunDistance(int dist)
    {
        runDistanceText.enabled = true;
        runDistanceText.text = string.Format("You ran: {0,25} meters", (int)dist);
        score += dist;
    }

    //Gets the total items collects for score and display.
    public void SetItemsCollected(int[] amount)
    {
        int itemBonus = 0;
        collectedText.enabled = true;
        climbsCollectedText.enabled = true;
        catchUpsCollectedText.enabled = true;
        climbsCollectedText.text = "x " + amount[0].ToString();
        catchUpsCollectedText.text = "x " + amount[1].ToString();

        itemBonus += (amount[0] + amount[1]) * 10;
        itemBonusText.text = string.Format("Item Bonus: {0,31}", itemBonus);
        score += itemBonus;
    }

    private void CheckIfHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        scoreText.text = string.Format("Total Score: {0,30}", score);

        if(score > highScore)
        {
            highScoreText.enabled = true;
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    private void DisplayHighScore()
    {
        mainMenuHighScoreText.text = string.Format("High score: {0,5}", PlayerPrefs.GetInt("HighScore"));
    }

    //Manages UI elements for power-ups.
    public void PowerUpCollected(ItemManager.itemType type)
    {
        if(type == ItemManager.itemType.climb)
        {
            climbSpeedSlider.gameObject.SetActive(true);
            climbSpeedBuffIcon.enabled = true;
        }

        if(type == ItemManager.itemType.catchUp)
        {
            catchUpSlider.gameObject.SetActive(true);
            catchUpBuffIcon.enabled = true;
        }
    }

    public void PowerUpExpired(ItemManager.itemType type)
    {
        if (type == ItemManager.itemType.climb)
        {
            climbSpeedSlider.gameObject.SetActive(false);
            climbSpeedBuffIcon.enabled = false;
        }

        if (type == ItemManager.itemType.catchUp)
        {
            catchUpSlider.gameObject.SetActive(false);
            catchUpBuffIcon.enabled = false;
        }
    }



}
