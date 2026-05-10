using System;
using System.Collections.Generic;


[Serializable] public class MotorConfig
{
    public float maxRPM;
    public float maxTorque;

    public MotorConfig() { }

    public MotorConfig(float maxMotorRPM, float maxMotorTorque)
    {
        maxRPM = maxMotorRPM;
        maxTorque = maxMotorTorque;
    }
}
