using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A set of drive assembly parameters and dependenices.
/// This class is supposed to be exposed in the inspector of the Rover Controller.
/// <br/> WARNING: Do not serialize this class to external save files (e.g., JSON, PlayerPrefs), 
/// as live scene and asset references will break upon deserialization.
/// For cross-session config saving, use a dedicated pure-data subcontainer (e.g., DriveAssemblyActiveSpecs) instead.
/// </summary>
[Serializable] public class DriveAssemblyConfig
{
    [Tooltip("Used as an ID during serialization, therefore should be unique in the scope of the particular Rover Controller")]
    [SerializeField] string assemblyName;
    public string AssemblyName => assemblyName;

    [SerializeField] List<WheelCollider> wheels;
    public IReadOnlyCollection<WheelCollider> Wheels => wheels.AsReadOnly();

    [SerializeField] MotorConfigPreset motorDefaultConfig;
    /// <summary>
    /// Represents the effective motor config currently used by the drive assembly.
    /// NOTE: If there will be more than one active config (e.g., transmission, motor, shaft, etc.), they can be encapsulated
    /// in a container (for example, DriveAssemblyActiveSpecs) for easier serialization.
    /// </summary>
    MotorConfig activeMotorConfig;
    /// <summary>
    /// The public Motor Config accessor performs lazy initialization.
    /// </summary>
    public MotorConfig MotorConfig
    {
        get
        {
            if (!HasMotorConfig)
                Initialize();
            return activeMotorConfig;
        }
    }
    bool HasMotorConfig => activeMotorConfig != null;



    /// <summary>
    /// Initializes the assembly config using the provided motor config as overwrites on top of the current motor config
    /// (if the assembly hasn't been initialized yet, the current motor config will be generated from the default motor config template).
    /// </summary>
    /// <param name="motorConfigOverrides">A motor config used to overwrite the default motor config template</param>
    public void Initialize(MotorConfig motorConfigOverrides)
    {
        Initialize();
        if (motorConfigOverrides != null)
            activeMotorConfig.CopyObjectFields(motorConfigOverrides);
    }

    /// <summary>
    /// Since the class is created through a parameterless constructor by the Unity inspector
    /// before the default Motor Config has ever been assigned,
    /// the initialization should be performed before using the config by other modules.
    /// This ensures the creation of a usable Motor Config instance.
    /// </summary>
    public void Initialize()
    {
        if (!HasMotorConfig)
            SetMotorConfigFromDefaultPreset();

        if (!HasMotorConfig)
            activeMotorConfig = new MotorConfig();
    }

    /// <summary>
    /// Resets the current motor configuration instance to defaults while preserving its current event invocation list (subscriptions).
    /// If there's no motor config instance currently, a new instance is created
    /// based on the default configuration preset if the default preset is present.
    /// Used both for initialization from defaults and for reset to defaults.
    /// </summary>
    public void SetMotorConfigFromDefaultPreset()
    {
        if (motorDefaultConfig == null)
            Debug.LogError($"Drive assembly \"{AssemblyName}\" can't set a motor config from the default preset: the preset isn't assigned.");
        else
        {
            MotorConfig defaultConfig = motorDefaultConfig.GenerateMotorConfigInstance();
            if (activeMotorConfig?.CopyObjectFields(defaultConfig) == null)
                activeMotorConfig = defaultConfig;
        }        
    }



    #region Name Validation

    public void EnsureNameNotEmpty()
    {
        if (string.IsNullOrWhiteSpace(AssemblyName))
            SetNameToDefault();
    }

    public void SetNameToDefault()
    {
        assemblyName = motorDefaultConfig ? $"{motorDefaultConfig.name} Assembly" : "New Drive Assembly";
    }

    public void ResolveNameCollision(int numOfMatchingNames)
    {
        //optionally, a name incrementation can be implemented for cases when there's (i) on the name's end already
        assemblyName = $"{assemblyName} ({numOfMatchingNames})";
    }

    #endregion
}
