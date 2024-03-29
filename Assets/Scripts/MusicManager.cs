﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Changes the current music based on game state events.
public class MusicManager : MonoBehaviour
{
    [SerializeField] private GameStateMananger gameStateManager;

    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource inGameMusic;

    private void OnEnable()
    {
        gameStateManager.mainMenuEvent += OnMainMenu;
        gameStateManager.startEvent += OnGameStart;
    }

    private void OnDisable()
    {
        gameStateManager.mainMenuEvent -= OnMainMenu;
        gameStateManager.startEvent -= OnGameStart;
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
