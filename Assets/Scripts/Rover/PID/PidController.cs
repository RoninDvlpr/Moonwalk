using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a generic PID adjustment mechanism.
/// Since the controller retains its internal state across loop iterations, calling the Reset method is required
/// when restarting the loop or reusing this instance to control a different value.
/// </summary>
public class PidController
{
    readonly PidConfig config;

    float integral;
    float derivative;
    float previousError;
    bool isFirstIteration = true;


    public PidController(PidConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// Calculates the corrective output for the current iteration of the control loop.
    /// </summary>
    /// <param name="target">The desired target value (setpoint).</param>
    /// <param name="current">The current measured value (process variable).</param>
    /// <param name="outputMin">The lower constraint for the output value.</param>
    /// <param name="outputMax">The upper constraint for the output value.</param>
    /// <param name="fixedDeltaTime">The time elapsed since the previous loop iteration.</param>
    /// <returns>The calculated effort required to reach the target, clamped between the minimum and maximum constraints.</returns>
    public float CalculateOutput(float target, float current, float outputMin, float outputMax, float fixedDeltaTime)
    {
        float error = target - current;
        float proportional = config.Kp * error;

        // integral and derivative terms are only updated if the time elapsed since the previous loop iteration is non-zero
        if (fixedDeltaTime > 0f)
        {
            integral += config.Ki * error * fixedDeltaTime;
            integral = Mathf.Clamp(integral, outputMin, outputMax);

            // calculating the derivative on the first iteration, while there's no previous error yet, will result in a derivative kick / spike
            if (!isFirstIteration)
                derivative = config.Kd * (error - previousError) / fixedDeltaTime;
            else
                isFirstIteration = false;
        }

        previousError = error;

        float totalOutput = proportional + integral + derivative;
        return Mathf.Clamp(totalOutput, outputMin, outputMax);
    }

    /// <summary>
    /// Resets the internal state of the control loop, clearing all accumulated error and history.
    /// </summary>
    public void Reset()
    {
        integral = derivative = previousError = 0f;
        isFirstIteration = true;
    }
}
