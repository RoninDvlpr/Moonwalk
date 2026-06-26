/// <summary>
/// Represents a mechanical device connected to a motor and powered by it.
/// </summary>
public interface IMotorLoad
{
    float GetCurrentRPM();
    void ApplyTorque(float torque);
}
