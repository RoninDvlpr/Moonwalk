using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single-speed transmission where wheels connected to the motor through a shaft, chain or belt
/// without and intermediarry gearbox.
/// </summary>
public class LockedTransmission : BaseTransmission
{
    public LockedTransmission(List<WheelCollider> wheels, Transform roverCenterOfRotation) : base(wheels, roverCenterOfRotation) { }

    /// <summary>
    /// If the provided rover center of rotation is null, the method considers the center of rotation to be at Vector3.zero.
    /// </summary>
    /// <param name="wheels">The transmission's wheels</param>
    /// <param name="roverCenterOfRotation">The pivot that the rover rotates around when turning</param>
    /// <returns>Effective X offset value for use in kinematics calculation</returns>
    protected override float CalculateEffectiveOffset(List<WheelCollider> wheels, Transform roverCenterOfRotation)
    {
        Vector3 pointOfRotation = roverCenterOfRotation?.position ?? Vector3.zero;
        float xOffsetSum = 0f;
        foreach (WheelCollider wheel in wheels)
        {
            Vector3 worldPositionDelta = wheel.transform.position - pointOfRotation;
            float deltaProjectedOnXAxis = Vector3.Dot(worldPositionDelta, wheel.transform.right);
            xOffsetSum += deltaProjectedOnXAxis;
        }
            
        return xOffsetSum/wheels.Count;
    }

    protected override float CalculateEffectiveRadius(List<WheelCollider> wheels)
    {
        float radiusSum = 0f;
        foreach (WheelCollider wheel in wheels)
            radiusSum += wheel.radius;
        return radiusSum / wheels.Count;
    }

    public override void DistributeTorque(float torque)
    {
        float torquePerWheel = torque / wheels.Count;
        foreach (WheelCollider wheel in wheels)
            wheel.motorTorque = torquePerWheel;
    }
}
