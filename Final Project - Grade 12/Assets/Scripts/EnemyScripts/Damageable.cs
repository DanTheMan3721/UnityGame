using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    [SerializeField]
    private int maxHealth = 3;
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
    [SerializeField]
    private int health = 3;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health<=0) { 
            IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool isAlive = true;
    private bool isInvincible = false;
    private float timeSinceHit = 0f;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            anim.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("isAlive set " + value);
        }
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(isInvincible)
        {
            if(timeSinceHit > invincibilityTime) 
            { 
                isInvincible=false;
                timeSinceHit = 0;
            }
            
            timeSinceHit += Time.deltaTime;
        }
        Hit(1);
    }
    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }
}
