using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoverTelemetryContainer
{
    public List<DriveAssemblyTelemetryContainer> assemblyTelemetries;

    public RoverTelemetryContainer(List<DriveAssemblyTelemetryContainer> assemblyTelemetries)
    {
        this.assemblyTelemetries = assemblyTelemetries;
    }
}
