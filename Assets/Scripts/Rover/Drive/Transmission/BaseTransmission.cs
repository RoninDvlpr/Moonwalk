using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines rules for calculating kinematic coefficients based on the wheel positions.
/// Responsible for transferring and distributing the motor torque among the assigned set of wheels.
/// </summary>
public abstract class BaseTransmission
{
    protected IReadOnlyCollection<WheelCollider> wheels;
    /// <summary>
    /// The effective X offset is calculated relative to this transform, taking into account its local X direction.
    /// For more precise calculations of complex drive assemblies, the assembly mounting point may also be taken into consideration.
    /// To accommodate this, an overload of the AssignWheels() method
    /// that accepts the additional transform parameter for the mounting point may be introduced.
    /// </summary>
    Transform roverCenterOfRotation;
    /// <summary>
    /// The X offset of an abstract virtual wheel that represents all the transmission wheels.
    /// Currently cached and needs to be recalculated if wheels position changes relative to the rover.
    /// </summary>
    public float EffectiveXOffset { get; private set; }
    /// <summary>
    /// The radius of an abstract virtual wheel that represents all the transmission wheels.
    /// Currently cached and needs to be recalculated if the radius of the wheels changes.
    /// </summary>
    public float EffectiveRadius { get; private set; }


    public BaseTransmission(IReadOnlyCollection<WheelCollider> wheels, Transform roverCenterOfRotation)
    {
        AssignWheels(wheels, roverCenterOfRotation);
    }

    /// <summary>
    /// DI interface. 
    /// </summary>
    /// <param name="newWheelsList">A set of wheels the transmission is responsible for.</param>
    /// <param name="roverCenterOfRotation">The point the rover is supposed to rotate around during turns.</param>
    public void AssignWheels(IReadOnlyCollection<WheelCollider> newWheelsList, Transform roverCenterOfRotation)
    {
        if (newWheelsList == null)
            Debug.LogWarning("Can't assign transmission's wheels list: the provided wheels list is null.");
        else
            wheels = newWheelsList;

        if (roverCenterOfRotation == null)
            Debug.LogWarning("Can't assign transmission's center of rotation: the provided Transform is null.");
        else
            this.roverCenterOfRotation = roverCenterOfRotation;

        CacheEffectiveValues();
    }

    /// <summary>
    /// Cache the derived effective parameters. Can be overridden in cases where some parameters, such as effective radius,
    /// need to be calculated dynamically for a more accurate simulation.
    /// </summary>
    protected virtual void CacheEffectiveValues()
    {
        EffectiveXOffset = CalculateEffectiveOffset(wheels, roverCenterOfRotation);
        EffectiveRadius = CalculateEffectiveRadius(wheels);
    }

    protected abstract float CalculateEffectiveOffset(IReadOnlyCollection<WheelCollider> wheels, Transform roverCenterOfRotation);
    protected abstract float CalculateEffectiveRadius(IReadOnlyCollection<WheelCollider> wheels);
    public abstract void DistributeTorque(float torque);
}
