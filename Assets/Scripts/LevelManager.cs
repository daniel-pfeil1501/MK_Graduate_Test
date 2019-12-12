using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float score;

    private ObjectPooler objectPooler;

    private List<GameObject> platformList;
    private Vector3 platformSpawnLocation;

    private float deltaX;
    private float platformLength;


    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private int maxActivePlatforms;
    [SerializeField] private int numberOfInitialPlatforms;

    private void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        platformList = new List<GameObject>();
        platformSpawnLocation = new Vector3(0, -2, 0);
        deltaX = 0;

        GameObject initialPlatform = objectPooler.GetObjectFromPool();
        initialPlatform.transform.position = platformSpawnLocation;
        initialPlatform.SetActive(true);
        //platformLength = initalPlatform.GetComponent<Collider2D>().bounds.size.x;
        platformLength = initialPlatform.transform.localScale.x;

        platformList.Add(initialPlatform);

        for (int i = 1; i < numberOfInitialPlatforms; i++)
        {
            initialPlatform = objectPooler.GetObjectFromPool();
            initialPlatform.transform.position = platformSpawnLocation + Vector3.right * i * platformLength;
            initialPlatform.SetActive(true);
            platformList.Add(initialPlatform);
        }
    }

    private void Update()
    {

        deltaX += horizontalMoveSpeed * Time.deltaTime;
        score += horizontalMoveSpeed * Time.deltaTime;
        Debug.Log(score);

        if (deltaX >= platformLength)
        {
            float excess = deltaX - platformLength;

            GameObject newPlatform = objectPooler.GetObjectFromPool();
            //newPlatform.transform.position = platformSpawnLocation + Vector3.right * platformLength * (numberOfInitialPlatforms - 1);
            newPlatform.transform.position = platformList[platformList.Count - 1].transform.position + (Vector3.right * (platformLength + excess));
            newPlatform.SetActive(true);
            platformList.Add(newPlatform);
            deltaX = 0;
        }

        if (platformList.Count > maxActivePlatforms)
        {
            platformList[0].SetActive(false);
            platformList.RemoveAt(0);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < platformList.Count; i++)
        {
            platformList[i].transform.Translate(new Vector3(-horizontalMoveSpeed * Time.deltaTime, 0, 0));
        }
    }
}
