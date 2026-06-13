using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single-speed transmission where wheels connected to the motor through a shaft, chain or belt
/// without and intermediarry gearbox.
/// </summary>
public class LockedTransmission : BaseTransmission
{
    public LockedTransmission(IReadOnlyCollection<WheelCollider> wheels, Transform roverCenterOfRotation) : base(wheels, roverCenterOfRotation) { }

    /// <summary>
    /// Calculates the mechanical shaft RPM by only accounting the wheels that touch the ground (aren't airborne).
    /// </summary>
    /// <returns>The average RPM of the wheels with ground traction</returns>
    public override float GetCurrentRpm()
    {
        if (wheels.IsNullOrEmpty())
        {
            Debug.LogWarning($"The wheels collection {wheels} is null or empty, returning current RPM of 0.");
            return 0f;
        }

        List<WheelCollider> groundedWheels = GetGroundedWheels();

        if (groundedWheels.Count > 0)
            return CalculateAverageRpm(groundedWheels);
        else
            return CalculateAverageRpm(wheels);
    }

    /// <summary>
    /// Dynamically routes 100% of the motor's torque away from airborne wheels and onto grounded ones.
    /// </summary>
    /// <param name="torque">The torque to distribute</param>
    public override void DistributeTorque(float torque)
    {
        if (wheels.IsNullOrEmpty())
        {
            Debug.LogWarning($"The torque hasn't been applied: the wheels collection {wheels} is null or empty.");
            return;
        }

        // set the torque of all wheels to 0 to prevent applying force to airborne wheels
        foreach (WheelCollider wheel in wheels)
            wheel.motorTorque = 0f;

        List<WheelCollider> groundedWheels = GetGroundedWheels();

        if (groundedWheels.Count > 0)
            DistributeTorqueEqually(torque, groundedWheels);
        else
            DistributeTorqueEqually(torque, wheels);
    }

    /// <summary>
    /// Returns the list of wheels that have traction.
    /// </summary>
    /// <returns>The list of found wheels standing on a collider</returns>
    List<WheelCollider> GetGroundedWheels()
    {
        List<WheelCollider> groundedWheels = new List<WheelCollider>();
        foreach (WheelCollider wheel in wheels)
            if (wheel.GetGroundHit(out WheelHit hit))
                groundedWheels.Add(wheel);
        return groundedWheels;
    }
}
