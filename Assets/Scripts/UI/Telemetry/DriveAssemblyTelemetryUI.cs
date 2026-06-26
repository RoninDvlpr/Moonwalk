using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DriveAssemblyTelemetryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI assemblyName, targetRPM, currentRPM, currentTorque;

    public void UpdateReadings(DriveAssemblyTelemetryContainer telemetryContainer)
    {
        assemblyName.text = telemetryContainer.assemblyName;
        targetRPM.text = telemetryContainer.targetRPM.ToString("F0");
        currentRPM.text = telemetryContainer.currentRPM.ToString("F0");
        currentTorque.text = telemetryContainer.currentTorque.ToString("F0");
    }
}
