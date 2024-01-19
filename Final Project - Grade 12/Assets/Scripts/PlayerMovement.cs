using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private DeathLogic dl;
    private SlowFall sf;

    // Wall Sliding
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;

    // Wall Jumping
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.5f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private LayerMask jumpableGround;

    // Regular Movement + Jumping
    private float dirX = 0f;
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float maxFallSpeed = 30;
    [SerializeField] private float sandMoveSpeed = 3f;
    [SerializeField] private float sandJumpForce = 4f;

    private float originalMoveSpeed;
    private float originalJumpForce;

    // Slow Falling
    [SerializeField] private float slowFallSpeed = 2f;

    // Animations
    private enum MovementState { idle, running, jumping, falling, wallSlide }

    // Attacks
    [SerializeField] private float swordTime = 0.6f;
    private bool attack = false;

    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        dl = GetComponent<DeathLogic>();
        sf = GetComponent<SlowFall>();

        originalMoveSpeed = movementSpeed;
        originalJumpForce = jumpForce;
    }

    private void Update()
    {
        if (!dl.dead)
        {
            if (!attack)
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

                        if (Input.GetButtonDown("Fire1"))
                        {
                            attack = true;
                            anim.SetTrigger(AnimationStrings.attack);
                            rb.velocity = new Vector2(0, rb.velocity.y);
                            StartCoroutine(attackActionCooldown());
                        }
                    }
                }

                WallSlide();

                WallJump();
            }

            AnimationStateUpdater();

            if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);

            if (sf.inSand)
            {
                movementSpeed = sandMoveSpeed;
                jumpForce = sandJumpForce;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slowFallSpeed, float.MaxValue));
            }
            else
            {
                movementSpeed = originalMoveSpeed;
                jumpForce = originalJumpForce;
            }
        }
    }

    private IEnumerator attackActionCooldown()
    {
        yield return new WaitForSeconds(swordTime);
        attack = false;
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
            rb.velocity = new Vector2(rb.velocity.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
