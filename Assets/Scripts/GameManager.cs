using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] RoverController rover;
    [SerializeField] RoverTelemetryUI telemetryUI;

    void Awake()
    {
        if (rover != null && telemetryUI != null)
            rover.TelemetryUpdated += telemetryUI.UpdateReadings;
    }

    void OnDestroy()
    {
        if (rover != null && !(telemetryUI is null))
            rover.TelemetryUpdated -= telemetryUI.UpdateReadings;
    }
}
