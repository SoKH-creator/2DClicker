using UnityEngine;

// 업그레이드 데이터 ScriptableObject
[CreateAssetMenu(fileName = "UpgradeData", menuName = "Game/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public StatType statType;
    public string upgradeName;
    public string description;

    public float baseValue;            // 시작 값
    public float increasePerLevel;     // 레벨당 증가치

    public int baseCost;               // 시작 비용
    public float costIncreasePerLevel; // 레벨당 비용 증가량

    public int maxLevel = 0;           // 0 = 무제한

    // 현재 레벨에서의 값 계산
    public float GetValueAtLevel(int level)
    {
        return baseValue + (increasePerLevel * level);
    }

    // 현재 레벨에서의 비용 계산
    public int GetCostAtLevel(int level)
    {
        return Mathf.RoundToInt(baseCost + (costIncreasePerLevel * level));
    }

    // 최대 레벨 도달 여부
    public bool IsMaxLevel(int level)
    {
        return (maxLevel > 0 && level >= maxLevel);
    }
}
