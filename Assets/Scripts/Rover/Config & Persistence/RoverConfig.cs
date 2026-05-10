using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class RoverConfig
{
    [SerializeField] List<DriveAssemblyConfig> driveAssemblyConfigs;
    public List<DriveAssemblyConfig> DriveAssemblyConfigs
    {
        get
        {
            return driveAssemblyConfigs;
        }
    }


    public RoverConfig(List<DriveAssemblyConfig> driveAssemblyConfigs)
    {
        this.driveAssemblyConfigs = driveAssemblyConfigs;
    }
}

