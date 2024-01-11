using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public DetectionZone attackZone;
    public float checkRadius;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;

    private float distance;
    private float dirX = 0f;

    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask wallLayer;

    
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

    public bool HasTarget = false;
    public bool hasTarget { get { return HasTarget; } private set 
        {
            HasTarget = value;
            anim.SetBool(AnimationStrings.hasTarget, value);
        } }
    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimationStrings.canMove);
        }
    }

    private enum MovementState {idle, running, attack, Death}
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
        hasTarget = attackZone.detectedCollisions.Count > 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        if (distance < checkRadius)
        {
            if(!CanMove)
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
            {
                FlipDirection();
            }
            if (!CanMove)
                rb.velocity = new Vector2(speed * walkDirectionVector.x, rb.velocity.y);
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
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

        if (walkDirectionVector == Vector2.left)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (walkDirectionVector == Vector2.right)
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
