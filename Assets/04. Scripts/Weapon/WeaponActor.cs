using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActor
{
    private WeaponData _data;
    private WeaponState _state;
    
    public WeaponData Data { get => _data; }
    public WeaponState State { get => _state; set => _state = value; }

    public event Action OnChanged;

    public WeaponActor(string id)
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
    public void Unlock(ref int exp) 
    {
        if (exp >= _data.requiredExp)
        {
            exp -= _data.requiredExp;
            OnChanged?.Invoke();
            _state.unlocked = true;
        }
        else { }
    }

    public Sprite GetIcon() { return _data.icon; }
    public string GetName() { return _data.name; }
    public int GetLevel() { return _state.level; }
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

