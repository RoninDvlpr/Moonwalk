using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveController : IPhysicsStep
{
    List<MotorController> leftSideMotors, rightSideMotors;
    Vector2 currentInput = Vector2.zero;

    public void SetInputCommand(Vector2 newInput)
    {
        currentInput = newInput;
    }

    public void OnFixedUpdate()
    {
        UpdateMotorsTargetVelocity();
    }

    void UpdateMotorsTargetVelocity()
    {
        Debug.Log(currentInput);

        if (leftSideMotors == null)
            Debug.LogWarning("Left side motors isn't assigned!");
        else
            foreach (MotorController motor in leftSideMotors)
                motor?.UpdateTargetVelocity();
    }
}
