using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBagUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform parentTransform;

    private void Awake()
    {
        if (slotPrefab == null)
            slotPrefab = Resources.Load<GameObject>("Prefabs/WeaponSlot");

        if (parentTransform == null)
            parentTransform = transform;
    }

    private void OnEnable()
    {
        if (WeaponDatabase.Dict == null) WeaponDatabase.Init();
        
        // 중복 생성 방지
        for (int i = parentTransform.childCount; i > 0; i--)
            Destroy(parentTransform.GetChild(i).gameObject);
        
        // 슬롯 생성
        foreach (var data in WeaponDatabase.Dict)
        {
            GameObject go = Instantiate(slotPrefab, parentTransform);
            go.SetActive(false); // OnEnable 지연

            var ui = go.GetComponent<WeaponSlotUI>();
            var model = new WeaponModel(data.Key);
            ui.Bind(model);

            go.SetActive(true);
        }
    }
}
