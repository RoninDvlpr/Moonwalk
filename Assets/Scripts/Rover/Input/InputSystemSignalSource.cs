using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// A non-simulation signal source (ISignalSource) implementation
/// that polls the signal values directly from the Unity Input System.
/// </summary>
public class InputSystemSignalSource : ISignalSource
{
    readonly InputAction moveAction;

    #region Initialization

    public InputSystemSignalSource()
    {
        CacheInputActionReference(InputSystem.actions, "Move", ref moveAction);
    }

    public InputSystemSignalSource(string moveActionName)
    {
        CacheInputActionReference(InputSystem.actions, moveActionName, ref moveAction);
    }

    public InputSystemSignalSource(InputActionAsset actionAsset, string moveActionName)
    {
        CacheInputActionReference(actionAsset, moveActionName, ref moveAction);
    }

    void CacheInputActionReference(InputActionAsset actionAsset, string actionName, ref InputAction cachedReference)
    {
        if (actionAsset == null)
        {
            Debug.LogError($"Can't cache an InputActionReference for the {actionName} action since the provided InputActionAsset in null!");
            return;
        }

        InputAction foundAction = actionAsset.FindAction(actionName);
        if (foundAction == null)
        {
            Debug.LogError($"The action {actionName} isn't found in the provided InputActionAsset {actionAsset.name}.");
            return;
        }

        cachedReference = foundAction;
    }

    #endregion


    public Vector2 GetCurrentSignal()
    {
        return moveAction.ReadValue<Vector2>();
    }
}
