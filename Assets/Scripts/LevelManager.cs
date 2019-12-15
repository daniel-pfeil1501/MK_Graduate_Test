using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float totalDistance;

    private ObjectPooler objectPooler;                          //Mananges a pool of platforms for the game to request and use.

    [SerializeField] GameStateMananger gameStateManager;        //This is subscribed to events to do with the current gamestate.

    private List<GameObject> activePlatforms;                   //A list of all the currently active platforms in the level.
    private GameObject initialPlatform;                         //The platform that will be used at the start of the level.
    private Vector3 platformSpawnLocation;                      //The location that all new platforms will spawn at.

    private float deltaX;                                       //Used to determine when a new platform is needed.
    private float platformLength;                               //Used to correctly space the platforms.

    bool gameOver = false;


    [SerializeField] private float horizontalMoveSpeed;         //Sets the speed at which the level will move.

    [SerializeField] private int maxActivePlatforms;            //The maximum allowed active platforms.
    [SerializeField] private int numberOfInitialPlatforms;      //number of platforms to spawn at the start of the game.

    private string[] platformLookup;                            //Used to find the names of each platform to request them from the pool.

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

    //Move all platforms active in the level.
    private void FixedUpdate()
    {
        if (!gameOver)
        {
            for (int i = 0; i < activePlatforms.Count; i++)
            {
                activePlatforms[i].transform.Translate(new Vector3(-horizontalMoveSpeed * Time.deltaTime, 0, 0));
            }
        }

    }

    //Track the distance moved for platform spawning and score.
    private void TrackDistance()
    {
        if (!gameOver)
        {
            deltaX += horizontalMoveSpeed * Time.deltaTime;
            totalDistance += horizontalMoveSpeed * Time.deltaTime;
        }
    }

    //Requests a new platform from the pool and adds it to the level.
    private void RequestNewPlatform()
    {
        float excess = deltaX - platformLength;
        deltaX = 0;

        GameObject newPlatform = objectPooler.GetObjectFromPool(platformLookup[Random.Range(0, platformLookup.Length)]);
        newPlatform.transform.position = activePlatforms[activePlatforms.Count - 1].transform.position + (Vector3.right * (platformLength + excess));
        newPlatform.SetActive(true);
        activePlatforms.Add(newPlatform);
    }

    //Sets the starting state of the level.
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

    //Clears the current level and resets to the start state.
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

    public int GetCurrentDistance()
    {
        return (int)totalDistance;
    }
}
