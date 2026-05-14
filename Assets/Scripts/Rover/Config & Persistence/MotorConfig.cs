using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class MotorConfig : ObservableConfig
{
    [SerializeField] float maxRPM;
    public float MaxRPM
    {
        get => maxRPM;
        set
        {
            maxRPM = value;
            NotifyAboutChanges();
        }
    }
    [SerializeField] float maxTorque;
    public float MaxTorque
    {
        get => maxTorque;
        set
        {
            maxTorque = value;
            NotifyAboutChanges();
        }
    }



    public MotorConfig() { }

    public MotorConfig(float maxMotorRPM, float maxMotorTorque)
    {
        maxRPM = maxMotorRPM;
        maxTorque = maxMotorTorque;
    }
}
