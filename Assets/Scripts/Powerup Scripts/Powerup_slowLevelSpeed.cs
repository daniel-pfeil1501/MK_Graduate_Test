using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_slowLevelSpeed : MonoBehaviour
{

    private LevelManager levelManager;
    [SerializeField] private float duration;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
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


        Debug.Log("in coroutine");
        float originalSpeed = levelManager.GetCurentSpeed();
        levelManager.changeLevelMoveSpeed(originalSpeed * 0.8f);

        yield return new WaitForSeconds(duration);

        levelManager.changeLevelMoveSpeed(originalSpeed);
        gameObject.SetActive(false);
    }
}
