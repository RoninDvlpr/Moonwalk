using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVisualModel : MonoBehaviour
{
    [SerializeField] WheelCollider wheelCollider;

    void Update()
    {
        SyncWheelPositionAndRotation();
    }

    void SyncWheelPositionAndRotation()
    {
        if (wheelCollider == null)
        {
            Debug.LogWarning("Can't sync wheel visual model because the wheel collider isn't assigned.");
            return;
        }

        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        transform.SetPositionAndRotation(position, rotation);
    }
}
