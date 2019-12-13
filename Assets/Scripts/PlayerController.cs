using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;

    [SerializeField] private float fallMulti = 2.5f;
    [SerializeField] private float lowJumpMulti = 2f;

    [SerializeField] private float catchUpSpeed;

    private Vector2 inputVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        inputVelocity = Vector2.zero;
    }

    public void AddInputToVelocity(Vector2 input)
    {
        inputVelocity = input;
    }

    private void FixedUpdate()
    {
        rb.velocity += inputVelocity;

        Jump();
        Catchup();


    }

    public Vector2 GetCurrentVelocity()
    {
        return rb.velocity;
    }

    private void Jump() //Jump calculations
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMulti - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && Input.touchCount == 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;
        }
    }

    private void Catchup()
    {
        if(transform.position.x < 0)
        {
            transform.Translate(Vector2.right * catchUpSpeed * Time.deltaTime);
        }
    }




}
