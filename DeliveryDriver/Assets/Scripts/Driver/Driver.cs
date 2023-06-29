using System;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField]
    float rotationRate = 250f;

    [SerializeField]
    float translationRate = 20f;

    DriverInputs m_DriverInputs;
    DriverInputs.DrivingActions m_DrivingActions;

    bool isMoving => m_DrivingActions.Moving.IsPressed();
    bool isSteering => m_DrivingActions.Steering.IsPressed();

    float m_FixedUpdateStepTime;
    
    void Awake()
    {
        SetRefreshRate();
        RegisterInputsCallbacks();
        
        m_FixedUpdateStepTime = Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        DoMove();
        DoSteer();
    }

    void DoMove()
    {
        if (isMoving)
        {
            var movingValue = m_DrivingActions.Moving.ReadValue<float>();
            transform.Translate(0, movingValue * translationRate * m_FixedUpdateStepTime, 0);
        }
    }

    void DoSteer()
    {
        if (isSteering && isMoving)
        {
            var steeringValue = m_DrivingActions.Steering.ReadValue<float>();
            transform.Rotate(0, 0, steeringValue * rotationRate * m_FixedUpdateStepTime);
        }
    }

    static void SetRefreshRate()
    {
        Time.fixedDeltaTime = 1 / 60f;
    }

    void RegisterInputsCallbacks()
    {
        m_DriverInputs = new DriverInputs();
        m_DriverInputs.Enable();
        m_DrivingActions = m_DriverInputs.Driving;
    }
}
