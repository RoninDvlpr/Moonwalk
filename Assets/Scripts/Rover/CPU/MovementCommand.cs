using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand
{
    public float TargetVelocity { get; private set; }
    public float TargetAngularSpeed { get; private set; }

    public MovementCommand(float targetVelocity, float targetAngularSpeed)
    {
        TargetVelocity = targetVelocity;
        TargetAngularSpeed = targetAngularSpeed;
    }
}
