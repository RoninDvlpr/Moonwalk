using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A group of wheels powered by a common motor.
/// </summary>
[Serializable] public class DriveAssembly : IPhysicsStep
{
    public string AssemblyName { get; private set; }
    DriveAssemblyConfig AssemblyConfig { get; set; }
    List<WheelCollider> Wheels
    {
        get
        {
            return AssemblyConfig.Wheels;
        }
    }

    #region Modules
    List<WheelCollider> wheels;
    MotorController motorController;
    BaseTransmission transmission;
    #endregion

    #region State
    MovementCommand movementCommandBuffer;
    #endregion



    #region Initialization

    public DriveAssembly(DriveAssemblyConfig assemblyConfig, Transform roverCenterOfRotation)
    {
        if (assemblyConfig == null)
            Debug.LogError($"The drive assembly \"{assemblyConfig.AssemblyName}\" isn't properly initalized: the Assembly Config is null!");
        else
            CheckConfigValidity(assemblyConfig);
        
        AssemblyConfig = assemblyConfig;
        InitializeModules(AssemblyConfig, roverCenterOfRotation);
    }

    void InitializeModules(DriveAssemblyConfig assemblyConfig, Transform roverCenterOfRotation)
    {
        motorController = new MotorController(assemblyConfig.MotorConfig);
        transmission = new LockedTransmission(wheels, roverCenterOfRotation);
    }

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


    public void UpdateMovemenCommand(MovementCommand roverMovementCommand)
    {
        movementCommandBuffer = roverMovementCommand;
    }

    public void PerformPhysicsStep()
    {
        // Calculate target assembly velocity
        // Calculate target motor RPM
        // Push target RPM to the motor
    }
}
