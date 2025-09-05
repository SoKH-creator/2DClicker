using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStats : MonoBehaviour
{
        public float finalAttack;
        public float finalCritChance;

        public void Calculate(PlayerData player, WeaponRuntime weapon)
        {
            // 간단한 계산 공식 (임시용)
            finalAttack = player.baseAttack + weapon.Models[weapon.equippedId].GetAttack();
            finalCritChance = player.baseCritRate + weapon.Models[weapon.equippedId].GetCrit();
    }
}
