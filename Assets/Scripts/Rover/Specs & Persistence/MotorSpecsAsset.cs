using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Specs/Motor Specs")]
public class MotorSpecsAsset : ScriptableObject
{
    /// <summary>
    /// The motor specs instance that belongs to the asset itself.
    /// It's private to protect the SO from accidental modifications when accessing it in the code during runtime.
    /// </summary>
    [SerializeField] MotorSpecs motorSpecs = new MotorSpecs();

    public MotorSpecs GenerateMotorSpecsInstance()
    {
        return new MotorSpecs(motorSpecs.maxRPM, motorSpecs.maxTorque);
    }
}
