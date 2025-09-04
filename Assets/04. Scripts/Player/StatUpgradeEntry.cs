using UnityEngine;

[System.Serializable]
public class StatUpgradeEntry
{
    public StatType statType;       // 어떤 능력치인지
    public float baseValue;         // 시작 값
    public float incrementPerLevel; // 레벨업할 때마다 증가하는 값

    // 특정 레벨일 때 최종 수치 반환
    public float GetValueAtLevel(int level)
    {
        return baseValue + (incrementPerLevel * level);
    }
}
