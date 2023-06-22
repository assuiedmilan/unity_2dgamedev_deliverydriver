using System;
using UnityEngine;

public class Driver : MonoBehaviour
{
    
    [SerializeField]
    float rotationRate = 1f;
    
    [SerializeField]
    float translationRate = 0.1f;
        
    DriverInputs m_DriverInputs;
    DriverInputs.DrivingActions m_DrivingActions;
    
    bool isMoving => m_DrivingActions.Moving.IsPressed();
    bool isSteering => m_DrivingActions.Steering.IsPressed();
    
    void Awake()
    {
        SetRefreshRate();
        RegisterInputsCallbacks();
    }

    void FixedUpdate()
    {
        DoMove();
        DoSteer();
    }

    void DoMove()
    {
        if(isMoving)
        {
            var movingValue = m_DrivingActions.Moving.ReadValue<float>();
            transform.Translate(0, movingValue * translationRate, 0);
        }
    }

    void DoSteer()
    {
       if(isSteering && isMoving)
       {
           var steeringValue = m_DrivingActions.Steering.ReadValue<float>();
           transform.Rotate(0, 0, steeringValue * rotationRate);
       }
        
    }

    static void SetRefreshRate()
    {
        Time.fixedDeltaTime = 1/60f;
    }
    
    void RegisterInputsCallbacks()
    {
        m_DriverInputs = new DriverInputs();
        m_DriverInputs.Enable();
        m_DrivingActions = m_DriverInputs.Driving;
    }
}
