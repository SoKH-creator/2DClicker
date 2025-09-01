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
    public float Attack => GetAttack();
    public float Crit => GetCrit();

    public float GetAttack() { /*not yet dicided*/ return 0; }
    public float GetCrit() { /*not yet dicided*/ return 0; }
}

