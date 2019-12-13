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
    [SerializeField] private List<ObjectPoolItem> pooledItems;
    public static ObjectPooler SharedInstance;



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

    public GameObject GetObjectFromPool(string itemName)
    {
        itemName += "(Clone)";

        if (pool == null)
        {
            GeneratePool();
        }
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy && pool[i].name == itemName)
            {

                return pool[i];
            }
        }
        return null;
    }

    private void GeneratePool()
    {
        pool = new List<GameObject>();
        for(int i = 0;i < pooledItems.Count; i++)
        {
            for(int j = 0;j < pooledItems[i].amountToPool; j++)
            {
                GameObject o = Instantiate(pooledItems[i].objectToPool);
                o.SetActive(false);
                pool.Add(o);
            }
        }
    }
}