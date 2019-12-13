using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> powerupPrefabs;
    [SerializeField] private float powerupSpawnInterval;
    private float powerupSpawnTimer;

    private List<GameObject> powerupPool;
    private List<GameObject> activePowerups;

    private void Start()
    {
        activePowerups = new List<GameObject>();
        PoolPowerups();
    }

    private void Update()
    {
        powerupSpawnTimer += Time.deltaTime;


        if (powerupSpawnTimer >= powerupSpawnInterval)
        {

            GameObject p = SpawnPowerup();
            powerupSpawnTimer = 0;
            if(p != null)
            {
                p.SetActive(true);
                p.transform.position = transform.position;
                activePowerups.Add(p);
            }
        }


    }

    private void FixedUpdate()
    {

    }

    private void PoolPowerups()
    {
        powerupPool = new List<GameObject>();
        for(int i =0;i < powerupPrefabs.Count; i++)
        {
            GameObject p = Instantiate(powerupPrefabs[i]);
            p.SetActive(false);
            powerupPool.Add(p);
        }
    }

    private GameObject SpawnPowerup()
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
}

