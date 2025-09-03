using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStats : MonoBehaviour
{
        public float finalAttack;
        public float finalCritChance;
        public float finalCritDamage;

        public void Calculate(PlayerData player, WeaponModel weapon)
        {
            if (player == null || weapon == null)
            {
                UnityEngine.Debug.LogWarning("스탯 계산에 필요한 데이터가 없습니다!");
                return;
            }

            // 간단한 계산 공식 (임시용)
            finalAttack = player.baseAttack + (weapon.baseAttack + weapon.level * weapon.attackGrowth);
            finalCritChance = player.baseCritRate + weapon.critBonus;
            finalCritDamage = 1.5f + weapon.critMultiplier;
        }
}
