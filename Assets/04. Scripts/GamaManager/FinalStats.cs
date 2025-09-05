using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStats : MonoBehaviour
{
        public float finalAttack;
        public float finalCritChance;
        public float finalCritDamage;

        public void Calculate(PlayerData player, Temp_WeaponModel weapon)
        {
            // 간단한 계산 공식 (임시용)
            finalAttack = player.baseAttack + (weapon.baseAttack + weapon.level * weapon.attackGrowth);
            finalCritChance = player.baseCritRate + weapon.critBonus;
            finalCritDamage = 1.5f + weapon.critMultiplier;
        }
}
