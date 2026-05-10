using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A set of drive assembly parameters and dependenices.
/// This class is supposed to be exposed in the inspector of the Rover Controller.
/// </summary>
[Serializable] public class DriveAssemblyConfig
{
    [Tooltip("Used as an ID during serialization, therefore should be unique in the scope of the particular Rover Controller")]
    [SerializeField] string assemblyName;
    public string AssemblyName
    {
        get
        {
            return assemblyName;
        }
    }
    [SerializeField] List<WheelCollider> wheels;
    public List<WheelCollider> Wheels
    {
        get
        {
            return wheels;
        }
    }
    [SerializeField] MotorConfigPreset motorDefaultConfig;
    MotorConfig currentMotorConfig;
    /// <summary>
    /// The public Motor Config accessor performs lazy initialization.
    /// </summary>
    public MotorConfig MotorConfig
    {
        get
        {
            if (!HasMotorConfig)
                Initialize();
            return currentMotorConfig;
        }
    }
    bool HasMotorConfig
    {
        get
        {
            return currentMotorConfig != null;
        }
    }



    /// <summary>
    /// Applies a user motor config overrides on top of the default motor config template.
    /// Used to load a serialized user motor settings.
    /// </summary>
    /// <param name="motorConfigOverrides">A motor config set by user that supposed to override the default motor config template</param>
    public void Initialize(MotorConfig motorConfigOverrides)
    {
        if (motorConfigOverrides != null)
            currentMotorConfig = motorConfigOverrides;
        else
            Initialize();
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
    }

    /// <summary>
    /// Resets the current motor configuration, creating a new instance based on the default configuration preset.
    /// Used both for initialization from defaults and for reset to defaults.
    /// </summary>
    public void SetMotorConfigFromDefaultPreset()
    {
        if (motorDefaultConfig == null)
            Debug.LogError($"Drive assembly \"{AssemblyName}\" can't set a motor config from the default preset: the preset isn't assigned.");
        else
            currentMotorConfig = motorDefaultConfig.GenerateMotorConfigInstance();
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
