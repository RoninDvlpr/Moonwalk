using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The container that represents a rover movement command at a given moment of time.
/// </summary>
public class MotionCommand
{
    /// <summary>
    /// Represents the target forward/reverse movement velocity in m/s.
    /// </summary>
    public float TargetLinearVelocity { get; private set; }
    /// <summary>
    /// Represents the turning speed in rad/s. A positive value stands for clockwise rotation.
    /// </summary>
    public float TargetAngularVelocity { get; private set; }

    public MotionCommand(float targetLinearVelocity, float targetAngularVelocity)
    {
        TargetLinearVelocity = targetLinearVelocity;
        TargetAngularVelocity = targetAngularVelocity;
    }
}
