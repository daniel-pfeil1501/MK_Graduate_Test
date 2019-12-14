using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private PlayerController controller;

    private float rayDistanceToGround;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float jumpVelocity;

    private Vector2 inputVelocity;

    private bool jump;
    private bool canJump;


    private void Start()
    {
        controller = GetComponent<PlayerController>();

        rayDistanceToGround = GetComponent<Collider2D>().bounds.extents.y + 0.2f;

        inputVelocity = Vector2.zero;

    }

    private void Update()
    {


        canJump = Physics2D.Raycast(transform.position, Vector3.down, rayDistanceToGround, layerMask);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        if (canJump && Mathf.Sign(controller.GetCurrentVelocity().y) <= 0)
        {
            animator.SetBool("isJumping", false);
        }

        if (Input.touchCount > 0 && canJump)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                inputVelocity.y += jumpVelocity;
                animator.SetBool("isJumping", true);
            }
        }
    }


    private void FixedUpdate()
    {
        controller.AddInputToVelocity(inputVelocity);
        inputVelocity = Vector2.zero;
    }

}
