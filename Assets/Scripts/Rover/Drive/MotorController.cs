using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : IPhysicsStep
{
    public MotorSpecs MotorSpecs { get; private set; }

    public MotorController(MotorSpecs specs)
    {
        MotorSpecs = specs;
    }

    public void PerformPhysicsStep()
    {
        // 
    }
}
