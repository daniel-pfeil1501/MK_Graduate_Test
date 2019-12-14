﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    public enum powerUpType { climb, catchUp, NumberOfTypes} //final enum is used to detect the number of available power-ups

    public delegate void PickupPowerupDelegate(powerUpType type);
    public event PickupPowerupDelegate puPickupEvent;

    public delegate void PowerUpExpiredDelegate(powerUpType type);
    public event PowerUpExpiredDelegate expiredEvent;

    public delegate void EndPowerupDelegate(powerUpType type);
    public event EndPowerupDelegate puEndEvent;

    [SerializeField] private GameStateMananger gameStateManager;

    [SerializeField] private Slider climbProgress;
    [SerializeField] private Slider catchUpProgress;

    [SerializeField] private float maxDuration;

    private float[] powerUpTimers;
    private bool[] activePowerUps;
    private int[] itemsCollected;

    private void Start()
    {
        int length = (int)powerUpType.NumberOfTypes;
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
                PowerUpExpired((powerUpType)i);
            }
        }
    }


    public void PowerUpCollected(powerUpType type, float duration)
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

    public void PowerUpExpired(powerUpType type)
    {
        if(expiredEvent != null)
        {
            expiredEvent(type);
        }
    }

    private void UpdateProgressBars()
    {
        climbProgress.value = powerUpTimers[(int)powerUpType.climb] / maxDuration;

        catchUpProgress.value = powerUpTimers[(int)powerUpType.catchUp] / maxDuration;

    }

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

