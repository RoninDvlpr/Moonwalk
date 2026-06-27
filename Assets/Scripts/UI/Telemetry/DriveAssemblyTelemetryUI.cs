using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DriveAssemblyTelemetryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI assemblyName, targetVelocity, targetRPM, currentRPM, currentTorque;

    public void UpdateReadings(DriveAssemblyTelemetryContainer telemetryContainer)
    {
        assemblyName.text = telemetryContainer.assemblyName;
        targetVelocity.text = telemetryContainer.targetVelocity.ToString("F0");
        targetRPM.text = telemetryContainer.targetRPM.ToString("F0");
        currentRPM.text = telemetryContainer.currentRPM.ToString("F0");
        currentTorque.text = telemetryContainer.currentTorque.ToString("F0");
    }
}
