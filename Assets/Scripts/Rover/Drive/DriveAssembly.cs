using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A group of wheels powered by a common motor.
/// </summary>
[Serializable] public class DriveAssembly : IPhysicsStep
{
    #region Config
    [Tooltip("Used as an ID during serialization, therefore should be unique in the scope of the particular Rover Controller")]
    [SerializeField] string assemblyName;
    public string AssemblyName { get { return assemblyName; } }
    [SerializeField] MotorSpecsAsset motorSpecsAsset;
    #endregion

    #region Modules
    [SerializeField] List<WheelCollider> wheels;
    MotorController motorController;
    BaseTransmission transmission;
    #endregion

    #region State
    MovementCommand movementCommandBuffer;
    #endregion



    public void Initialize(Transform roverCenterOfRotation)
    {
        if (motorSpecsAsset == null)
        {
            Debug.LogWarning($"Can't initialize the drive assembly: {assemblyName}. Motor Specs Assets isn't assigned!");
            return;
        }

        motorController = new MotorController(motorSpecsAsset.GenerateMotorSpecsInstance());
        transmission = new LockedTransmission(wheels, roverCenterOfRotation);
    }


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


    #region Name Validation

    public void EnsureNameNotEmpty()
    {
        if (string.IsNullOrWhiteSpace(AssemblyName))
            SetNameToDefault();
    }

    public void SetNameToDefault()
    {
        assemblyName = motorSpecsAsset ? $"{motorSpecsAsset.name} Assembly" : "New Drive Assembly";
    }

    public void ResolveNameCollision(int numOfMatchingNames)
    {
        //optionally, a name incrementation can be implemented for cases when there's (i) on the name's end already
        assemblyName = $"{assemblyName} ({numOfMatchingNames})";
    }

    #endregion


    #region Specs

    public DriveAssemblySpecs GenerateAssemblySpecs()
    {
        return new DriveAssemblySpecs(assemblyName, motorController.MotorSpecs);
    }

    #endregion
}
