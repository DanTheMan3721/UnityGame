using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFall : MonoBehaviour
{
    private Rigidbody rb;
    
    private bool slowFalling;
    [SerializeField] private float slowFallSpeed;
    [SerializeField] private LayerMask quickSandLayer;
    [SerializeField] private Transform quickSandCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SlowedFall();
    }
    private bool CanFall()
    {
        return (Physics2D.OverlapCircle(quickSandCheck.position, 0.2f, quickSandLayer));
    }

    private void SlowedFall()
    {
        if (CanFall())
        {
            slowFalling = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slowFallSpeed, float.MaxValue));
        }
        else
        {
            slowFalling = false;
        }
    }
}
