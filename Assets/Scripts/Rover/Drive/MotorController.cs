using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : IPhysicsStep
{
    public MotorConfig MotorConfig { get; private set; }
    public float MaxRPM => MotorConfig?.MaxRPM ?? default (float);
    float targetRPM;
    public float TargetRPM
    {
        get => targetRPM;
        set => targetRPM = Mathf.Clamp(value, 0f, MaxRPM);
    }


    public MotorController(MotorConfig config)
    {
        if (config == null)
            Debug.LogError("The provided motor config is null!");
        MotorConfig = config;
    }

    public void PerformPhysicsStep()
    {
        // Run PID loop to adjust torque
        // Distribute torque to the transmission ? Probably, the drive assembly should do the destribution
    }
}
