using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel
{
    private WeaponData _data;
    private WeaponState _state;

    public WeaponData Data { get { return _data; } }
    public WeaponState State { get { return _state; } set { _state = value; } }

    public event Action OnChanged;

    public WeaponModel(string id)
    {
        _data = WeaponDatabase.GetWeaponData(id);
        _state = new WeaponState(id);
    }

    public bool TryUnlock(int expTotal, out int expSpent)
    {
        int cost = GetRequiredExp();
        if (_state.unlocked || expTotal < cost)
        {
            expSpent = 0;
            return false;
        }
         
        expSpent = cost;
        _state.unlocked = true;
        OnChanged?.Invoke();
        return true;
    }
    public bool TryUpgrade(int expTotal, out int expSpent)
    {
        int cost = GetRequiredExp();
        if (!_state.unlocked || expTotal < cost)
        {
            expSpent = 0;
            return false;
        }
        
        expSpent = cost;
        _state.level += 1;
        OnChanged?.Invoke();
        return true;
    }

    public float GetAttack() => _data.baseAttack + _state.level * _data.deltaAttack;
    public float GetCrit() => _data.baseCritRate + _state.level * _data.deltaCritRate;
    public int GetRequiredExp() => _data.requiredExp + _state.level * _data.deltaRequiredExp;
}

