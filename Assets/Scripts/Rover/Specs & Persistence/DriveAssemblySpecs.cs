using System;
using System.Collections.Generic;


[Serializable] public class DriveAssemblySpecs
{
    public string assemblyName;
    public MotorSpecs motorSpecs;

    public DriveAssemblySpecs(string assemblyName, MotorSpecs motorSpecs)
    {
        this.assemblyName = assemblyName;
        this.motorSpecs = motorSpecs;
    }
}
