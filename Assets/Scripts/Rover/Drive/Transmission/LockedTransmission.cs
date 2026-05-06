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

    protected override float CalculateEffectiveOffset(List<WheelCollider> wheels, Transform roverCenterOfRotation)
    {
        float xOffsetSum = 0f;
        foreach (WheelCollider wheel in wheels)
        {
            Vector3 worldPositionDelta = wheel.transform.position - roverCenterOfRotation.transform.position;
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
