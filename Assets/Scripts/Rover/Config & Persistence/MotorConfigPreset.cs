using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Motor Config")]
public class MotorConfigPreset : ScriptableObject
{
    /// <summary>
    /// The motor config instance that belongs to the asset itself.
    /// It's private to protect the SO from accidental modifications when accessing it form a code during runtime.
    /// </summary>
    [SerializeField] MotorConfig motorConfig = new MotorConfig();

    public MotorConfig GenerateMotorConfigInstance()
    {
        return new MotorConfig(motorConfig.maxRPM, motorConfig.maxTorque);
    }
}
