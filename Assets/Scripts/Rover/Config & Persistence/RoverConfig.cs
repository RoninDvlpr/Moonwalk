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
    [SerializeField] List<NamedSerializedData> serializedMotorConfigs;



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
    /// Creates the auxiliary list of serialized motor containers.
    /// </summary>
    public void OnBeforeSerialize()
    {
        if (DriveAssemblyConfigs.IsNullOrEmpty())
            return;

        serializedMotorConfigs = new List<NamedSerializedData>();
        foreach (DriveAssemblyConfig config in DriveAssemblyConfigs)
        {
            NamedSerializedData serializedConfig = new NamedSerializedData(config.AssemblyName, config.MotorConfig);
            serializedMotorConfigs.Add(serializedConfig);
        }
    }

    /// <summary>
    /// Appies serialized data to corresponding motor configs with matching drive assembly names.
    /// Note: For successful loading of motor configs, the assembly configs list should be provided prior to deserialization.
    /// </summary>
    public void OnAfterDeserialize()
    {
        if (serializedMotorConfigs.IsNullOrEmpty() || DriveAssemblyConfigs.IsNullOrEmpty())
            return;

        foreach (NamedSerializedData data in serializedMotorConfigs)
            foreach (DriveAssemblyConfig config in DriveAssemblyConfigs)
                if (config.AssemblyName == data.Name)
                    data.ApplyAsOverwriteTo(config.MotorConfig);
    }

    #endregion
}

