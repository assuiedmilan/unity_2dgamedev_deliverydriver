using System;
using Unity.DeliveryDriver.Editor.Enumerations;
using UnityEngine;

namespace Unity.DeliveryDriver.Runtime.Driver
{
    public class Driver : MonoBehaviour
    {
        [SerializeField]
        float rotationRate = 250f;

        [SerializeField]
        float translationRate = 20f;

        [SerializeField]
        float boostRateInPercent = 10f;
        
        DriverInputs m_DriverInputs;
        DriverInputs.DrivingActions m_DrivingActions;

        bool isMoving => m_DrivingActions.Moving.IsPressed();
        bool isSteering => m_DrivingActions.Steering.IsPressed();
        bool m_IsBoosted;
        bool m_IsBumped;
        
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

        void OnTriggerEnter2D(Collider2D other)
        {
            CheckForBoost(other);
        }
        
        void CheckForBoost(Collider2D other)
        {
            if (other.CompareTag(GameTags.Boost.ToString()))
            {
                m_IsBoosted = true;
                m_IsBumped = false;
                Destroy(other.gameObject);
            }
            
            if (other.CompareTag(GameTags.Bump.ToString()))
            {
                m_IsBoosted = false;
                m_IsBumped = true;
                Destroy(other.gameObject);
            }
        }
    
        void DoMove()
        {
            if (isMoving)
            {
                var movingValue = m_DrivingActions.Moving.ReadValue<float>();
                transform.Translate(0, movingValue * translationRate * m_FixedUpdateStepTime * GetBoostRate(), 0);
            }
        }

        void DoSteer()
        {
            if (isSteering)
            {
                var steeringValue = m_DrivingActions.Steering.ReadValue<float>();
                transform.Rotate(0, 0, steeringValue * rotationRate * m_FixedUpdateStepTime);
            }
        }

        float GetBoostRate()
        {
            if (m_IsBoosted)
            {
                return 1 + boostRateInPercent/100;
            }
            
            if (m_IsBumped)
            {
                return 1 - boostRateInPercent/100;
            }
            
            return 1;
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
}