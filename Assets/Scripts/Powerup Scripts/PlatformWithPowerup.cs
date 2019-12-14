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

    private void Start()
    {
        powerUpManager = FindObjectOfType<PowerupManager>();
        renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        powerUpToUse = powerUps[Random.Range(0, powerUps.Length)];
    }
    private void Awake()
    {
        if(renderer == null)
        {
            renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        }

        renderer.enabled = true;
        collider.enabled = true;
        powerUpToUse = powerUps[Random.Range(0, powerUps.Length)];

        renderer.sprite = powerUpToUse.icon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            powerUpManager.PowerUpCollected(powerUpToUse.type, powerUpToUse.duration);
            renderer.enabled = false;
            collider.enabled = false;
        }
    }
}
