using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a named object serialized as a JSON string. Used for name-based deserialization.
/// </summary>
[Serializable] public class NamedSerializedData
{
    string name;
    public string Name => name;
    string jsonData;
    public string JsonData => jsonData;


    public NamedSerializedData(string objectName, object objectToSerialize)
    {
        name = objectName;
        jsonData = JsonUtility.ToJson(objectToSerialize);
    }

    /// <summary>
    /// Overwrites the target instance with the stored serialized JSON string.
    /// The existing object field values that aren't present in the serialized data are preserved.
    /// </summary>
    /// <param name="objectToApplyTo">Object that will be overwritten with the stored JSON data</param>
    public void ApplyAsOverwriteTo(object objectToApplyTo)
    {
        if (objectToApplyTo == null)
        {
            Debug.LogWarning("Deserialization failed: Can't apply JSON data to a null object!");
            return;
        }
        else if (string.IsNullOrEmpty(JsonData))
        {
            Debug.LogWarning("Deserialization failed: JSON data string is null or empty.");
            return;
        }

        JsonUtility.FromJsonOverwrite(JsonData, objectToApplyTo);
    }
}
