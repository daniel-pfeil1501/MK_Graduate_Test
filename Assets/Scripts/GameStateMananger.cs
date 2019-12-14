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

    public delegate void OnStartDelegate();
    public event OnStartDelegate startEvent;

    public delegate void OnMainMenuDelegate();
    public event OnMainMenuDelegate mainMenuEvent;

    private PolygonCollider2D collider;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelManager;


    void Start()
    {
        player.SetActive(false);
        levelManager.SetActive(false);

        collider = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.SetActive(false);
            if(gameOverEvent != null)
            {
                gameOverEvent();
            }
        }
    }

    public void StartGame()
    {
        player.SetActive(true);
        levelManager.SetActive(true);
        if(startEvent != null)
        {
            startEvent();
        }
    }

    public void RestartLevel()
    {
        player.SetActive(true);
        if(restartEvent != null)
        {
            restartEvent();
        }
    }

    public void MainMenu()
    {
        levelManager.SetActive(false);
        player.SetActive(false);
        if(mainMenuEvent != null)
        {
            mainMenuEvent();
        }
        if (restartEvent != null) { restartEvent(); }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
