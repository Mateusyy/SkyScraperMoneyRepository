using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public TaskType Type;
    [SerializeField]
    public float ValueToCollect;
    [SerializeField]
    public float CurrentValueOfCollection;
    [SerializeField]
    public int IndexOfFloor;

    public Task()
    {
    }

    public Task(string name, TaskType type, float valueToCollect, float currentValueOfCollection)
    {
        Name = name;
        Type = type;
        ValueToCollect = valueToCollect;
        CurrentValueOfCollection = currentValueOfCollection;
    }

    public Task(string name, TaskType type, float valueToCollect, float currentValueOfCollection, int indexOfFloor)
        : this(name, type, valueToCollect, currentValueOfCollection)
    {
        IndexOfFloor = indexOfFloor;
    }
}