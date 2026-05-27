using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class RoverConfig : ISerializationCallbackReceiver
{
    /// <summary>
    /// Holds a reference pointing to the Rover Controller's list of assembly configs.
    /// Will be ignored by the JsonUtility serializer (it ignores properties).
    /// </summary>
    public IReadOnlyCollection<DriveAssemblyConfig> DriveAssemblyConfigs { get; private set; }
    [SerializeField] List<NamedSerializedData> serializedAssemblyConfigs;



    public RoverConfig(IReadOnlyCollection<DriveAssemblyConfig> driveAssemblyConfigs)
    {
        DriveAssemblyConfigs = driveAssemblyConfigs;
    }

    #region Events Subscription

    public void SubscribeToMotorUpdates(Action motorConfigUpdateHandler)
    {
        foreach (DriveAssemblyConfig assemblyConfig in DriveAssemblyConfigs)
            assemblyConfig.MotorConfig.OnConfigUpdated += motorConfigUpdateHandler;
    }

    public void UnsubscribeFromMotorUpdates(Action motorConfigUpdateHandler)
    {
        foreach (DriveAssemblyConfig assemblyConfig in DriveAssemblyConfigs)
            assemblyConfig.MotorConfig.OnConfigUpdated -= motorConfigUpdateHandler;
    }

    #endregion


    #region Serialization

    /// <summary>
    /// Creates the auxiliary list of serialized assembly containers.
    /// </summary>
    public void OnBeforeSerialize()
    {
        if (DriveAssemblyConfigs.IsNullOrEmpty())
            return;

        serializedAssemblyConfigs = new List<NamedSerializedData>();
        foreach (DriveAssemblyConfig config in DriveAssemblyConfigs)
        {
            NamedSerializedData serializedConfig = new NamedSerializedData(config.AssemblyName, config);
            serializedAssemblyConfigs.Add(serializedConfig);
        }
    }

    /// <summary>
    /// Appies serialized data to corresponding assembly configs with matching names.
    /// Note: For successful loading of assembly configs, the config list should be provided prior to deserialization.
    /// </summary>
    public void OnAfterDeserialize()
    {
        if (serializedAssemblyConfigs.IsNullOrEmpty() || DriveAssemblyConfigs.IsNullOrEmpty())
            return;

        foreach (NamedSerializedData data in serializedAssemblyConfigs)
            foreach (DriveAssemblyConfig config in DriveAssemblyConfigs)
                if (config.AssemblyName == data.Name)
                    data.ApplyAsOverwriteTo(config);
    }

    #endregion
}

