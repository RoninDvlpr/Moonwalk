using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DriveAssemblyTelemetryContainer
{
    public string assemblyName;
    public float targetVelocity, targetRPM, currentRPM, currentTorque;

    public DriveAssemblyTelemetryContainer(string assemblyName, float targteVelocity, float targetRPM, float currentRPM, float currentTorque)
    {
        this.assemblyName = assemblyName;
        this.targetVelocity = targteVelocity;
        this.targetRPM = targetRPM;
        this.currentRPM = currentRPM;
        this.currentTorque = currentTorque;
    }
}
