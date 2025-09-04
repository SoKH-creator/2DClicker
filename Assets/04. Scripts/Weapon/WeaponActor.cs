using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActor : MonoBehaviour
{
    [NonSerialized]
    private WeaponData _data;
    private WeaponState _state;
    
    public event Action OnChanged;

    public void Init()
    {
        var db = Resources.Load<WeaponDatabase>("WeaponDatabase");
        _data = db.GetWeaponData(_state.id);

        _state = new WeaponState(_data.id);
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

