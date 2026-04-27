using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverModule : IPhysicsStep
{
    ISignalSource signalSource;
    DriveController roverDriveController;

    public ReceiverModule(ISignalSource signalSource, DriveController roverDriveController)
    {
        this.signalSource = signalSource;
        this.roverDriveController = roverDriveController;
    }

    public void OnFixedUpdate()
    {
        /// If necessary, a lower receiver sampling rate can be simulated by skipping a number of frames
        PushInputToDriveController();
    }

    /// <summary>
    /// Samples the signal and pushes the received command to the Drive Controller.
    /// </summary>
    void PushInputToDriveController()
    {
        Vector2 signalSample = signalSource.GetCurrentSignal();
        roverDriveController.SetInputCommand(signalSample);
    }
}
