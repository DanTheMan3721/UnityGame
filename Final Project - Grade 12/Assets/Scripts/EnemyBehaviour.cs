using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float checkRadius;

    Rigidbody2D rb;

    private float distance;
    private float dirX = 0f;

    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask wallLayer;

    private bool IsWalled()
    {
        return (Physics2D.OverlapCircle(wallCheckRight.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(wallCheckLeft.position, 0.2f, wallLayer));
    }
    private SpriteRenderer sprite;
    private Animator anim;

    public enum WalkableDirection {Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection walkDirection
    {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value; }
    }

    private enum MovementState {idle, running, attack, Death}
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        if (distance < checkRadius)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (IsWalled())
            {
                FlipDirection();
            }
            rb.velocity = new Vector2(speed * walkDirectionVector.x, rb.velocity.y);
        }
        
    }

    private void FlipDirection()
    {
        if (walkDirection == WalkableDirection.Right)
        {
            walkDirection = WalkableDirection.Left;
        }
        else if (walkDirection == WalkableDirection.Left)
        {
            walkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Walkable direction not set to legal value");
        }
    }

    private void AnimationStateUpdater()
    {
        MovementState state;

        if (walkDirection == WalkableDirection.Right)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (walkDirection == WalkableDirection.Left)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("State", (int)state);
    }
}
