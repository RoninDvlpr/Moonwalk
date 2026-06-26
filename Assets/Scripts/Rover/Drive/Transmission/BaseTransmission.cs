using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines rules for calculating kinematic coefficients based on the wheel positions.
/// Responsible for transferring and distributing the motor torque among the assigned set of wheels.
/// </summary>
public abstract class BaseTransmission : IMotorLoad
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
    /// Caching is currently implemented, so this value needs to be recalculated if wheels position changes relative to the rover.
    /// </summary>
    public float EffectiveXOffset { get; private set; }
    /// <summary>
    /// The radius of an abstract virtual wheel that represents all the transmission wheels.
    /// Caching is currently implemented, so this value needs to be recalculated if the radius of the wheels changes.
    /// </summary>
    public float EffectiveRadius { get; private set; }



    #region Initialization

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

    #endregion


    #region Effective Parameters Getters

    #region Offset
    protected virtual float CalculateEffectiveOffset(IReadOnlyCollection<WheelCollider> wheels, Transform roverCenterOfRotation)
    {
        return CalculateAverageOffset(wheels, roverCenterOfRotation);
    }

    /// <summary>
    /// If the provided rover center of rotation is null, the method considers the center of rotation to be at Vector3.zero.
    /// </summary>
    /// <param name="wheels">The transmission's wheels for which the average offset is calculated</param>
    /// <param name="roverCenterOfRotation">The pivot that the rover rotates around when turning</param>
    /// <returns>Effective X offset value for use in kinematics calculation</returns>
    protected float CalculateAverageOffset(IReadOnlyCollection<WheelCollider> wheels, Transform roverCenterOfRotation = null)
    {
        if (wheels.IsNullOrEmpty())
        {
            Debug.LogWarning($"The wheels collection {wheels} is null or empty, returning average offset of 0.");
            return 0f;
        }

        Vector3 pointOfRotation = roverCenterOfRotation?.position ?? Vector3.zero;
        float xOffsetSum = 0f;
        foreach (WheelCollider wheel in wheels)
        {
            Vector3 worldPositionDelta = wheel.transform.position - pointOfRotation;
            float deltaProjectedOnXAxis = Vector3.Dot(worldPositionDelta, wheel.transform.right);
            xOffsetSum += deltaProjectedOnXAxis;
        }

        return xOffsetSum / wheels.Count;
    }
    #endregion


    #region Radius
    protected virtual float CalculateEffectiveRadius(IReadOnlyCollection<WheelCollider> wheels) => CalculateAverageRadius(wheels);

    protected float CalculateAverageRadius(IReadOnlyCollection<WheelCollider> wheelsCollection)
    {
        if (wheelsCollection.IsNullOrEmpty())
        {
            Debug.LogWarning($"The wheels collection {wheelsCollection} is null or empty, returning average radius of 0.");
            return 0f;
        }

        float radiusSum = 0f;
        foreach (WheelCollider wheel in wheelsCollection)
            radiusSum += wheel.radius;
        return radiusSum / wheelsCollection.Count;
    }
    #endregion

    #endregion


    #region RPM Getters

    public abstract float GetCurrentRPM();

    public float CalculateAverageRPM(IReadOnlyCollection<WheelCollider> wheelsCollection)
    {
        if (wheelsCollection.IsNullOrEmpty())
        {
            Debug.LogWarning($"The wheels collection {wheelsCollection} is null or empty, returning current RPM of 0.");
            return 0f;
        }

        float rpmSum = 0f;
        foreach (WheelCollider wheel in wheelsCollection)
            rpmSum += wheel.rpm;
        return rpmSum / wheelsCollection.Count;
    }

    #endregion


    #region Torque Distribution

    public virtual void ApplyTorque(float torque) => DistributeTorque(torque);

    public abstract void DistributeTorque(float torque);

    protected void DistributeTorqueEqually(float torqueAmount, IReadOnlyCollection<WheelCollider> wheelsCollection)
    {
        if (wheelsCollection.IsNullOrEmpty())
        {
            Debug.LogWarning($"The torque hasn't been applied: the wheels collection {wheelsCollection} is null or empty.");
            return;
        }

        float torquePerWheel = torqueAmount / wheelsCollection.Count;
        foreach (WheelCollider wheel in wheelsCollection)
            wheel.motorTorque = torquePerWheel;
    }

    #endregion
}
