using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float jumpSpeed;
    public DetectionZone attackZone;
    public float checkRadius;
    public float walkStopRate = 0.6f;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;

    private float distance;
    private float dirX = 0f;
    public float timeElapsed = 0;

    private SpriteRenderer sprite;
    private Animator anim;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection walkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool HasTarget = false;
    public float timeBeforeJump = 0.5f;
    public float maxTime;

    public bool hasTarget
    {
        get { return HasTarget; }
        private set
        {
            HasTarget = value;
            anim.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimationStrings.canMove);
        }
    }

    private enum MovementState { idle, running, attack, Death }
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
        timeElapsed += Time.deltaTime;
        if (touchingDirections.isGrounded)
        {
            
            if (distance < checkRadius)
            {
                if (CanMove)
                {

                    //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                    if (player.transform.position.x > transform.position.x)
                    {
                        walkDirection = WalkableDirection.Right;
                        if (timeElapsed >= timeBeforeJump)
                        {
                            rb.velocity = new Vector2(speed * walkDirectionVector.x, jumpSpeed);
                        }
                    }
                    else if (player.transform.position.x < transform.position.x)
                    {
                        walkDirection = WalkableDirection.Left;
                        if (timeElapsed >= timeBeforeJump)
                        {
                            rb.velocity = new Vector2(speed * walkDirectionVector.x, jumpSpeed);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
                }
            }
            else
            {
                if (touchingDirections.IsOnWall)
                {
                    FlipDirection();
                }
                if (CanMove)
                    if (timeElapsed >= timeBeforeJump)
                    {
                        rb.velocity = new Vector2(speed * walkDirectionVector.x, jumpSpeed);
                    }
                    else
                    {
                        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
                    }
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        if (timeElapsed >= maxTime)
        {
            timeElapsed = 0;
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
}
