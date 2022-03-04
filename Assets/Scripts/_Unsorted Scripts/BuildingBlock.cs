using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    [SerializeField] private bool isBreakable = false;

    public bool IsBreakable { get => isBreakable; set => isBreakable = value; }
}
