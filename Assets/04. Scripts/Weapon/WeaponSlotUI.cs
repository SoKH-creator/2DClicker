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
    public TextMeshProUGUI exp;

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
        id = _model.Data.id;
        weaponNameLv.text = _model.Data.weaponName + " Lv." + _model.State.level.ToString();
        weaponDesc.text = "+ 공격력 " + _model.GetAttack().ToString() + "%\n" +
            "+ 치명타 확률 " + _model.GetCrit().ToString() + "%";
        exp.text = _model.GetRequiredExp().ToString();

        if (!_model.State.unlocked) // 무기 잠금 상태
        {
            unlockBtn.SetActive(true);
            upgradeBtn.SetActive(false);
            equipBtn.SetActive(false);
        }
        else if (Temp_GameManager.Instance.weaponRuntime.equippedId == _model.Data.id) // 무기 장착 상태
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
    private void OnEquipBtn()
    {
        Temp_GameManager.Instance.weaponRuntime.TryEquip(id);
    }
    private void OnUnlock()
    {
        if (_model.TryUnlock(ref exp)) Refresh();
    }
    private void OnUpgrade()
    {
        if (_model.TryUnlock(ref exp)) Refresh();
    }

}
