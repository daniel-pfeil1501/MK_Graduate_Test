using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
}
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private ObjectPoolItem[] pooledItems;
    public static ObjectPooler SharedInstance;

    private List<GameObject> pool;
    private string[] platformNames;
    private void OnEnable()
    {
        SharedInstance = this;
        GeneratePool();
    }

    //Retrives and returns an item from the pool with the name given.
    public GameObject GetObjectFromPool(string itemName)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy && pool[i].name == itemName)
            {

                return pool[i];
            }
        }
        return null;
    }

    //Generates a pool of all supplied platforms.
    private void GeneratePool()
    {
        platformNames = new string[pooledItems.Length];
        pool = new List<GameObject>();

        for(int i = 0;i < pooledItems.Length; i++)
        {
            for(int j = 0;j < pooledItems[i].amountToPool; j++)
            {
                GameObject o = Instantiate(pooledItems[i].objectToPool);
                platformNames[i] = o.name;
                o.SetActive(false);
                pool.Add(o);
            }
        }
    }

    public string[] GetPlatformNames()
    {
        return platformNames;
    }
}