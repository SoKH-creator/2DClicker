using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_WeaponModel : MonoBehaviour
{
    public float baseAttack = 5f;
    public int level = 3;
    public float attackGrowth = 2f;

    public float critBonus = 0.05f;      // 치명타 확률 +5%
    public float critMultiplier = 0.3f;  // 치명타 데미지 +30%

    public GameObject instance => this.gameObject;

    public float GetAttack()
    {
        return baseAttack + level * attackGrowth;
    }

    public float GetCrit()
    {
        return critBonus;
    }

    public float GetCritDamage()
    {
        return critMultiplier;
    }
}
