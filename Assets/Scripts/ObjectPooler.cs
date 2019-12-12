using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    [SerializeField]
    private GameObject objectToPool;
    [SerializeField]
    private int amountToPool = 5;

    private List<GameObject> pool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        if (pool == null)
        {
            GeneratePool();
        }
    }

    public GameObject GetObjectFromPool()
    {
        if (pool == null)
        {
            GeneratePool();
        }
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    private void GeneratePool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject o = Instantiate(objectToPool);
            o.SetActive(false);
            pool.Add(o);
        }
    }
}