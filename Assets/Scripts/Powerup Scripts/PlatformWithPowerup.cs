using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWithPowerup : MonoBehaviour
{
    [SerializeField] private PowerUpInfo powerUp;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Collider2D collider;

    public delegate void PickupPowerupDelegate(string powerupName, float duration);
    public event PickupPowerupDelegate puPickupEvent;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        renderer.sprite = powerUp.icon;      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("PICKED UP");
        if (collision.collider.CompareTag("Player"))
        {
            if(puPickupEvent != null)
            {
                puPickupEvent(powerUp.name, powerUp.duration);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT");
    }
}
