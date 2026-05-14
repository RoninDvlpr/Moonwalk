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
            return cachedMaxLinearSpeed;
        }
    }
    bool recalculationRequired = true;


    public KinematicLimits(RoverConfig roverConfig, Func<float> maxLinearSpeedCalculator, Func<float> maxAngularSpeedCalculator)
    {
        this.roverConfig = roverConfig;
        this.maxLinearSpeedCalculator = maxLinearSpeedCalculator;
        this.maxAngularSpeedCalculator = maxAngularSpeedCalculator;

        roverConfig.SubscribeToMotorMaxRPMChangedEvents(MarkForRecalculation);
        RecalculateCachedValues();
    }

    void MarkForRecalculation() => recalculationRequired = true;

    void RecalculateCachedValues()
    {
        cachedMaxLinearSpeed = maxLinearSpeedCalculator();
        cachedMaxAngularSpeed = maxAngularSpeedCalculator();
        recalculationRequired = false;
    }

    public void Dispose()
    {
        roverConfig.UnsubscribeToMotorMaxRPMChangedEvents(MarkForRecalculation);
    }
}
