using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{
    public GameObject pickup;
    public int pickUpCount = 0;
    public TextMeshProUGUI count;
    private void OnTriggerEnter2D(Collider2D pickUp)
    {
        Debug.Log("Works?");
        if (pickUp.tag == "pickUp")
        {
            Debug.Log("Works");
            pickUpCount++;
        }

    }
    private void Update()
    {
        count.text = "Gems Collected: " + pickUpCount.ToString();
    }
}
