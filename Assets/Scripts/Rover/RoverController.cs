using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An orchestrator-pattern MonoBehaviour that encapsulates and controlls all rover subsystems.
/// </summary>
public class RoverController : MonoBehaviour
{
    ReceiverModule receiverModule;
    DriveController driveController;

    void Start()
    {
        InitializeModules();
    }

    /// <summary>
    /// Implements and encapsulates the Rover's custom DI.
    /// </summary>
    void InitializeModules()
    {
        driveController = new DriveController();

        ISignalSource signalSource = new InputSystemSignalSource();
        receiverModule = new ReceiverModule(signalSource, driveController);
    }

    /// <summary>
    /// Notifies the Rover modules sequentially about the next physics time step.
    /// The manual control of the modules' execution order ensures synchronization, preventing a 1-frame lag between systems.
    /// Such an approach allows for flexible module behavior, enabling more accurate simulation of independent systems.
    /// </summary>
    void FixedUpdate()
    {
        receiverModule.OnFixedUpdate();
        driveController.OnFixedUpdate();
    }
}
