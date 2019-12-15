using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWithItem : MonoBehaviour
{
    [SerializeField] private ItemInfo[] items;                              //A list containing scriptable objects of items available.
    [SerializeField] private GameObject powerUpSpawn;                       //The location the power up will spawn at.
    [SerializeField] private Collider2D collider;
    [SerializeField] private AudioSource powerUpEffect;
    [SerializeField] private AudioSource bombEffect;
    [SerializeField, Range(0,100)] private int percentSpawnChance = 60;     //The chance that this platform will spawn an item when enabled

    private ItemInfo itemToUse;
    private SpriteRenderer renderer;

    private ItemManager itemManager;

    private bool collected;

    private void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
    
    }

    private void OnEnable()
    {

        if (renderer == null)
        {
            renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        }

        collider.enabled = false;
        renderer.enabled = false;

        if (Random.Range(0,100) <= percentSpawnChance - 1 )
        {
            itemToUse = items[Random.Range(0, items.Length)];
            renderer.enabled = true;
            collider.enabled = true;
            collected = false;
            renderer.sprite = itemToUse.icon;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collected)
        {

            if(itemToUse.type != ItemManager.itemType.bomb)
            {
                powerUpEffect.Play();
            }
            else
            {
                bombEffect.Play();
            }

            collected = true;
            renderer.enabled = false;
            collider.enabled = false;
            itemManager.ItemCollected(itemToUse.type, itemToUse.duration);
        }
    }
}
