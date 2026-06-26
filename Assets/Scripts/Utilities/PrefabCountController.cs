using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the number of prefab instances under the given parent container.
/// </summary>
/// <typeparam name="T">A type of instance to manage</typeparam>
public class PrefabCountController<T> where T : MonoBehaviour
{
    T prefab;
    Transform container;
    public List<T> instances;


    public PrefabCountController(T instancePrefab, Transform instancesContainer)
    {
        prefab = instancePrefab;
        container = instancesContainer;
        instances = new List<T>();
    }

    public PrefabCountController(T instancePrefab, Transform instancesContainer, int initialCount) : this(instancePrefab, instancesContainer)
    {
        AdjustInstancesCount(initialCount);
    }

    /// <summary>
    /// Ensures there's exactly the provided number of instances.
    /// </summary>
    /// <param name="targetNumber">Defines how many instances should exist</param>
    public void AdjustInstancesCount(int targetCount)
    {
        while (instances.Count != targetCount)
            if (targetCount > instances.Count)
                CreateInstance();
            else if (targetCount < instances.Count)
                RemoveLastInstance();
    }

    /// <summary>
    /// Removes and destroys given number of instances from the end of the list.
    /// </summary>
    /// <param name="numberToRemove">The number of instances to remove. If negative, its absolute value will be used.</param>
    void RemoveInstances(int numberToRemove)
    {
        numberToRemove = Mathf.Abs(numberToRemove);

        if (numberToRemove > instances.Count)
        {
            Debug.LogWarning($"Trying to remove {numberToRemove} instances while there's only {instances.Count} instances currently present.");
            numberToRemove = instances.Count;
        }

        for (int i = 0; i < numberToRemove; i++)
            RemoveLastInstance();
    }

    void RemoveInstance(int instanceIndex)
    {
        T instance = instances[instanceIndex];
        instances.RemoveAt(instanceIndex);
        Object.Destroy(instance);
    }

    void RemoveLastInstance() => RemoveInstance(instances.Count - 1);

    /// <summary>
    /// Instantiates a number of instances and adds them to the list.
    /// </summary>
    /// <param name="numberToCreate">The number of instances to create. If negative, its absolute value will be used.</param>
    void CreateInstances(int numberToCreate)
    {
        numberToCreate = Mathf.Abs(numberToCreate);

        for (int i = 0; i < numberToCreate; i++)
            CreateInstance();
    }

    void CreateInstance()
    {
        T spawnedInstance = Object.Instantiate(prefab, container);
        instances.Add(spawnedInstance);
    }
}
