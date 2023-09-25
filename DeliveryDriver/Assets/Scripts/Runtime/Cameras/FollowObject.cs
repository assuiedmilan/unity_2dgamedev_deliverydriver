using System;
using UnityEngine;

namespace Unity.DeliveryDriver.Runtime.Cameras
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField]
        GameObject objectToFollow;
        
        Vector3 m_ZPosition;

        void Awake()
        {
            m_ZPosition = new Vector3(0, 0, transform.position.z);
        }

        void LateUpdate()
        {
            if(objectToFollow != null)
            {
                transform.position = objectToFollow.transform.position + m_ZPosition;
            }
        }
    }
}