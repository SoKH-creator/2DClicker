using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    private WeaponData weaponData;

    [Header("Weapon Info")]
    public int level;
    public string Id => weaponData.id;
    public string Name => weaponData.weaponName;
    public float Attack => GetAttack(level);
    public float Crit => GetCrit(level);

    public float GetAttack(int level) { /*not yet dicided*/ return 0; }
    public float GetCrit(int level) { /*not yet dicided*/ return 0; }
}

