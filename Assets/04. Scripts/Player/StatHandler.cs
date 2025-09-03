using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    // 플레이어의 업그레이드 레벨 저장
    private Dictionary<StatType, int> upgradeLevels = new Dictionary<StatType, int>();

    // 골드 변수 통합
    private int _gold = 100; // 초기 골드 (테스트용)
    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnGoldChanged?.Invoke(); // 골드 변경 시 이벤트 발생
        }
    }

    public System.Action OnGoldChanged; // 골드 변경 이벤트

    // 특정 능력치 레벨 가져오기
    public int GetLevel(StatType type)
    {
        return upgradeLevels.ContainsKey(type) ? upgradeLevels[type] : 0;
    }

    // 특정 능력치 최종 값 계산
    public float GetFinalStat(UpgradeData upgradeData)
    {
        int level = GetLevel(upgradeData.statType);
        return upgradeData.GetValueAtLevel(level);
    }

    // 업그레이드 시도
    public bool TryUpgrade(UpgradeData upgradeData)
    {
        int currentLevel = GetLevel(upgradeData.statType);

        if (upgradeData.IsMaxLevel(currentLevel))
        {
            Debug.Log($"{upgradeData.upgradeName}은(는) 이미 최대 레벨입니다.");
            return false;
        }

        int cost = upgradeData.GetCostAtLevel(currentLevel);

        if (gold >= cost)
        {
            gold -= cost; // 프로퍼티를 통해 설정 (이벤트 발생)
            upgradeLevels[upgradeData.statType] = currentLevel + 1;

            Debug.Log($"{upgradeData.upgradeName} 업그레이드 성공! Lv {currentLevel + 1}, 현재 값: {upgradeData.GetValueAtLevel(currentLevel + 1)}");
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }
    }
}