using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : IPhysicsStep
{
    #region Dependencies
    IMotorLoad motorLoad;
    public MotorConfig MotorConfig { get; private set; }
    PidController pidController;
    #endregion

    #region Config
    public float MaxRPM => MotorConfig?.MaxRPM ?? default (float);
    public float MaxTorque => MotorConfig?.MaxTorque ?? default (float);
    #endregion

    #region State
    float targetRPM;
    public float TargetRPM
    {
        get => targetRPM;
        set => targetRPM = Mathf.Clamp(value, 0f, MaxRPM);
    }
    float currentTorque;
    public float CurrentTorque
    {
        get => currentTorque;
        private set => currentTorque = Mathf.Clamp(value, -MaxTorque, MaxTorque);
    }
    #endregion


    public MotorController(MotorConfig config, IMotorLoad load)
    {
        if (config == null)
            Debug.LogError("The provided motor config is null!");
        MotorConfig = config;
        pidController = new PidController(MotorConfig?.PidConfig);

        if (load == null)
            Debug.LogError("The provided motor load is null!");
        motorLoad = load;
    }

    public void PerformPhysicsStep(float deltaTime)
    {
        if (motorLoad == null)
        {
            Debug.LogWarning("Motor isn't functioning: the provided motor load object is null!");
            return;
        }

        // Run PID loop to adjust torque
        float currentRPM = motorLoad.GetCurrentRpm();
        CurrentTorque = pidController.CalculateOutput(targetRPM, currentRPM, -MaxTorque, MaxTorque, deltaTime);

        // Apply torque to the transmission
        motorLoad.ApplyTorque(CurrentTorque);
    }
}
