using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DriveAssemblyTelemetryContainer
{
    public string assemblyName;
    public float targetRPM, currentRPM, currentTorque;

    public DriveAssemblyTelemetryContainer(string assemblyName, float targetRPM, float currentRPM, float currentTorque)
    {
        this.assemblyName = assemblyName;
        this.targetRPM = targetRPM;
        this.currentRPM = currentRPM;
        this.currentTorque = currentTorque;
    }
}
