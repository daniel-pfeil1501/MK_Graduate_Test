using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float[] powerUpTimers;
    private bool[] activePowerUps;
    private int[] itemsCollected;

    private void Start()
    {
        int length = (int)powerUpType.NumberOfTypes;
        itemsCollected = new int[length];
        powerUpTimers = new float[length];
        activePowerUps = new bool[length];

        gameStateManager.gameOverEvent += SendTotalItemsCollected;
    }

    private void Update()
    {
        for(int i = 0; i < powerUpTimers.Length; i++)
        {
            if(powerUpTimers[i] > 0)
            {
                powerUpTimers[i] -= Time.deltaTime;
                Debug.Log(powerUpTimers[i]);
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
        itemsCollected[(int)type]++;
        activePowerUps[(int)type] = true;
        powerUpTimers[(int)type] += duration;

        if(puPickupEvent != null)
        {
            puPickupEvent(type);
        }
    }

    public void PowerUpExpired(powerUpType type)
    {
        Debug.Log(name + " Expired");
        if(expiredEvent != null)
        {
            expiredEvent(type);
        }
    }

    public void SendTotalItemsCollected()
    {
        FindObjectOfType<UIManager>().SetItemsCollected(itemsCollected);
    }
}

