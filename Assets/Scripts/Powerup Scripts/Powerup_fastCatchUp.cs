using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_fastCatchUp : MonoBehaviour
{
    private PlayerController controller;
    [SerializeField] private float duration;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PickUp());
        }

    }

    private IEnumerator PickUp()
    {
        controller.ChangeCatchUpSpeed(2f);

        yield return new WaitForSeconds(duration);

        controller.ChangeCatchUpSpeed(0.5f);
        gameObject.SetActive(false);
    }
}
