using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Observer
public class WeaponSlotUI : MonoBehaviour
{
    public Image icon;
    public string id;
    
    [Header("UI texts")]
    public TextMeshProUGUI weaponNameLv;
    public TextMeshProUGUI weaponDesc;
    public TextMeshProUGUI unlockExp;
    public TextMeshProUGUI upgradeExp;

    [Header("Buttons")]
    public GameObject unlockBtn;
    public GameObject upgradeBtn;
    public GameObject equipBtn;

    private WeaponModel _model;

    private void OnDisable()
    {
        if (_model != null) _model.OnChanged -= Refresh;
    }

    public void Bind(WeaponModel souce) 
    {
        // 이전 구독 있으면 취소
        if (_model != null)
            _model.OnChanged -= Refresh;

        _model = souce;

        // 활성 상태일 때만 구독
        if (isActiveAndEnabled)
            _model.OnChanged += Refresh;
        
        Refresh();
    }
    public void Refresh()
    {
        icon.sprite = _model.Data.icon;
        id = _model.Data.id;
        weaponNameLv.text = _model.Data.weaponName + " Lv." + _model.State.level.ToString();
        weaponDesc.text = "+ 공격력 " + _model.GetAttack().ToString() + "%\n" +
            "+ 치명타 확률 " + _model.GetCrit().ToString() + "%";
        unlockExp.text = _model.GetRequiredExp().ToString();
        upgradeExp.text = _model.GetRequiredExp().ToString();

        if (!_model.State.unlocked) // 무기 잠금 상태
        {
            unlockBtn.SetActive(true);
            upgradeBtn.SetActive(false);
            equipBtn.SetActive(false);
        }
        else if (GameManager.Instance.weaponRuntime.equippedId == _model.Data.id) // 무기 장착 상태
        {
            unlockBtn.SetActive(false);
            upgradeBtn.SetActive(true);
            equipBtn.SetActive(false);
        }
        else
        {
            unlockBtn.SetActive(false);
            upgradeBtn.SetActive(false);
            equipBtn.SetActive(true);
        }
    }

    // -----버튼 기능들----
    public void OnEquipBtn()
    {
        GameManager.Instance.weaponRuntime.TryEquip(id);
        Refresh();
    }
    public void OnUnlock()
    {
        if (_model.TryUnlock(GameManager.Instance.Exp, out int spent))
        {
            GameManager.Instance.Exp -= spent;
            Refresh();
        }
    }
    public void OnUpgrade()
    {
        if (_model.TryUpgrade(GameManager.Instance.Exp, out int spent))
        {
            GameManager.Instance.Exp -= spent;
            Refresh();
        }           
    }
}
