using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public GameOverScreen GameOverScreen;
    public bool dead;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Die();    
        }
    }

    private void Die()
    {
        dead = true;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        GameOverScreen.Setup();
    }
}
