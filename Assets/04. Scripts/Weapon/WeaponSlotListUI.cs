using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotListUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform parentTransform;

    private List<GameObject> _slots;
    private WeaponRuntime _runtime;

    private void Awake()
    {
        if (slotPrefab == null)
            slotPrefab = Resources.Load<GameObject>("Prefabs/WeaponSlot");

        if (parentTransform == null)
            parentTransform = transform;

        _slots = new List<GameObject>();
    }

    private void OnEnable()
    {
        // runtime 연결
        _runtime = GameManager.Instance.weaponRuntime;

        // 데이터베이스 초기화
        if (WeaponDatabase.Dict == null)
            WeaponDatabase.Init();

        // 자식 오브젝트 제거: 중복 생성 방지용
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
            Destroy(parentTransform.GetChild(i).gameObject);

        // 슬롯 생성
        foreach (var data in _runtime.Models)
        {
            GameObject go = Instantiate(slotPrefab, parentTransform);
            go.SetActive(false); // OnEnable 지연
            
            WeaponSlotUI ui = go.GetComponent<WeaponSlotUI>();
            ui.Bind(data.Value);

            go.SetActive(true);
            _slots.Add(go);
        }

        // 이벤트 구독
        _runtime.OnEquippedChanged += RefreshAllSlots;
    }
    private void OnDisable()
    {
        if (_runtime != null)
            _runtime.OnEquippedChanged -= RefreshAllSlots;
    }
    private void RefreshAllSlots()
    {
        foreach (var slot in _slots)
            slot.GetComponent<WeaponSlotUI>().Refresh();
    }
}
