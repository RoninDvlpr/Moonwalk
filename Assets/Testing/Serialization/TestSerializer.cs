using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A helper class that performs serialization/deserialization of provided objects
/// into a JSON string and writing/reading of the serialized string into the player prefs.
/// </summary>
public class TestSerializer : MonoBehaviour
{
    public void SaveObject(object objectToSave, string propertyName)
    {
        string jsonString = JsonUtility.ToJson(objectToSave);
        PlayerPrefs.SetString(propertyName, jsonString);
        Debug.Log($"Saved object of type {objectToSave.GetType().Name} into the {propertyName} Player Prefs property");
    }

    public void LoadObject(object objectToLoad, string propertyName)
    {
        string jsonString = PlayerPrefs.GetString(propertyName);
        JsonUtility.FromJsonOverwrite(jsonString, objectToLoad);
        Debug.Log($"Loaded object of type {objectToLoad.GetType().Name} from the {propertyName} Player Prefs property");
    }
}
