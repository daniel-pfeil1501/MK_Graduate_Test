using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_climbspeed : MonoBehaviour
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
        Debug.Log(player.tag);
        if (player.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PickUp());
        }
        else if (player.CompareTag("outOfBounds"))
        {
            gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.tag);
    }

    private IEnumerator PickUp()
    {
        controller.ChangeClimbSpeed(2f);

        yield return new WaitForSeconds(duration);

        controller.ChangeClimbSpeed(0.5f);
        gameObject.SetActive(false);
    }


}
