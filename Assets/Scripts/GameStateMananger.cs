using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMananger : MonoBehaviour
{
    public delegate void OnGameOverDelegate();
    public event OnGameOverDelegate gameOverEvent;

    public delegate void OnRestartDelegate();
    public event OnRestartDelegate restartEvent;

    private bool gameOver;

    private PolygonCollider2D collider;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text runDistanceText;
    [SerializeField] private Text collectedText;
    [SerializeField] private Text climbsCollectedText;
    [SerializeField] private Text catchUpsCollectedText;

    [SerializeField] private Canvas canvas;

    private Color colour;
    [SerializeField] private float alphaIncreaseRate;


    void Start()
    {
        DisableAllElements();

        colour = gameOverText.color;
        colour.a = 0;

        collider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (gameOver)
        {
            if (colour.a < 1)
            {
                colour.a += alphaIncreaseRate * Time.deltaTime;
                gameOverText.color = colour;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvas.enabled = true;
            gameOverText.enabled = true;
            gameOver = true;
            if(gameOverEvent != null)
            {
                gameOverEvent();
            }
        }
    }

    private void DisableAllElements()
    {
        canvas.enabled = false;

        gameOverText.enabled = false;
        runDistanceText.enabled = false;
        collectedText.enabled = false;
        climbsCollectedText.enabled = false;
        catchUpsCollectedText.enabled = false;
    }

    public void RestartLevel()
    {
        DisableAllElements();
        if(restartEvent != null)
        {
            restartEvent();
        }
    }

    public void SetRunDistance(int dist)
    {
        runDistanceText.enabled = true;
        runDistanceText.text = "You ran: " + dist + " meters.";
    }

    public void SetItemsCollected(int[] amount)
    {
        collectedText.enabled = true;
        climbsCollectedText.enabled = true;
        catchUpsCollectedText.enabled = true;
        climbsCollectedText.text = "x " + amount[0].ToString();
        catchUpsCollectedText.text = "x " + amount[1].ToString();
    }
}
