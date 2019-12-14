using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameStateMananger gameStateMananger;

    private Rigidbody2D rb;
    private Collider2D collider;


    [SerializeField] private float fallMulti = 2.5f;
    [SerializeField] private float lowJumpMulti = 2f;

    [SerializeField] private float catchUpSpeed;
    [SerializeField] private float climbSpeed;
    private float gravityScale;

    [SerializeField] LayerMask layerMask;
    [SerializeField, Range(2,6)] int numberOfRays;
    [SerializeField] private float rayInsetAmount;          //Used to stop the player gollding with the ground when running on a flat surface.
    private float rayLength;                                //Ray length to detect the player colliding with the side of an obstacle;
    private Vector2 rayOrigin;
    private float raySpacing;

    private Vector2 inputVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        gameStateMananger.restartEvent += ResetPlayer;

        inputVelocity = Vector2.zero;

        rayLength = collider.bounds.size.x + 0.02f;
        raySpacing = (collider.bounds.size.y - rayInsetAmount * 2) / (numberOfRays - 1);

        gravityScale = rb.gravityScale;

        
    }

    public void AddInputToVelocity(Vector2 input)
    {
        inputVelocity = input;
    }

    private void Update()
    {
        rayOrigin = new Vector2(collider.bounds.max.x, collider.bounds.min.y + rayInsetAmount);
    }

    private void FixedUpdate()
    {
        rb.velocity += inputVelocity;

        Jump();
        Catchup();
        if (DetectCollision())
        {
            transform.position += new Vector3(0, climbSpeed * Time.deltaTime, 0);
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }


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
        if(transform.position.x < -1)
        {
            transform.Translate(Vector2.right * catchUpSpeed * Time.deltaTime);
        }
    }

    private bool DetectCollision()
    {
        bool hit = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            Debug.DrawRay(rayOrigin + Vector2.up * raySpacing * i, Vector2.right, Color.red);
            if (Physics2D.Raycast(rayOrigin + Vector2.up * i * raySpacing, Vector2.right, rayLength, layerMask)) { hit = true; }
        }

        return hit;
    }

    public void ChangeClimbSpeed(float amount)
    {
        climbSpeed *= amount;
    }

    public void ChangeCatchUpSpeed(float amount)
    {
        catchUpSpeed *= amount;
    }

    public void ResetPlayer()
    {
        gameObject.transform.position = new Vector3(-8, 0, 0);
    }
}
