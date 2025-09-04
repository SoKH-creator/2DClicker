using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Observer
public class WeaponSlotUI : MonoBehaviour
{
    public Image icon;

    [Header("UI texts")]
    public TextMeshProUGUI weaponNameLv;
    public TextMeshProUGUI weaponDesc;
    public TextMeshProUGUI exp;
        
    private WeaponModel _model;

    private void OnDisable()
    {
        if (_model != null) _model.OnChanged -= Refresh;
    }

    public void Bind(WeaponModel souce) 
    {
        // 이전 구독 있으면 취소
        if (_model != null) _model.OnChanged -= Refresh;

        _model = souce;

        // 활성 상태일 때만 구독
        if (isActiveAndEnabled)
            _model.OnChanged += Refresh;
    
        Refresh();
    }
    void Refresh()
    {
        icon.sprite = _model.Data.icon;
        weaponNameLv.text = _model.Data.weaponName + " Lv." + _model.State.level.ToString();
        weaponDesc.text = "+ 공격력 " + _model.GetAttack().ToString() + "%\n" +
            "+ 치명타 확률 " + _model.GetCrit().ToString() + "%";
        exp.text = _model.GetRequiredExp().ToString();
    }
}
