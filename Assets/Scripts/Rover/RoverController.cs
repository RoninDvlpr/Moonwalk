using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An orchestrator-pattern MonoBehaviour that encapsulates and controlls all rover subsystems.
/// </summary>
public class RoverController : MonoBehaviour
{
    #region Config
    RoverConfig roverConfig;
    [Tooltip("A point the rover rotates around during turns")]
    [SerializeField] Transform roverCenterOfRotation;
    [SerializeField] List<DriveAssemblyConfig> driveAssemblyConfigs;
    [SerializeField] bool enableDebugMode;
    #endregion

    #region Modules
    ReceiverModule receiverModule;
    VehicleControlUnit VCU;
    [SerializeField] List<DriveAssembly> driveAssemblies;
    #endregion



    #region Initialization

    void Start()
    {
        if (roverCenterOfRotation == null)
            roverCenterOfRotation = transform;
        GenerateRoverConfig();
        InitializeModules(roverConfig, roverCenterOfRotation);
    }

    void GenerateRoverConfig()
    {
        roverConfig = new RoverConfig(driveAssemblyConfigs.AsReadOnly());
        // Loading of serialized config overrides should happen here
    }

    /// <summary>
    /// Implements and encapsulates the Rover's custom DI.
    /// </summary>
    void InitializeModules(RoverConfig config, Transform roverCenterOfRotation)
    {
        ISignalSource signalSource = new InputSystemSignalSource();
        receiverModule = new ReceiverModule(signalSource);

        foreach (DriveAssemblyConfig assemblyConfig in config.DriveAssemblyConfigs)
            driveAssemblies.Add(new DriveAssembly(assemblyConfig, roverCenterOfRotation));

        VCU = new VehicleControlUnit(receiverModule, driveAssemblies, config);
    }

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
        ValidateDriveAssemblyConfigNames();
    }

    /// <summary>
    /// Check each assembly name to ensure all names are unique and non-empty.
    /// The assemblies that were added last (stored at a higher index) are validated first.
    /// </summary>
    void ValidateDriveAssemblyConfigNames()
    {
        if (driveAssemblyConfigs == null || driveAssemblyConfigs.Count == 0)
            return;

        for (int i = driveAssemblyConfigs.Count - 1; i >= 0; i--)
            ValidateAssemblyName(i);
    }

    void ValidateAssemblyName(int validatedAssemblyIndex)
    {
        driveAssemblyConfigs[validatedAssemblyIndex].EnsureNameNotEmpty();
        EnsureNameUniqueness(validatedAssemblyIndex);
    }

    int CountMatchingAssemblyNames(int validatedAssemblyIndex)
    {
        int matchingAssemblyNamesFound = 0;
        for (int j = 0; j < driveAssemblyConfigs.Count; j++)
            if (j != validatedAssemblyIndex && driveAssemblyConfigs[j].AssemblyName == driveAssemblyConfigs[validatedAssemblyIndex].AssemblyName)
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
                Debug.LogError($"Drive assembly config names validation loop seems to be stuck. Breaking the loop after performing {cyclesSafetyCap} iterations.");
                break;
            }

            matchingAssemblyNamesFound = CountMatchingAssemblyNames(validatedAssemblyIndex); //perform additional check to ensure the uniqueness of the new name
            if (matchingAssemblyNamesFound > 0)
                driveAssemblyConfigs[validatedAssemblyIndex].ResolveNameCollision(matchingAssemblyNamesFound);
            cyclesPerformed++;
        }
    }

    #endregion
}
