using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BiomeInformation : ScriptableObject
{
    public BuildingBlock floorPrefab;
    public BuildingBlock wallPrefab;
    public BuildingBlock ceilingPrefab;
    public DoorBlock doorPrefab;
}
