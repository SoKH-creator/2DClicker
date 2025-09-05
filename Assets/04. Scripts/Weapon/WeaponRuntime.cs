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
    
    public event Action OnEquippedChanged; // 장착 변경 관리
    public event Action<string> OnAnyModelChanged; // 아무거나 변경 시 (id 알림)

    public void Init()
    {
        WeaponDatabase.Init();
        _models = new();
        
        foreach (var kv in WeaponDatabase.Dict)
        {
            _models[kv.Key] = new WeaponModel(kv.Key);

            _models[kv.Key].OnChanged += () => OnAnyModelChanged?.Invoke(kv.Key);
        }

        // 기본 무기 없을 시, 기본 무기 장착
        string defaultId = "Axe01";
        if (_models.TryGetValue(defaultId, out WeaponModel model))
        {
            model.State.unlocked = true;
            TryEquip(defaultId);
        }
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

        OnAnyModelChanged?.Invoke(id);
        OnEquippedChanged?.Invoke(); // 장착 변경 이벤트 호출
        return true;
    }

}
