using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements movement algorithms.
/// Can be used in a strategy pattern implementation.
/// </summary>
public class KinematicPlanner
{
    public MovementCommand ComputeMovementCommand(Vector2 input, float maxLinearSpeed, float maxAngularSpeed)
    {
        ClampInputVector(ref input);
        return new MovementCommand(input.y * maxLinearSpeed, input.x * maxAngularSpeed);
    }

    /// <summary>
    /// Clamps the input values between -1 and 1.
    /// </summary>
    /// <param name="input">Input vector to clamp</param>
    protected void ClampInputVector(ref Vector2 input)
    {
        input.x = Mathf.Clamp(input.x, -1f, 1f);
        input.y = Mathf.Clamp(input.y, -1f, 1f);
    }
}
