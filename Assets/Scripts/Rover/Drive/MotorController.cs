using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : IPhysicsStep
{
    public MotorConfig MotorConfig { get; private set; }

    public MotorController(MotorConfig config)
    {
        if (config == null)
            Debug.LogError("The provided motor config is null!");
        MotorConfig = config;
    }

    public void PerformPhysicsStep()
    {
        // 
    }
}
