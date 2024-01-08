using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject pickup;
    public GameObject player;
    public int pickUpCount = 0;
    void OnTriggerEnter2D(Collider2D player)
    {
        if(player.gameObject.tag == "Player")
        {
            pickup.active = false;
            pickUpCount++;
        }

    }
}
