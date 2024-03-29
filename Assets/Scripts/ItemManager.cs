﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public enum itemType { climb, catchUp, bomb, NumberOfTypes}         //final enum is used to detect the number of available items

    public delegate void PickupItemDelegate(itemType type);             //Announces when an item is picked up by the player.
    public event PickupItemDelegate puPickupEvent;

    public delegate void ItemExpiredDelegate(itemType type);            //Announces when an item expires.
    public event ItemExpiredDelegate expiredEvent;

    [SerializeField] private GameStateMananger gameStateManager;

    [SerializeField] private Slider climbProgress;                      //Sliders the remaining duration of the power-ups on the in-game UI.
    [SerializeField] private Slider catchUpProgress;

    [SerializeField] private float maxDuration;                         //Picking up a power up while the same one is already active increases the time up to this amount.

    private float[] powerUpTimers;                                      //Arrays to store various information about items collected.
    private bool[] activePowerUps;
    private int[] itemsCollected;

    private void Start()
    {
        int length = (int)itemType.NumberOfTypes;
        itemsCollected = new int[length];
        powerUpTimers = new float[length];
        activePowerUps = new bool[length];
    }

    private void OnEnable()
    {
        gameStateManager.gameOverEvent += SendTotalItemsCollected;
    }

    private void OnDisable()
    {
        gameStateManager.gameOverEvent -= SendTotalItemsCollected;
    }

    private void Update()
    {
        for(int i = 0; i < powerUpTimers.Length; i++)
        {
            if(powerUpTimers[i] > 0)
            {
                UpdateProgressBars();
                powerUpTimers[i] -= Time.deltaTime;

            }
            else if (activePowerUps[i])
            {
                powerUpTimers[i] = 0;
                activePowerUps[i] = false;
                PowerUpExpired((itemType)i);
            }
        }
    }

    //Receives and processes information about items picked up.
    public void ItemCollected(itemType type, float duration)
    {
        int i = (int)type;

        itemsCollected[i]++;
        powerUpTimers[i] += duration;


        if(powerUpTimers[i] > maxDuration) { powerUpTimers[i] = maxDuration; }

        if(puPickupEvent != null && !activePowerUps[i])
        {
            activePowerUps[i] = true;
            puPickupEvent(type);
        }
    }

    public void PowerUpExpired(itemType type)
    {
        if(expiredEvent != null)
        {
            expiredEvent(type);
        }
    }

    private void UpdateProgressBars()
    {
        climbProgress.value = powerUpTimers[(int)itemType.climb] / maxDuration;

        catchUpProgress.value = powerUpTimers[(int)itemType.catchUp] / maxDuration;

    }

    //At the end of the game sends total items collected to the UI to display and calculate score.
    public void SendTotalItemsCollected()                   
    {
        FindObjectOfType<UIManager>().SetItemsCollected(itemsCollected);

        catchUpProgress.value = 0;
        climbProgress.value = 0;

        for (int i = 0; i < itemsCollected.Length; i++)
        {
            itemsCollected[i] = 0;
            powerUpTimers[i] = 0;
            activePowerUps[i] = false; 
        }
    }
}

