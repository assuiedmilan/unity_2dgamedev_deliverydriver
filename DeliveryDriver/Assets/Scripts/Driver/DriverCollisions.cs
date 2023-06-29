using System;
using UnityEngine;

public class DriverCollisions : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"Entered collision with {other.collider}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Entered trigger collision with {other.GetComponent<Collider2D>()}");
    }
}
