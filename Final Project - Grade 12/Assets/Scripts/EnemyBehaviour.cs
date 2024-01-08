using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float checkRadius;
    public float attackRadius;

    private float distance;

    private SpriteRenderer sprite;
    private Animator anim;
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;
    LayerMask whatIsPlayer;

    private bool isInChaseRange;
    private bool isInAttackRange;
    private enum MovementState { idle, running}
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //enemy moves towards player if in circle
        /*isInChaseRange = Physics2D.OverlapBox(transform.position, checkRadius, whatIsPlayer);*/
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        //animation stuff
        AnimationStateUpdater();
    }

    private void AnimationStateUpdater()
    {
        /*MovementState state;

        if (dirX > 0.1f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0.1f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("State", (int)state);*/
    }
}
