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

    [SerializeField] private List<GameObject> platformsToUse;
    private Dictionary<int, string> platformDictionary;

    private void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        platformList = new List<GameObject>();
        GenerateDictionary();



        platformSpawnLocation = new Vector3(0, -2, 0);
        deltaX = 0;

        GameObject initialPlatform = objectPooler.GetObjectFromPool("basic_platform");
        initialPlatform.transform.position = platformSpawnLocation;
        initialPlatform.SetActive(true);
        //platformLength = initalPlatform.GetComponent<Collider2D>().bounds.size.x;
        platformLength = initialPlatform.transform.localScale.x;

        platformList.Add(initialPlatform);

        for (int i = 1; i < numberOfInitialPlatforms; i++)
        {
            initialPlatform = objectPooler.GetObjectFromPool("basic_platform");
            initialPlatform.transform.position = platformSpawnLocation + Vector3.right * i * platformLength;
            initialPlatform.SetActive(true);
            platformList.Add(initialPlatform);
        }
    }

    private void Update()
    {

        deltaX += horizontalMoveSpeed * Time.deltaTime;
        score += horizontalMoveSpeed * Time.deltaTime;

        if (deltaX >= platformLength)
        {
            float excess = deltaX - platformLength;

            GameObject newPlatform = objectPooler.GetObjectFromPool(platformDictionary[Random.Range(0, platformsToUse.Count)]);
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

    private void GenerateDictionary()
    {
        platformDictionary = new Dictionary<int, string>();

        for(int i = 0;i< platformsToUse.Count; i++)
        {
            platformDictionary.Add(i, platformsToUse[i].name);
        }
    }
}
