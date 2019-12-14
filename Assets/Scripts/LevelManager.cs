using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private float totalDistance;

    private ObjectPooler objectPooler;

    [SerializeField] GameStateMananger gameStateManager;
    [SerializeField] Text currentDistanceText;

    private List<GameObject> activePlatforms;
    private GameObject initialPlatform;
    private Vector3 platformSpawnLocation;

    private float deltaX;
    private float platformLength;

    bool gameOver = false;


    [SerializeField] private float horizontalMoveSpeed;

    [SerializeField] private int maxActivePlatforms;
    [SerializeField] private int numberOfInitialPlatforms;

    private string[] platformLookup;

    private void Start()
    {
        platformLength = 0;


        objectPooler = ObjectPooler.SharedInstance;
        activePlatforms = new List<GameObject>();
        platformLookup = objectPooler.GetPlatformNames();

        platformSpawnLocation = new Vector3(0, -2, 0);

        StartLevel();
    }

    private void OnEnable()
    {
        gameStateManager.startEvent += RestartLevel;
        gameStateManager.gameOverEvent += SendTotalDistanceRun;
        gameStateManager.restartEvent += RestartLevel;
    }

    private void OnDisable()
    {
        gameStateManager.startEvent -= RestartLevel;
        gameStateManager.gameOverEvent -= SendTotalDistanceRun;
        gameStateManager.restartEvent -= RestartLevel;
    }

    private void Update()
    {
        TrackDistance();

        if (deltaX >= platformLength)
        {
            RequestNewPlatform();
        }

        //Remove the oldest platform when the total exceeds the limit set.
        if (activePlatforms.Count > maxActivePlatforms)
        {
            activePlatforms[0].SetActive(false);
            activePlatforms.RemoveAt(0);
        }
    }

    private void FixedUpdate() //Move all platforms active in the level.
    {
        if (!gameOver)
        {
            for (int i = 0; i < activePlatforms.Count; i++)
            {
                activePlatforms[i].transform.Translate(new Vector3(-horizontalMoveSpeed * Time.deltaTime, 0, 0));
            }
        }

    }

    private void TrackDistance() //Track the distance moved for platform spawning and score.
    {
        if (!gameOver)
        {
            deltaX += horizontalMoveSpeed * Time.deltaTime;
            totalDistance += horizontalMoveSpeed * Time.deltaTime;
            currentDistanceText.text = string.Format("Distance: {0,4}", (int)totalDistance);
        }
    }

    private void RequestNewPlatform()
    {
        float excess = deltaX - platformLength;
        deltaX = 0;

        GameObject newPlatform = objectPooler.GetObjectFromPool(platformLookup[Random.Range(0, platformLookup.Length)]);
        newPlatform.transform.position = activePlatforms[activePlatforms.Count - 1].transform.position + (Vector3.right * (platformLength + excess));
        newPlatform.SetActive(true);
        activePlatforms.Add(newPlatform);
    }


    private void StartLevel()
    {
        gameOver = false;
        deltaX = 0;
        totalDistance = 0;

        for (int i = 0; i < numberOfInitialPlatforms; i++)
        {
            initialPlatform = objectPooler.GetObjectFromPool(platformLookup[0]);
            initialPlatform.transform.position = platformSpawnLocation + Vector3.right * i * (platformLength - 0.05f);
            initialPlatform.SetActive(true);

            if (platformLength == 0)
            {
                platformLength = initialPlatform.GetComponent<Collider2D>().bounds.size.x;
            }

            activePlatforms.Add(initialPlatform);
        }
    }

    public void RestartLevel()
    {
        totalDistance = 0;
        foreach(GameObject platform in activePlatforms)
        {
            platform.SetActive(false);
        }
        activePlatforms.Clear();
        StartLevel();
        
    }

    public void SendTotalDistanceRun()
    {
        gameOver = true;
        FindObjectOfType<UIManager>().SetRunDistance((int)totalDistance);
    }
}
