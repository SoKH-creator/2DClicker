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

    public void Init()
    {
        weaponData = new WeaponData();
        
    }
    public bool TryUnlock(ref int exp)
    {
        if (exp >= weaponData.requiredExp)
        {
            exp -= weaponData.requiredExp;

            weaponData.requiredExp += weaponData.deltaRequiredExp;
            
            return true;
        }
        else
        {
            return false;
        }            
    }
    public bool TryUpgrade(ref int exp)
    {
        if (exp >= weaponData.requiredExp)
        {
            exp -= weaponData.requiredExp;

            weaponData.baseAttack += weaponData.deltaAttack;
            weaponData.baseCritRate += weaponData.deltaCritRate;
            weaponData.requiredExp += weaponData.deltaRequiredExp;
            
            return true;
        }
        else
        {
            return false ;
        }
    }
    public void Unlock() 
    {

    }
    public float GetAttack() 
    {
        /*not yet dicided*/ return 0;
    }
    public float GetCrit()
    {
        /*not yet dicided*/ return 0;
    }
}

