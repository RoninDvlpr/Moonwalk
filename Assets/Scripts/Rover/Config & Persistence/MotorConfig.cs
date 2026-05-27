using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class MotorConfig : ObservableConfig
{
    [SerializeField] float maxRPM;
    public float MaxRPM
    {
        get => maxRPM;
        set
        {
            maxRPM = value;
            NotifyAboutChanges();
        }
    }
    [SerializeField] float maxTorque;
    public float MaxTorque
    {
        get => maxTorque;
        set
        {
            maxTorque = value;
            NotifyAboutChanges();
        }
    }


    /// <summary>
    /// Initializes the config using default float values (0f).
    /// With a maximum RPM and torque of 0, the motor won't produce power, signaling that the motor hasn't been initialized properly.
    /// </summary>
    public MotorConfig()
    {
        Debug.LogWarning($"Initializing motor config with default values: max RPM of {MaxRPM} and max torque of {MaxTorque}. " +
            "If appropriate values aren’t set, the motor may not behave as intended.");
    }

    public MotorConfig(float maxMotorRPM, float maxMotorTorque)
    {
        maxRPM = maxMotorRPM;
        maxTorque = maxMotorTorque;
    }
}
