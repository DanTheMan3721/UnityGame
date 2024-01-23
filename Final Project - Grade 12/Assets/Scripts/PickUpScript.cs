using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{
    public int pickUpCount = 0;
    public TextMeshProUGUI count;
    public int totalPickUps = 0;
    public GameObject portal;
    private void OnTriggerEnter2D(Collider2D pickUp)
    {
        if (pickUp.tag == "PickUp")
        {
            pickUpCount++;
        }

    }
    private void Update()
    {
        count.text = "Gems Collected: " + pickUpCount.ToString() + " / " + totalPickUps.ToString();
        if (pickUpCount == totalPickUps)
        {
            portal.SetActive(true);
        }
    }
}
