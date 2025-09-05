using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 메인 씬에서 UI 연결용
public class WeaponUI : MonoBehaviour
{
    public GameObject weaponBagUI;

    [Header("Equipped Weapon")]
    public Image icon;
    public TextMeshProUGUI equippedName;
    public TextMeshProUGUI equippedDescription;

    private string id;
    private WeaponRuntime _runtime;

    public void OnEnable()
    {
        StartCoroutine(WaitUntillReady());
    }

    IEnumerator WaitUntillReady()
    {
        // gamemanger, weaponRuntime 대기
        yield return new WaitUntil(
            () =>  GameManager.Instance != null && GameManager.Instance.weaponRuntime != null);

        _runtime = GameManager.Instance.weaponRuntime;
        
        _runtime.OnAnyModelChanged += Refresh;
        _runtime.OnEquippedChanged += OnEquippedChanged;

        Refresh(null);
    }
    public void Refresh(string id)
    {
        if (string.IsNullOrEmpty(id))
            id = _runtime.equippedId;

        if (string.IsNullOrEmpty(id) || !_runtime.Models.TryGetValue(id, out var model) || model == null)
            return;

        icon.sprite = model.Data.icon;
        equippedName.text = model.Data.weaponName + " Lv." + model.State.level;
        equippedDescription.text = "공격력: " + model.GetAttack().ToString()
            + "\n치명타 확률: " + model.GetCrit().ToString() + "%";
    }
    private void OnEquippedChanged() => Refresh(null);

    //----버튼 연결용----
    public void OnSelectBtn()
    {
        weaponBagUI.SetActive(true);
    }
    public void OnBackBtn()
    {
        weaponBagUI.SetActive(false);
    }
}
