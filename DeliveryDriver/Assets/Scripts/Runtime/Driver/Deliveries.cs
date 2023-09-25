using System;
using Unity.DeliveryDriver.Editor.Enumerations;
using UnityEngine;

namespace Unity.DeliveryDriver.Runtime.Driver
{
    public class Delivery : MonoBehaviour
    {
        bool m_HasPackage;
        
        void OnTriggerEnter2D(Collider2D other)
        {
            CheckForCollisionWithPackage(other);
            CheckForCollisionWithCustomer(other);
        }
        
        void CheckForCollisionWithPackage(Component other)
        {
            if (other.CompareTag(GameTags.Package.ToString()) && !m_HasPackage)
            {
                m_HasPackage = true;
                Debug.Log("Packaged picked up");
                Destroy(other.gameObject);
            }
        }
        
        void CheckForCollisionWithCustomer(Component other)
        {
            if (other.CompareTag(GameTags.Customer.ToString()) && m_HasPackage)
            {
                m_HasPackage = false;
                Debug.Log("Packaged delivered");
                Destroy(other.gameObject);
            }
        }
    }
}