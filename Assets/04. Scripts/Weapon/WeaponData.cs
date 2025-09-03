using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponStat { Attack, CritRate }

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string weaponName;
    public Sprite icon;

    [Header("Base State")]
    public float baseAttack;
    public float baseCritRate;
    public int requiredExp;

    [Header("Delta Values")]
    public float deltaAttack;
    public float deltaCritRate;
    public int deltaRequiredExp;
}
