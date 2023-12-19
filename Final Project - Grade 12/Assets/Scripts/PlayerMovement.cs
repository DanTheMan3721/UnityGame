using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float maxFallSpeed = 30;

    private enum MovementState { idle, running, jumping, falling }

    private void Start()
    { 
        // Set RigidBody Variable
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Player Movement
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && jumpCheck())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);

        AnimationStateUpdater();
    }

    private void AnimationStateUpdater()
    {
        MovementState state;
        
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

        anim.SetInteger("State", (int)state);
    }

    private bool jumpCheck()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}
