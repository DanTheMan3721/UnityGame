using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public GameOverScreen GameOverScreen;
    public bool dead;

    [SerializeField] private float deadWait = 0.5f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death") && gameObject.tag == "Player")
        {
            Die();    
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Death") && gameObject.tag == "Player")
        {
            Die();
            
        }
    }

    private void Die()
    {
        dead = true;
        anim.SetTrigger("death");
        Invoke("SetupAccess", deadWait);
    }

    private void SetupAccess()
    {
        GameOverScreen.Setup();
    }
}
