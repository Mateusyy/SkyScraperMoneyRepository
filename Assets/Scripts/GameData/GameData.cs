using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameData : MonoBehaviour
{
    public static GameData instance { get; private set; }

    [SerializeField]
    private SlotData[] slots;
    [SerializeField]
    private ManagerData[] managers;
    [SerializeField]
    private UpgreadeData[] upgreades;
    [SerializeField]
    private BuildingMapData[] buildingMapData;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as GameData;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        Assert.IsTrue(slots.Length > 0);
        Assert.IsTrue(managers.Length > 0);
    }

    public int numberOfSlots
    {
        get { return slots.Length; }
    }

    public SlotData GetDataForSlot(int index)
    {
        Assert.IsTrue(index >= 0 && index < slots.Length);
        return slots[index];
    }

    public int numberOfManagers
    {
        get { return managers.Length; }
    }

    public ManagerData GetDataForManager(int index)
    {
        Assert.IsTrue(index >= 0 && index < managers.Length);
        return managers[index];
    }

    public int numberOfUpgreades
    {
        get { return upgreades.Length; }
    }

    public UpgreadeData GetDataForUpgreade(int index)
    {
        Assert.IsTrue(index >= 0 && index < upgreades.Length);
        return upgreades[index];
    }

    public BuildingMapData GetDataForBuildingMap(int index)
    {
        Assert.IsTrue(index >= 0 && index < buildingMapData.Length);
        return buildingMapData[index];
    }

    public int numberOfBuildingMap
    {
        get { return buildingMapData.Length; }
    }
}
