using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Driver : MonoBehaviour
{
    
    [SerializeField]
    float rotationRate = 1f;
    
    [SerializeField]
    float translationRate = 0.1f;
    
    void Awake()
    {
        SetRefreshRate();
        RegisterInputsCallbacks();
    }

    void FixedUpdate()
    {
        transform.Translate(0, translationRate, 0, Space.Self);
    }
    
    void DoSteer(InputAction.CallbackContext obj)
    {
        transform.Rotate(0, 0, rotationRate);
        Debug.Log("Rotating !");
    }

    static void SetRefreshRate()
    {
        Time.fixedDeltaTime = 1/60f;
    }
    
    void RegisterInputsCallbacks()
    {
        var inputActions = new DriverInputs();
        inputActions.Enable();
        
        inputActions.Driving.Steering.performed += DoSteer;
    }


}
