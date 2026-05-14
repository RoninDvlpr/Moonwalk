using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class RoverConfig
{
    [SerializeField] List<DriveAssemblyConfig> driveAssemblyConfigs;
    public IReadOnlyCollection<DriveAssemblyConfig> DriveAssemblyConfigs => driveAssemblyConfigs.AsReadOnly();


    public RoverConfig(List<DriveAssemblyConfig> driveAssemblyConfigs)
    {
        this.driveAssemblyConfigs = driveAssemblyConfigs;
    }

    #region Events Subscription

    #region Max RPM

    public void SubscribeToMotorMaxRPMChangedEvents(Action maxRPMChangedHandler)
    {
        foreach (DriveAssemblyConfig assemblyConfig in driveAssemblyConfigs)
            assemblyConfig.MotorConfig.MaxRPMChanged += maxRPMChangedHandler;
    }

    public void UnsubscribeToMotorMaxRPMChangedEvents(Action maxRPMChangedHandler)
    {
        foreach (DriveAssemblyConfig assemblyConfig in driveAssemblyConfigs)
            assemblyConfig.MotorConfig.MaxRPMChanged -= maxRPMChangedHandler;
    }

    #endregion

    #endregion
}

