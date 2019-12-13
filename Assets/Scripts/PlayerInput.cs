using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerInput : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] Animator animator;

    private float rayDistanceToGround;
    [SerializeField] LayerMask layerMask;
    [SerializeField] private float fallMulti = 2.5f;
    [SerializeField] private float lowJumpMulti = 2f;

    private bool jump;
    private bool canJump;
    private bool isJumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rayDistanceToGround = GetComponent<Collider2D>().bounds.extents.y + 0.2f;

        isJumping = false;

    }

    private void Update()
    {

        canJump = Physics2D.Raycast(transform.position, Vector3.down, rayDistanceToGround, layerMask);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        if (canJump && Mathf.Sign(rb.velocity.y) < 0)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (Input.touchCount > 0 && canJump)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                jump = true;
                isJumping = true;
                animator.SetBool("isJumping", true);
            }
        }


        Debug.Log(canJump);



    }

    private void FixedUpdate()
    {

        if (jump)
        {
            jump = false;
            rb.velocity += new Vector2(0, 10);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMulti - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && Input.touchCount == 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;
        }

    }

}
