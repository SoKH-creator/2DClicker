using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatUpgradeTable", menuName = "Stats/Player Stat Upgrade Table")]
public class PlayerStatUpgradeTable : ScriptableObject
{
    public List<StatUpgradeEntry> statUpgrades;

    // 특정 능력치의 값 구하기
    public float GetStatValue(StatType statType, int level)
    {
        var entry = statUpgrades.Find(s => s.statType == statType);
        if (entry != null)
            return entry.GetValueAtLevel(level);

        return 0f;
    }
}
