using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWithPowerup : MonoBehaviour
{
    [SerializeField] private PowerUpInfo[] powerUps;
    [SerializeField] private GameObject powerUpSpawn;
    [SerializeField] private Collider2D collider;

    private PowerUpInfo powerUpToUse;
    private SpriteRenderer renderer;

    private PowerupManager powerUpManager;

    private bool collected;

    private void Start()
    {
        powerUpManager = FindObjectOfType<PowerupManager>();
        renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        powerUpToUse = powerUps[Random.Range(0, powerUps.Length)];
    }

    private void OnEnable()
    {
        if (renderer == null)
        {
            renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        }

        powerUpToUse = powerUps[Random.Range(0, powerUps.Length)];
        renderer.enabled = true;
        collider.enabled = true;
        collected = false;

        renderer.sprite = powerUpToUse.icon;
    }

    private void Awake()
    {


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;
            powerUpManager.PowerUpCollected(powerUpToUse.type, powerUpToUse.duration);
            renderer.enabled = false;
        }
    }
}
