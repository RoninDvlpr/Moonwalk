using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a rover's CPU
/// </summary>
public class VehicleControlUnit : IPhysicsStep
{
    #region Modules
    KinematicPlanner kinematicPlanner = new KinematicPlanner();
    ReceiverModule receiver;
    List<DriveAssembly> driveAssemblies;
    #endregion

    #region State
    RoverSpecs roverSpecs;
    public bool DebugModeEnabled { get; set; }
    #endregion



    public VehicleControlUnit(ReceiverModule receiver, List<DriveAssembly> driveAssemblies, RoverSpecs roverSpecs)
    {
        this.receiver = receiver;
        this.driveAssemblies = driveAssemblies;
        this.roverSpecs = roverSpecs;
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
}
