using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRuntime
{
    [Header("Equipped Weapon ID")]
    public string equippedId;

    private Dictionary<string, WeaponModel> _models;
    public Dictionary<string, WeaponModel> Models => _models;

    // 장착 변경 관리
    public event Action OnEquippedChanged;

    public void Init()
    {
        WeaponDatabase.Init();
        _models = new();
        
        foreach (var data in WeaponDatabase.Dict)
        {
            _models[data.Key] = new WeaponModel(data.Key);
        }

        // set basic weapon if no equipped weapon
        if (string.IsNullOrEmpty(equippedId))
            TryEquip("Axe01");
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
        // 잠금 해제된 것만 장착
        if (!_models[id].State.unlocked)
            return false;

        // 이미 해당 무기를 장착 중이면 패스
        if (id == equippedId)
            return true;

        equippedId = id;
        OnEquippedChanged?.Invoke(); // 장착 변경 이벤트 호출
        return true;
    }

}
