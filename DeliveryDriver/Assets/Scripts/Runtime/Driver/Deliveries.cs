using System;
using UnityEngine;

namespace Unity.DeliveryDriver.Runtime.Driver
{
    public class Delivery : MonoBehaviour
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
}