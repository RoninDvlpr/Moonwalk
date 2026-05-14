using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class RoverConfig : ISerializationCallbackReceiver
{
    [SerializeField] List<DriveAssemblyConfig> driveAssemblyConfigs;
    public IReadOnlyCollection<DriveAssemblyConfig> DriveAssemblyConfigs => driveAssemblyConfigs.AsReadOnly();


    public RoverConfig(List<DriveAssemblyConfig> driveAssemblyConfigs)
    {
        this.driveAssemblyConfigs = driveAssemblyConfigs;
    }

    #region Events Subscription

    public void SubscribeToMotorUpdate(Action motorConfigUpdateHandler)
    {
        foreach (DriveAssemblyConfig assemblyConfig in driveAssemblyConfigs)
            assemblyConfig.MotorConfig.OnConfigUpdated += motorConfigUpdateHandler;
    }

    public void UnsubscribeFromMotorUpdate(Action motorConfigUpdateHandler)
    {
        foreach (DriveAssemblyConfig assemblyConfig in driveAssemblyConfigs)
            assemblyConfig.MotorConfig.OnConfigUpdated -= motorConfigUpdateHandler;
    }

    #endregion

    #region Serialization

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {

    }

    #endregion
}

