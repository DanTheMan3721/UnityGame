using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFall : MonoBehaviour
{
    public bool inSand = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inSand = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inSand = false;
    }
}
