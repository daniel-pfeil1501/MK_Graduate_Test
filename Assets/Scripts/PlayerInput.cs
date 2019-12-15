using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private PlayerController controller;

    private float rayDistanceToGround;                  //distance of ray used to detect if the player is grounded.
    private Vector3 rayOrigin;                          //Start position of the raycast

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float jumpVelocity;        //The amount of velocity added to the rigidbody's y axis when the player jumps 

    private Vector2 inputVelocity;                      //The total volocity gathered from input.

    private bool canJump;                               //True when the raycast is hitting the ground.

    private Collider2D collider;


    private void Start()
    {
        controller = GetComponent<PlayerController>();

        collider = GetComponent<Collider2D>();
        rayDistanceToGround = collider.bounds.extents.y + 0.1f;

        inputVelocity = Vector2.zero;

    }

    private void Update()
    {
        rayOrigin = new Vector3(collider.bounds.min.x, collider.bounds.center.y, 0);

        Debug.DrawRay(rayOrigin, Vector3.down, Color.red);
        canJump = Physics2D.Raycast(rayOrigin, Vector3.down, rayDistanceToGround, layerMask);

        if (Input.touchCount > 0&& canJump)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                inputVelocity.y += jumpVelocity;
                animator.SetBool("isJumping", true);
                canJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        controller.AddInputToVelocity(inputVelocity);

        if(canJump && Mathf.Sign(-controller.GetCurrentVelocity().y) == 1)
        {
            animator.SetBool("isJumping", false);
        }



        inputVelocity = Vector2.zero;
    }
}
