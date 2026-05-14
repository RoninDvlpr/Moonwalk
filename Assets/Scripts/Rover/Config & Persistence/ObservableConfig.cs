using System;
using UnityEngine;

/// <summary>
/// The base config class that implements the observer pattern combined with deserialization.
/// </summary>
[Serializable] public abstract class ObservableConfig : ISerializationCallbackReceiver
{
    /// <summary>
    /// This event is fired when any updates are made to the config.
    /// </summary>
    public event Action OnConfigUpdated;


    /// <summary>
    /// Notifies all the subscribers by firing the OnConfigUpdated event.
    /// </summary>
    protected void NotifyAboutChanges() => OnConfigUpdated?.Invoke();

    /// <summary>
    /// Triggers on Unity inspector changes.
    /// </summary>
    protected virtual void OnValidate() => NotifyAboutChanges();

    public virtual void OnAfterDeserialize() => NotifyAboutChanges();
    public virtual void OnBeforeSerialize() { }

    /// <summary>
    /// Applies serialized JSON overrides without breaking event links.
    /// Gracefully handles deserialization of older config versions by applying overrides only to the matching fields.
    /// </summary>
    /// <param name="json">The JSON string holding overrides</param>
    public void ApplyJSONOverrides(string json) => JsonUtility.FromJsonOverwrite(json, this);

    /// <summary>
    /// Copies all the applicable field values from a source object to this object.
    /// The copy is performed by converting the source object into an intermediate JSON string
    /// and applying this string as JSON overrides through the same method that performs JSON deserialization.
    /// </summary>
    /// <param name="objectToCopyFrom">The source object</param>
    /// <returns>The value that indicates whether the operation completed successfully (true)
    /// or the operation was aborted due to an error (false)</returns>
    public bool CopyObjectFields(ObservableConfig objectToCopyFrom)
    {
        if (objectToCopyFrom == null)
        {
            Debug.LogWarning("Can't copy object fields since the provided object is null!");
            return false;
        }
        else if (objectToCopyFrom.GetType() != this.GetType())
        {
            Debug.LogError($"Type mismatch: Can't copy a {objectToCopyFrom.GetType()} into the {this.GetType()}");
            return false;
        }

        string json = JsonUtility.ToJson(objectToCopyFrom);
        ApplyJSONOverrides(json);
        return true;
    }
}
