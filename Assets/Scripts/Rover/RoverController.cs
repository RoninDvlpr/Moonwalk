using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An orchestrator-pattern MonoBehaviour that encapsulates and controlls all rover subsystems.
/// </summary>
public class RoverController : MonoBehaviour
{
    #region Modules
    [SerializeField] List<DriveAssembly> driveAssemblies;
    VehicleControlUnit VCU;
    ReceiverModule receiverModule;
    #endregion

    #region State
    RoverSpecs roverSpecs;
    [SerializeField] bool enableDebugMode;
    #endregion



    #region Initialization

    void Start()
    {
        GenerateRoverSpecs();
        InitializeModules(roverSpecs);
    }

    void GenerateRoverSpecs()
    {
        List<DriveAssemblySpecs> assemblySpecs = new List<DriveAssemblySpecs>();
        foreach (DriveAssembly assembly in driveAssemblies)
            assemblySpecs.Add(assembly.GenerateAssemblySpecs());

        roverSpecs = new RoverSpecs(assemblySpecs);
    }

    /// <summary>
    /// Implements and encapsulates the Rover's custom DI.
    /// </summary>
    void InitializeModules(RoverSpecs specs)
    {
        ISignalSource signalSource = new InputSystemSignalSource();
        receiverModule = new ReceiverModule(signalSource);

        foreach (DriveAssembly assembly in driveAssemblies)
            assembly.Initialize(transform);

        VCU = new VehicleControlUnit(receiverModule, driveAssemblies, specs);
    }

    //void InitializeMotors()
    //{
    //    motors = new List<MotorController>();
    //    foreach (WheelGroup group in wheelGroups)
    //        foreach (WheelCollider collider in group.wheels)
    //            motors.Add(new MotorController(collider, group.motorSpecsAsset.GenerateMotorSpecsInstance()));
    //}

    #endregion


    /// <summary>
    /// Notifies the Rover modules sequentially about the next physics time step.
    /// The manual control of the modules' execution order ensures synchronization, preventing a 1-frame lag between systems.
    /// Such an approach allows for flexible module behavior, enabling more accurate simulation of independent systems.
    /// </summary>
    void FixedUpdate()
    {
        VCU.DebugModeEnabled = enableDebugMode;

        receiverModule.PerformPhysicsStep();
        VCU.PerformPhysicsStep();
        foreach (DriveAssembly assembly in driveAssemblies)
            assembly.PerformPhysicsStep();
    }


    #region Validation

    private void OnValidate()
    {
        ValidateDriveAssemblyNames();
    }

    /// <summary>
    /// Check each assembly name to ensure all names are unique and non-empty.
    /// The assemblies that were added last (stored at a higher index) are validated first.
    /// </summary>
    void ValidateDriveAssemblyNames()
    {
        if (driveAssemblies == null || driveAssemblies.Count == 0)
            return;

        for (int i = driveAssemblies.Count - 1; i >= 0; i--)
            ValidateAssemblyName(i);
    }

    void ValidateAssemblyName(int validatedAssemblyIndex)
    {
        driveAssemblies[validatedAssemblyIndex].EnsureNameNotEmpty();
        EnsureNameUniqueness(validatedAssemblyIndex);
    }

    int CountMatchingAssemblyNames(int validatedAssemblyIndex)
    {
        int matchingAssemblyNamesFound = 0;
        for (int j = 0; j < driveAssemblies.Count; j++)
            if (j != validatedAssemblyIndex && driveAssemblies[j].AssemblyName == driveAssemblies[validatedAssemblyIndex].AssemblyName)
                matchingAssemblyNamesFound++;
        return matchingAssemblyNamesFound;
    }

    void EnsureNameUniqueness(int validatedAssemblyIndex)
    {
        int cyclesPerformed = 0;
        int cyclesSafetyCap = 1000;

        int matchingAssemblyNamesFound = int.MaxValue;
        while (matchingAssemblyNamesFound > 0)
        {
            if (cyclesPerformed > cyclesSafetyCap)
            {
                Debug.LogError($"Drive assembly names validation loop seems to be stuck. Breaking the loop after performing {cyclesSafetyCap} iterations.");
                break;
            }

            matchingAssemblyNamesFound = CountMatchingAssemblyNames(validatedAssemblyIndex); //perform additional check to ensure the uniqueness of the new name
            if (matchingAssemblyNamesFound > 0)
                driveAssemblies[validatedAssemblyIndex].ResolveNameCollision(matchingAssemblyNamesFound);
            cyclesPerformed++;
        }
    }

    #endregion
}
