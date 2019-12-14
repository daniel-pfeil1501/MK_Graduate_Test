using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private GameStateMananger gameStateManager;

    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource inGameMusic;

    private void Start()
    {
        gameStateManager.mainMenuEvent += OnMainMenu;
        gameStateManager.startEvent += OnGameStart;
    }

    public void OnMainMenu()
    {
        if (inGameMusic.isPlaying) { inGameMusic.Stop(); }

        mainMenuMusic.Play();
    }

    public void OnGameStart()
    {
        mainMenuMusic.Stop();
        inGameMusic.Play();
    }
}
