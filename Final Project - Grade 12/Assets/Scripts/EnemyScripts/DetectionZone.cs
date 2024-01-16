using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCollisions = new List<Collider2D>();
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        detectedCollisions.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollisions.Remove(collision);
    }
}
