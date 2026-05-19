using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand
{
    public float TargetLinearVelocity { get; private set; }
    public float TargetAngularVelocity { get; private set; }

    public MovementCommand(float targetLinearVelocity, float targetAngularVelocity)
    {
        TargetLinearVelocity = targetLinearVelocity;
        TargetAngularVelocity = targetAngularVelocity;
    }
}
