using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a rover's CPU
/// </summary>
public class VehicleControlUnit : IPhysicsStep, IDisposable
{
    #region Modules
    KinematicPlanner kinematicPlanner = new KinematicPlanner();
    KinematicLimits kinematicLimits;
    ReceiverModule receiver;
    List<DriveAssembly> driveAssemblies;
    #endregion

    #region State
    RoverConfig roverConfig;
    public bool DebugModeEnabled { get; set; }
    #endregion



    public VehicleControlUnit(ReceiverModule receiver, List<DriveAssembly> driveAssemblies, RoverConfig roverConfig)
    {
        this.receiver = receiver;
        this.driveAssemblies = driveAssemblies;
        this.roverConfig = roverConfig;
        kinematicLimits = new KinematicLimits(roverConfig, mkmdfgmfdmgfd);
    }

    public void PerformPhysicsStep()
    {
        UpdateMovementCommand();
    }

    void UpdateMovementCommand()
    {
        Vector2 currentInput = receiver.GetCurrentInput();
        MovementCommand movementCommand = kinematicPlanner.ComputeMovementCommand(currentInput);
        BroadcastMovementCommand(movementCommand);

        if (DebugModeEnabled)
            Debug.Log(currentInput);
    }

    void BroadcastMovementCommand(MovementCommand movementCommand)
    {
        if (driveAssemblies == null || driveAssemblies.Count == 0)
            Debug.LogWarning("Drive assemblies isn't assigned!");
        else
            foreach (DriveAssembly assembly in driveAssemblies)
                assembly?.UpdateMovemenCommand(movementCommand);
    }

    public void Dispose()
    {
        kinematicLimits?.Dispose();
    }
}
