using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class RoverSpecs
{
    [Header("Drive Assemblies Specs")]
    List<DriveAssemblySpecs> driveAssemblies;

    public RoverSpecs(List<DriveAssemblySpecs> driveAssembliesSpecs)
    {
        driveAssemblies = driveAssembliesSpecs;
    }
}

