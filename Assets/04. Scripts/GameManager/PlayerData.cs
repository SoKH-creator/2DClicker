using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int gold = 1000;

    // FinalStats 계산용 스탯들
    public float baseAttack = 10f;
    public float baseCritRate = 0.1f;      // 치명타 확률 (10%)
    public float baseCritDamage = 1.5f;    // 치명타 데미지 (1.5배)
}