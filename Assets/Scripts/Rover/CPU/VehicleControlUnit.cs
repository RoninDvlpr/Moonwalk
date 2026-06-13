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
    RoverCapabilities roverCapabilities;
    ReceiverModule receiver;
    IReadOnlyCollection<DriveAssembly> driveAssemblies;
    #endregion

    #region State
    public bool DebugModeEnabled { get; set; }
    #endregion



    public VehicleControlUnit(ReceiverModule receiver, IReadOnlyCollection<DriveAssembly> driveAssemblies, RoverConfig roverConfig)
    {
        this.receiver = receiver;
        this.driveAssemblies = driveAssemblies;
        roverCapabilities = new RoverCapabilities(roverConfig, driveAssemblies);
    }

    public void PerformPhysicsStep(float deltaTime)
    {
        UpdateMotionCommand();
    }

    void UpdateMotionCommand()
    {
        Vector2 currentInput = receiver.GetCurrentInput();

        MotionCommand command = KinematicSolver.ComputeMotionCommand(currentInput, roverCapabilities.MaxLinearVelocity, roverCapabilities.MaxAngularVelocity);
        TransmitMotionCommand(command, driveAssemblies);

        if (DebugModeEnabled)
            Debug.Log(currentInput);
    }

    void TransmitMotionCommand(MotionCommand motionCommand, IReadOnlyCollection<DriveAssembly> driveAssemblies)
    {
        if (driveAssemblies.IsNullOrEmpty())
            Debug.LogWarning("Can't broadcast movement command: Drive assemblies aren't assigned!");
        else
            foreach (DriveAssembly assembly in driveAssemblies)
                PushAssemblyTargetVelocity(motionCommand, assembly);
    }

    void PushAssemblyTargetVelocity(MotionCommand motionCommand, DriveAssembly assembly)
    {
        if (assembly == null)
        {
            Debug.LogWarning("Can't update assembly target velocity: the given assembly is null.");
            return;
        }

        float assemblyTargetVelocity = KinematicSolver.ComputeAssemblyTargetVelocity(motionCommand, assembly);
        assembly.TargetVelocity = assemblyTargetVelocity;
    }

    public void Dispose()
    {
        roverCapabilities?.Dispose();
    }
}
