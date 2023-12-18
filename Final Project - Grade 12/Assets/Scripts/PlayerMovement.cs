using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        // Set RigidBody Variable
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Player Movement
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 100f);
        }
    }
}
