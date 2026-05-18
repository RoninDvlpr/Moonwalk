using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The envelope that manages caching of derived rover parameters, such as maximum linear speed (derived from maximum RPM and wheel radius)
/// and maximum angular speed (derived from wheels configuration and maximum linear speed).
/// </summary>
public class KinematicLimits : IDisposable
{
    RoverConfig roverConfig;
    Func<float> maxLinearSpeedCalculator, maxAngularSpeedCalculator;
    float cachedMaxLinearSpeed;
    /// <summary>
    /// The accessor implements lazy recalculation of the cached maximum speed,
    /// which is performed when retrieving the underlying field value.
    /// </summary>
    public float MaxLinearSpeed
    {
        get
        {
            if (recalculationRequired)
                RecalculateCachedValues();
            return cachedMaxLinearSpeed;
        }
    }
    float cachedMaxAngularSpeed;
    /// <summary>
    /// The accessor implements lazy recalculation of the cached maximum speed,
    /// which is performed when retrieving the underlying field value.
    /// </summary>
    public float MaxAngularSpeed
    {
        get
        {
            if (recalculationRequired)
                RecalculateCachedValues();
            return cachedMaxAngularSpeed;
        }
    }
    bool recalculationRequired = true;


    public KinematicLimits(RoverConfig roverConfig, Func<float> maxLinearSpeedCalculator, Func<float> maxAngularSpeedCalculator)
    {
        this.roverConfig = roverConfig;
        this.maxLinearSpeedCalculator = maxLinearSpeedCalculator;
        this.maxAngularSpeedCalculator = maxAngularSpeedCalculator;

        roverConfig.SubscribeToMotorUpdates(MarkForRecalculation);
        // In case of implementation of wheels configuration changes during gameplay,
        // the cached values should also be recalculated on those changes
        RecalculateCachedValues();
    }

    void MarkForRecalculation() => recalculationRequired = true;

    /// <summary>
    /// Cached values should be recalculated when a configuration they are based on changes.
    /// </summary>
    void RecalculateCachedValues()
    {
        cachedMaxLinearSpeed = maxLinearSpeedCalculator();
        cachedMaxAngularSpeed = maxAngularSpeedCalculator();
        recalculationRequired = false;
    }

    public void Dispose()
    {
        roverConfig.UnsubscribeFromMotorUpdates(MarkForRecalculation);
    }
}
