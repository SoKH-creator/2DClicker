using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRuntime
{
    [Header("Equipped Weapon ID")]
    public string equippedId;

    private Dictionary<string, WeaponActor> _actors;

    public void Init()
    {
        WeaponDatabase.Init();
        _actors = new();
        
        foreach (var data in WeaponDatabase.dict)
        {
            _actors[data.Key] = new WeaponActor(data.Key);
        }

        // set basic weapon if no equipped weapon
        if (equippedId == "")
            equippedId = "Axe01";
    }
    public void Apply(WeaponsSave save)
    {
        foreach (var state in save.states)
            _actors[state.id].State = state;

        equippedId = save.equippedWeaponID;
    }
    public WeaponsSave ToSave() 
    {
        WeaponsSave save = new WeaponsSave();
        foreach (var actor in _actors.Values)
        {
            save.states.Add(actor.State);
            save.equippedWeaponID = equippedId;
        }
        return save;
    }
    public bool TryEquip(string id) 
    {
        if (_actors[id].State.unlocked)
        {
            return false;
        }
        else
        {
            equippedId = id;
            return true;
        }
    }
}
