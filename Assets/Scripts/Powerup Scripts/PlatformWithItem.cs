using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWithItem : MonoBehaviour
{
    [SerializeField] private ItemInfo[] items;
    [SerializeField] private GameObject powerUpSpawn;
    [SerializeField] private Collider2D collider;
    [SerializeField] private AudioSource powerUpEffect;
    [SerializeField] private AudioSource bombEffect;

    private ItemInfo itemToUse;
    private SpriteRenderer renderer;

    private ItemManager itemManager;

    private bool collected;

    private void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        //itemToUse = items[Random.Range(0, items.Length)];
    }

    private void OnEnable()
    {
        if (renderer == null)
        {
            renderer = powerUpSpawn.GetComponent<SpriteRenderer>();
        }

        itemToUse = items[Random.Range(0, items.Length)];
        renderer.enabled = true;
        collider.enabled = true;
        collected = false;

        renderer.sprite = itemToUse.icon;
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
            itemManager.ItemCollected(itemToUse.type, itemToUse.duration);
        }
    }
}
