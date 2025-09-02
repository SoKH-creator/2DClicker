using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActor : MonoBehaviour
{
    [Header("Weapon Info")]
    public string ID;
    public int level;
    public bool unlocked;

    [NonSerialized]
    private WeaponData weaponData;
    
    //public event Action OnChanged;

    public bool TryUnlock(ref int exp) { return true; }
    public bool TryUpgrade(ref int exp) { return true; }
    public float GetAttack() { /*not yet dicided*/ return 0; }
    public float GetCrit() { /*not yet dicided*/ return 0; }
}

