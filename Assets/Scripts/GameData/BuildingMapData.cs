using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingMapData", menuName = "Game Data/Building Map Data", order = 1)]
public class BuildingMapData : ScriptableObject
{
    public bool isActive;
    public float cost;
}
