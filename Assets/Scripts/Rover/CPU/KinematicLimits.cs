using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The envelope that manages caching of derived rover parameters, such as maximum linear velocity (derived from maximum RPM and wheel radius)
/// and maximum angular velocity (derived from wheels configuration and maximum linear velocity).
/// </summary>
public class KinematicLimits : IDisposable
{
    RoverConfig roverConfig;
    Func<float> maxLinearVelocityCalculator, maxAngularVelocityCalculator;
    float cachedMaxLinearVelocity;
    /// <summary>
    /// The accessor implements lazy recalculation of the cached maximum velocity,
    /// which is performed when retrieving the underlying field value.
    /// </summary>
    public float MaxLinearVelocity
    {
        get
        {
            if (recalculationRequired)
                RecalculateCachedValues();
            return cachedMaxLinearVelocity;
        }
    }
    float cachedMaxAngularVelocity;
    /// <summary>
    /// The accessor implements lazy recalculation of the cached maximum velocity,
    /// which is performed when retrieving the underlying field value.
    /// </summary>
    public float MaxAngularVelocity
    {
        get
        {
            if (recalculationRequired)
                RecalculateCachedValues();
            return cachedMaxAngularVelocity;
        }
    }
    bool recalculationRequired = true;


    public KinematicLimits(RoverConfig roverConfig, Func<float> maxLinearVelocityCalculator, Func<float> maxAngularVelocityCalculator)
    {
        this.roverConfig = roverConfig;
        this.maxLinearVelocityCalculator = maxLinearVelocityCalculator;
        this.maxAngularVelocityCalculator = maxAngularVelocityCalculator;

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
        cachedMaxLinearVelocity = maxLinearVelocityCalculator();
        cachedMaxAngularVelocity = maxAngularVelocityCalculator();
        recalculationRequired = false;
    }

    public void Dispose()
    {
        roverConfig.UnsubscribeFromMotorUpdates(MarkForRecalculation);
    }
}
