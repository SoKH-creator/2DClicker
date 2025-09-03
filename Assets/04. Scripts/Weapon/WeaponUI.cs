using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Observer
public class WeaponUI : MonoBehaviour
{
    public Sprite icon;

    [Header("UI texts")]
    public TextMeshPro weaponNameLv;
    public TextMeshPro weaponDesc;
    public TextMeshPro exp;
    
    [Header("Weapon Info")]
    public string weaponName;
    public string stat;
    public int requiredExp;

    private WeaponActor _actor;

    private void OnEnable()
    {
        _actor.OnChanged += Refresh;
    }
    private void OnDisable()
    {
        _actor.OnChanged -= Refresh;
    }

    void Bind(WeaponActor souce) { _actor = souce; }
    void Refresh()
    {
        icon = _actor.GetIcon();
        weaponNameLv.text = _actor.GetName() + " Lv." + _actor.GetLevel().ToString();
        weaponDesc.text = "+ 공격력 " + _actor.GetAttack().ToString() + "%\n" +
            "+ 치명타 확률 " + _actor.GetCrit().ToString() + "%";
        exp.text = _actor.GetRequiredExp().ToString();
    }
}
