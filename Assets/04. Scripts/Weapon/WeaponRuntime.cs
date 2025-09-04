using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRuntime
{
    [Header("Equipped Weapon ID")]
    public string equippedId;

    private Dictionary<string, WeaponModel> _models;
    public Dictionary<string, WeaponModel> Models => _models;

    public void Init()
    {
        WeaponDatabase.Init();
        _models = new();
        
        foreach (var data in WeaponDatabase.Dict)
        {
            _models[data.Key] = new WeaponModel(data.Key);
        }

        // set basic weapon if no equipped weapon
        if (equippedId == "")
            equippedId = "Axe01";
    }
    public void Apply(WeaponsSave save)
    {
        foreach (var state in save.states)
            _models[state.id].State = state;

        equippedId = save.equippedWeaponID;
    }
    public WeaponsSave ToSave() 
    {
        WeaponsSave save = new WeaponsSave();
        foreach (var actor in _models.Values)
        {
            save.states.Add(actor.State);
            save.equippedWeaponID = equippedId;
        }
        return save;
    }
    public bool TryEquip(string id) 
    {
        if (_models[id].State.unlocked)
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
