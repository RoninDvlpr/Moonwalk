using System;
using System.Collections.Generic;


[Serializable] public class MotorSpecs
{
    public float maxRPM;
    public float maxTorque;

    public MotorSpecs() { }

    public MotorSpecs(float maxMotorRPM, float maxMotorTorque)
    {
        maxRPM = maxMotorRPM;
        maxTorque = maxMotorTorque;
    }
}
