using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float distance;
    [SerializeField] private float aliveTime;
    [SerializeField] private float gravScale = 5;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        if (isFalling == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

            Debug.DrawRay(transform.position, Vector2.down * distance, Color.red);

            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player")
                {
                    rb.gravityScale = gravScale;
                    isFalling = true;
                    Invoke("Destroy", aliveTime);
                }
            }
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
