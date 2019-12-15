using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameStateMananger gameStateMananger;
    [SerializeField] ItemManager powerUpManager;

    private Rigidbody2D rb;
    private Collider2D collider;


    [SerializeField] private float fallMulti = 2.5f;        //Increases the decent speed of the player.
    [SerializeField] private float lowJumpMulti = 2f;       //Increases the speed at the start of a jump.

    [SerializeField] private float catchUpSpeed;            //The speed at which the player will catch up if they fall behind.
    [SerializeField] private float climbSpeed;              //The speed at which the player will climb walls.

    private float defaultGravity;

    PlayerStats playerStats;

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

        inputVelocity = Vector2.zero;

        rayLength = collider.bounds.size.x + 0.02f;
        raySpacing = (collider.bounds.size.y - rayInsetAmount * 2) / (numberOfRays - 1);

        defaultGravity = rb.gravityScale;
        playerStats = new PlayerStats(climbSpeed, catchUpSpeed);
    }

    private void OnEnable()
    {
        gameStateMananger.restartEvent += ResetPlayer;
        gameStateMananger.startEvent += ResetPlayer;
        powerUpManager.puPickupEvent += ApplyPowerUp;
        powerUpManager.expiredEvent += RemovePowerUp;
    }

    private void OnDisable()
    {
        gameStateMananger.restartEvent -= ResetPlayer;
        gameStateMananger.startEvent -= ResetPlayer;
        powerUpManager.puPickupEvent -= ApplyPowerUp;
        powerUpManager.expiredEvent -= RemovePowerUp;
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
        rb.velocity += inputVelocity * Vector2.up;

        Jump();
        Catchup();
        if (DetectCollision())
        {
            transform.position += new Vector3(0, climbSpeed * Time.deltaTime, 0);
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }


    }

    public Vector2 GetCurrentVelocity()
    {
        return rb.velocity;
    }

    //Jump calculations
    private void Jump()
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

    //When the player falls behind the centre, they will slowly catch back up.
    private void Catchup()
    {
        if(transform.position.x < -1)
        {
            transform.Translate(Vector2.right * catchUpSpeed * Time.deltaTime);
        }
    }

    //Used to determine if the player should be climbing a wall.
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

    //Adds power-up effects to the player.
    public void ApplyPowerUp(ItemManager.itemType type)
    {
        if(type == ItemManager.itemType.climb)
        {
            climbSpeed = playerStats.buffedClimbSpeed;
        }

        if(type == ItemManager.itemType.catchUp)
        {
            catchUpSpeed = playerStats.buffedCatchUpSpeed;
        }
    }

    //Removes power-up effects from the player.
    public void RemovePowerUp(ItemManager.itemType type)
    {
        if(type == ItemManager.itemType.climb)
        {
            climbSpeed = playerStats.defaultClimbSpeed;
        }

        if (type == ItemManager.itemType.catchUp)
        {
            catchUpSpeed = playerStats.defaultClimbSpeed;
        }
    }

    //Returns the player when the game is reset.
    public void ResetPlayer()
    {
        catchUpSpeed = playerStats.defaultCatchUpSpeed;
        climbSpeed = playerStats.defaultClimbSpeed;

        rb.velocity = Vector2.zero;

        gameObject.transform.position = new Vector3(-8, 0, 0);
    }

    public void PlayDeathParticle()
    {
        GetComponent<ParticleSystem>().Play();
    }

    //Stores various stats used by the character.
    private struct PlayerStats
    {
        public float defaultClimbSpeed { get; }
        public float buffedClimbSpeed { get; }
        public float buffedCatchUpSpeed { get; }
        public float defaultCatchUpSpeed { get; }

        public PlayerStats(float climbSpeed, float catchUpSpeed)
        {
            defaultClimbSpeed = climbSpeed;
            buffedClimbSpeed = climbSpeed * 2;

            defaultCatchUpSpeed = catchUpSpeed;
            buffedCatchUpSpeed = catchUpSpeed * 2;
        }    
    }
}
