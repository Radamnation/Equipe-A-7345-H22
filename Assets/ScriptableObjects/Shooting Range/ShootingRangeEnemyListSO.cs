using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Shooting Range/Enemy List", fileName = "SO _ ShootingRangeEnemyList")]
public class ShootingRangeEnemyListSO : ScriptableObject
{
    public List<GameObject> myEnemyList = new List<GameObject>();
}
