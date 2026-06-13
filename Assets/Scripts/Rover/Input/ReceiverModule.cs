using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverModule : IPhysicsStep
{
    ISignalSource signalSource;
    Vector2 signalBuffer;

    public ReceiverModule(ISignalSource signalSource)
    {
        this.signalSource = signalSource;
    }

    public void PerformPhysicsStep(float deltaTime)
    {
        /// If necessary, a lower receiver sampling rate can be simulated by skipping a number of frames
        SampleSignal();
    }

    void SampleSignal() => signalBuffer = signalSource.GetCurrentSignal();

    public Vector2 GetCurrentInput() => signalBuffer;
}
