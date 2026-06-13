using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


/// <summary>
/// A group of wheels powered by a common motor.
/// </summary>
[Serializable] public class DriveAssembly : IPhysicsStep
{
    #region Config
    DriveAssemblyConfig AssemblyConfig { get; set; }
    public string AssemblyName
    {
        get
        {
            if (AssemblyConfig == null)
                Debug.LogError($"Can't get the assembly name: assembly config is null.");
            return AssemblyConfig?.AssemblyName;
        }
    }
    #endregion

    #region Modules
    MotorController motorController;
    BaseTransmission transmission;
    #endregion

    #region State
    public float TargetVelocity { get; set; }
    /// <summary>
    /// Calculated based on the transmission's effective radius and motor's maximum RPM.
    /// </summary>
    public float MaxLinearVelocity => KinematicSolver.ConvertRPMToLinearVelocity(motorController.MaxRPM, transmission.EffectiveRadius);
    /// <summary>
    /// If the drive assemly offest is 0 it means it's located in the rover rotation pivot and doesn't limit the rover angular velocity.
    /// In such case the value of this property will be the positive infinity.
    /// </summary>
    public float MaxAngularVelocity
    {
        get
        {
            float maxVelocity = MaxLinearVelocity / Mathf.Abs(transmission.EffectiveXOffset);
            return float.IsNaN(maxVelocity) ? float.PositiveInfinity : maxVelocity;
        }
    }
    /// <summary>
    /// The abstract X offset of the assembly relative to the rover rotation pivot.
    /// </summary>
    public float EffectiveXOffset => transmission.EffectiveXOffset;
    #endregion



    #region Initialization

    public DriveAssembly(DriveAssemblyConfig assemblyConfig, Transform roverCenterOfRotation)
    {
        if (assemblyConfig == null)
            Debug.LogError($"The drive assembly \"{assemblyConfig.AssemblyName}\" isn't properly initalized: the Assembly Config is null!");
        else if (!CheckConfigValidity(assemblyConfig))
            assemblyConfig.Initialize();
        
        AssemblyConfig = assemblyConfig;
        InitializeModules(AssemblyConfig, roverCenterOfRotation);
    }

    void InitializeModules(DriveAssemblyConfig assemblyConfig, Transform roverCenterOfRotation)
    {
        transmission = new LockedTransmission(assemblyConfig?.Wheels, roverCenterOfRotation);
        motorController = new MotorController(assemblyConfig?.MotorConfig, transmission);
    }

    /// <summary>
    /// Check if config fields are present and ready for use.
    /// Log warning if a fields is missing.
    /// </summary>
    /// <param name="configToValidate">The config to check</param>
    /// <returns>'true' if config is valid, otherwise 'false'</returns>
    bool CheckConfigValidity(DriveAssemblyConfig configToValidate)
    {
        bool configIsValid = true;

        if (configToValidate.MotorConfig == null)
        {
            Debug.LogWarning($"The drive assembly \"{configToValidate.AssemblyName}\" may not be initialized properly: the motor config is null!");
            configIsValid = false;
        }

        if (configToValidate.Wheels == null)
        {
            Debug.LogWarning($"The drive assembly \"{configToValidate.AssemblyName}\" may not be initialized properly: the Wheels list is null!");
            configIsValid = false;
        }

        return configIsValid;
    }

    #endregion


    public void PerformPhysicsStep(float deltaTime)
    {
        motorController.TargetRPM = KinematicSolver.ConvertLinearVelocityToRPM(TargetVelocity, transmission.EffectiveRadius);
        motorController.PerformPhysicsStep(deltaTime);
    }
}
