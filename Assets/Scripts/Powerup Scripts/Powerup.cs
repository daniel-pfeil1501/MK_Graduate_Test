using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    [SerializeField] private float duration;
    private float timer;
    private bool consumed;
    protected string powerupName;

    public delegate void PickupPowerupDelegate(string powerupName);
    public event PickupPowerupDelegate puPickupEvent;

    public delegate void EndPowerupDelegate(string powerupName);
    public event EndPowerupDelegate puEndEvent;

    private void Awake()
    {
        timer = 0f;
        consumed = false;

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void Update()
    {
        if (!consumed) { return; }
        Debug.Log(timer);
        if (timer > duration)
        {
          if(puEndEvent != null)
            {
                puEndEvent(powerupName);
            }
            gameObject.SetActive(false);
        }

            timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            consumed = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
 
            if(puPickupEvent != null)
            {
                puPickupEvent(powerupName);
            }
            
        }

    }

}
