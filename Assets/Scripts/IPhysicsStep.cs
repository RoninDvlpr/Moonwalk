/// <summary>
/// Represents an object that gets notified of the next physics step
/// in a FixedUpdate() method of an encapsulating MonoBehaviour.
/// </summary>
public interface IPhysicsStep
{
    /// <summary>
    /// Notify this object about the next physics step.
    /// </summary>
    public void OnFixedUpdate();
}
