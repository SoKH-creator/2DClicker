using System.Collections.Generic;
using UnityEngine;


public enum StatType
 {
    Gold,                // 골드
    ATK,                 // 공격력
    AutoAttackSpeed,     // 자동 공격 속도
    Critical,            // 치명타
    CritChance,          // 치명타 확률
    CritDamage,          // 치명타 피해

}

[CreateAssetMenu(fileName = "New StatData", menuName = "Stats/Chracter Stats")]

public class StatData : ScriptableObject
{
    public string characterName;
    public List<StatEntry> stats;
}

[System.Serializable]
public class StatEntry
{
    public StatType statType;
    public float baseValue;
}