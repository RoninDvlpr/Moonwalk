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
    PidConfig config;
    public float Kp
    {
        get
        {
            if (config == null)
                LogDefaultCoefficientWarning("Kp", default(float));
            return config?.Kp ?? default(float);
        }
    }
    public float Ki
    {
        get
        {
            if (config == null)
                LogDefaultCoefficientWarning("Ki", default(float));
            return config?.Ki ?? default(float);
        }
    }
    public float Kd
    {
        get
        {
            if (config == null)
                LogDefaultCoefficientWarning("Kd", default(float));
            return config?.Kd ?? default(float);
        }
    }

    float integral;
    float derivative;
    float previousError;
    bool isFirstIteration = true;


    public PidController(PidConfig config)
    {
        if (config == null)
            Debug.LogError("The the provided PID config is null!");
        this.config = config;
    }

    /// <summary>
    /// Calculates the corrective output for the current iteration of the control loop.
    /// </summary>
    /// <param name="target">The desired target value (setpoint).</param>
    /// <param name="current">The current measured value (process variable).</param>
    /// <param name="outputMin">The lower constraint for the output value.</param>
    /// <param name="outputMax">The upper constraint for the output value.</param>
    /// <param name="deltaTime">The time elapsed since the previous loop iteration.</param>
    /// <returns>The calculated effort required to reach the target, clamped between the minimum and maximum constraints.</returns>
    public float CalculateOutput(float target, float current, float outputMin, float outputMax, float deltaTime)
    {
        float error = target - current;
        float proportional = Kp * error;

        // integral and derivative terms are only updated if the time elapsed since the previous loop iteration is non-zero
        if (deltaTime > 0f)
        {
            integral += Ki * error * deltaTime;
            integral = Mathf.Clamp(integral, outputMin, outputMax);

            // calculating the derivative on the first iteration, while there's no previous error yet, will result in a derivative kick / spike
            if (!isFirstIteration)
                derivative = Kd * (error - previousError) / deltaTime;
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

    void LogDefaultCoefficientWarning(string coefficientName, float coefficientDefaultValue)
    {
        Debug.LogWarning($"The PID config is null, returning the default value of {coefficientDefaultValue} for the {coefficientName} coefficient.");
    }
}
