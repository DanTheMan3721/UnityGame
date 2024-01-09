using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 10f;
    [SerializeField]  private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float maxFallSpeed = 30;

    private enum MovementState { idle, running, jumping, falling, wallSlide }

    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isWallJumping)
        {
            // Player Movement
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);

            if (!isWallSliding)
            {
                if (Input.GetButtonDown("Jump") && jumpCheck())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }

            if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }

        AnimationStateUpdater();

        WallSlide();

        WallJump();
    }

    private void AnimationStateUpdater()
    {
        MovementState state = 0;

        if (!isWallSliding)
        {
            if (dirX > 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
            }
            else if (dirX < 0f)
            {
                state = MovementState.running;
                sprite.flipX = true;
            }
            else
            {
                state = MovementState.idle;
            }

            if (rb.velocity.y > 0.1f)
            {
                state = MovementState.jumping;
            }
            else if (rb.velocity.y < -0.1f)
            {
                state = MovementState.falling;
            }
        }

        if (isWallSliding)
        {
            state = MovementState.wallSlide;
        }

        anim.SetInteger("State", (int)state);
    }

    private bool jumpCheck()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool IsWalled()
    {
        return (Physics2D.OverlapCircle(wallCheckRight.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(wallCheckLeft.position, 0.2f, wallLayer));
    }

    private void WallSlide()
    {
        if (IsWalled() && dirX != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;

            if (dirX > 0f)
            {
                wallJumpingDirection = -1;
            }
            else
            {
                wallJumpingDirection = 1;
            }

            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                sprite.flipX = !sprite.flipX;
            }


            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
