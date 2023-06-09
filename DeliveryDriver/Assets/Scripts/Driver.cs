using System;
using UnityEngine;

public class Driver : MonoBehaviour
{
    
    [SerializeField]
    float rotationRate = 1f;
    
    [SerializeField]
    float translationRate = 0.1f;
    
    void Awake()
    {
        Time.fixedDeltaTime = 1/60f;
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationRate);
        transform.Translate(0, translationRate, 0, Space.Self);
    }
}
