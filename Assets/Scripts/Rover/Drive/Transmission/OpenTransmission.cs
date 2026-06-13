using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an open differential transmission.
/// Torque is divided equally among all wheels, allowing them to spin at different speeds.
/// </summary>
public class OpenTransmission : BaseTransmission
{
    public OpenTransmission(IReadOnlyCollection<WheelCollider> wheels, Transform roverCenterOfRotation) : base(wheels, roverCenterOfRotation) { }

    public override float GetCurrentRpm() => CalculateAverageRpm(wheels);

    /// <summary>
    /// Torque is split equally among all connected wheels, regardless of traction.
    /// </summary>
    public override void DistributeTorque(float torque) => DistributeTorqueEqually(torque, wheels);
}
