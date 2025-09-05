using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStats : MonoBehaviour
{
    public float finalAttack;
    public float finalCritChance;

    private UpgradeData playerAtkData;
    private UpgradeData playerCritData;

    public void Init()
    {
        playerAtkData = Resources.Load<UpgradeData>("PlayerData/ATK");
        playerCritData = Resources.Load<UpgradeData>("PlayerData/CritChance");
    }
    public void Calculate(StatHandler player, WeaponRuntime weapon)
    {
        // 간단한 계산 공식
        finalAttack = player.GetFinalStat(playerAtkData) + weapon.Models[weapon.equippedId].GetAttack();
        finalCritChance = player.GetFinalStat(playerCritData) + weapon.Models[weapon.equippedId].GetCrit();
    }
}
