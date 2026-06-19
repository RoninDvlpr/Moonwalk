using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigSerializationTester : TestSerializer
{
    public List<DriveAssemblyConfig> serializedAssemblies;

    void Start()
    {
        RoverConfig roverConfig = new RoverConfig(serializedAssemblies.AsReadOnly());
        //SaveObject(roverConfig, "savedTestConfig");
        LoadObject(roverConfig, "savedTestConfig");
    }
}
