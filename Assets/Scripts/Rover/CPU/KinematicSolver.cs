using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kinematic math library, used in the implementation of movement algorithms.
/// </summary>
public static class KinematicSolver
{
    #region Input Resolution

    public static MotionCommand ComputeMotionCommand(Vector2 input, float maxLinearVelocity, float maxAngularVelocity)
    {
        ClampInputVector(ref input);
        return new MotionCommand(input.y * maxLinearVelocity, input.x * maxAngularVelocity);
    }

    /// <summary>
    /// Clamps the input values between -1 and 1.
    /// </summary>
    /// <param name="input">Input vector to clamp</param>
    public static void ClampInputVector(ref Vector2 input)
    {
        input.x = Mathf.Clamp(input.x, -1f, 1f);
        input.y = Mathf.Clamp(input.y, -1f, 1f);
    }

    #endregion


    #region Velocity Calculation

    public static float ConvertRPMToLinearVelocity(float rpm, float wheelRadius)
    {
        return rpm / 60f * CalculateCircumference(wheelRadius);
    }

    public static float ConvertLinearVelocityToRPM(float linearVelocity, float wheelRadius)
    {
        return linearVelocity * 60f / CalculateCircumference(wheelRadius);
    }

    public static float CalculateCircumference(float radius) =>  2f * Mathf.PI * radius;

    public static float ComputeAssemblyTargetVelocity(MotionCommand command, DriveAssembly assembly)
    {
        return command.TargetLinearVelocity - command.TargetAngularVelocity * assembly.EffectiveXOffset;
    }

    #endregion
}
