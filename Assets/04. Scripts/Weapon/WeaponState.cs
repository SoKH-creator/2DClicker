using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState
{
    [Header("Weapon Info")]
    public string id;
    public int level;
    public bool unlocked;

    public WeaponState(string id)
    {
        this.id = id;
        level = 0;
        unlocked = false;
    }
}
