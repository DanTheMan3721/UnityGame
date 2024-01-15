using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D pickUp)
    {
        if (pickUp.tag == "Player")
        {
            Destroy(gameObject);
        }

    }
}
