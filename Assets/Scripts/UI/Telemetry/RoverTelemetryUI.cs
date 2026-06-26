using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverTelemetryUI : MonoBehaviour
{
    [SerializeField] DriveAssemblyTelemetryUI assemblyTelemetryEntryPrefab;
    [SerializeField] Transform assemblyTelemetryEntriesContainer;
    PrefabCountController<DriveAssemblyTelemetryUI> countController;

    void Start()
    {
        countController = new PrefabCountController<DriveAssemblyTelemetryUI>(assemblyTelemetryEntryPrefab, assemblyTelemetryEntriesContainer);
    }

    public void UpdateReadings(RoverTelemetryContainer roverTelemetry)
    {
        countController.AdjustInstancesCount(roverTelemetry.assemblyTelemetries.Count);

        for (int i = 0; i < roverTelemetry.assemblyTelemetries.Count; i++)
            countController.instances[i].UpdateReadings(roverTelemetry.assemblyTelemetries[i]);
    }
}
