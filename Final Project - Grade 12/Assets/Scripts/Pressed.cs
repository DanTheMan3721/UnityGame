using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressed : MonoBehaviour
{
    private Animator anim;
    private bool buttonPressed = false;

    [SerializeField] private GameObject platform;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();   
    }

    void Update()
    {
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            buttonPressed = true;
            for (int i = 0; i < 1; i++)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            anim.SetTrigger("Pressed");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            buttonPressed = false;
            anim.ResetTrigger("Pressed");
    }
}
