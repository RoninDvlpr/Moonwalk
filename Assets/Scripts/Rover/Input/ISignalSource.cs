using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface that represents a generic signal source the receiver module interacts with (samples).
/// Can represent an RF antenna, a physical wire jack, or a user input device.
/// </summary>
public interface ISignalSource
{
    /// <summary>
    /// Get the current input signal from the source.
    /// </summary>
    /// <returns>Current rover input signal as a Vector2</returns>
    public Vector2 GetCurrentSignal();
}
