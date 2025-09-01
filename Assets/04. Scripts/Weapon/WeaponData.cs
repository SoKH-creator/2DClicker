using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponStat { Attack, CritRate }

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponId;
    public string displayName;
    public Sprite icon;
}
