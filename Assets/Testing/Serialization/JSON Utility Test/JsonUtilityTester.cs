using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtilityTester : TestSerializer
{
    public JsonUtilityTestContainer sourceObject, overwrittenObject1, overwrittenObject2;

    void Start()
    {
        //SaveObject(sourceObject, "savedTestString");
        LoadObject(overwrittenObject1, "savedTestString");
        LoadObject(overwrittenObject2, "savedTestString");
    }
}
