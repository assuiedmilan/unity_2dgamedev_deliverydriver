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
    
    void Awake()
    {
        SetRefreshRate();
        RegisterInputsCallbacks();
    }

    void FixedUpdate()
    {
        DoSteer();
    }
    
    
    void DoSteer()
    {
        float movingValue = 0;
        var isMoving = false;
        
        if(m_DrivingActions.Moving.IsPressed())
        {
            movingValue = m_DrivingActions.Moving.ReadValue<float>();
            transform.Translate(0, movingValue * translationRate, 0);
            
            isMoving = movingValue != 0;
        }
        
        if(m_DrivingActions.Steering.IsPressed() && isMoving)
        {
            var steeringValue = m_DrivingActions.Steering.ReadValue<float>();
            transform.Rotate(0, 0, steeringValue * rotationRate * 1/movingValue);
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
