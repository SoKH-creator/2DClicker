using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel
{
    private WeaponData _data;
    private WeaponState _state;
    
    public WeaponData Data { get => _data; }
    public WeaponState State { get => _state; set => _state = value; }

    public event Action OnChanged;

    public WeaponModel(string id)
    {
        _data = WeaponDatabase.GetWeaponData(id);
        _state = new WeaponState(id);
    }

    public bool TryUnlock(ref int exp)
    {
        if (exp >= _data.requiredExp)
        {
            exp -= _data.requiredExp;
            _data.requiredExp += _data.deltaRequiredExp;
            _state.unlocked = true;
            OnChanged?.Invoke();
            return true;
        }
        else { return false; }            
    }
    public bool TryUpgrade(ref int exp)
    {
        if (exp >= _data.requiredExp)
        {
            exp -= _data.requiredExp;

            _data.baseAttack += _data.deltaAttack;
            _data.baseCritRate += _data.deltaCritRate;
            _data.requiredExp += _data.deltaRequiredExp;
            OnChanged?.Invoke();
            return true;
        }
        else { return false; }
    }

    public float GetAttack()
    {
        float atk = _data.baseAttack;
        
        for (int i = 0; i< _state.level; i++)
            atk += _data.deltaAttack;

        return atk;
    }
    public float GetCrit()
    {
        float crit = _data.baseCritRate;
        
        for (int i = 0; i < _state.level; i++)
            crit += _data.deltaCritRate;

        return crit;
    }
    public int GetRequiredExp()
    {
        int exp = _data.requiredExp;

        for (int i = 0; i < _state.level; i++)
            exp += _data.deltaRequiredExp;
        
        return exp;
    }
}

