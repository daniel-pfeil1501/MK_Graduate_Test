using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{

    [SerializeField] private GameStateMananger gameStateManager;

    [SerializeField] private List<GameObject> powerupPrefabs;

    private List<GameObject> powerupPool;

    private PlayerController controller;
    private LevelManager levelManager;

    public static PowerupManager SharedInstance;

    public delegate void EndPowerupDelegate(string powerupName);
    public event EndPowerupDelegate puEndEvent;

    private int[] itemsCollected;

    private void Start()
    {
        //FindObjectOfType<PlatformWithPowerup>().puPickupEvent += PowerUpCollected;
        //FindObjectOfType<Powerup>().puEndEvent += PowerUpExpired;

        gameStateManager.gameOverEvent += SendTotalItemsCollected;

        controller = FindObjectOfType<PlayerController>();
        levelManager = FindObjectOfType<LevelManager>();

        PoolPowerups();

        SharedInstance = this;
    }

    private void PoolPowerups()
    {
        int numberOfItems = 0;
        powerupPool = new List<GameObject>();
        for(int i =0;i < powerupPrefabs.Count; i++)
        {
            GameObject p = Instantiate(powerupPrefabs[i]);
            p.SetActive(false);
            powerupPool.Add(p);
            numberOfItems++;
        }
        itemsCollected = new int[numberOfItems];
    }

    public GameObject RequestPowerup()
    {
        for(int i = 0;i < powerupPool.Count; i++)
        {
            if (!powerupPool[i].activeInHierarchy)
            {
                return powerupPool[i];
            }
        }
        return null;
    }

    public void PowerUpCollected(string name, float duration)
    {
        Debug.Log(name + " Collected");
        if(name == "climbSpeed") { itemsCollected[0]++; }
        if(name == "catchUpSpeed") { itemsCollected[1]++; }
    }

    public void PowerUpExpired(string name)
    {
        Debug.Log(name + " Expired");
    }

    public void SendTotalItemsCollected()
    {
        FindObjectOfType<UIManager>().SetItemsCollected(itemsCollected);
    }
}

