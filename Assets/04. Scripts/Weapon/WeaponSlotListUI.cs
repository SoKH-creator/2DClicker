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

        // 리스트 재생성
        if (_slots == null)
            _slots = new List<GameObject>();
        _slots.Clear();

        // 슬롯 생성
        foreach (var kv in _runtime.Models)
        {
            GameObject go = Instantiate(slotPrefab, parentTransform);
            go.SetActive(false); // OnEnable 지연
            
            WeaponSlotUI ui = go.GetComponent<WeaponSlotUI>();
            ui.Bind(kv.Value);

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
        if (_slots == null) return;

        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            GameObject go = _slots[i];

            // Destroy된 오브젝트 null로 판단
            if(!go)
            {
                _slots.RemoveAt(i);
                continue;
            }

            // ui 널 방어
            WeaponSlotUI ui = go.GetComponent<WeaponSlotUI>();
            if (!ui)
            {
                _slots.RemoveAt(i);
                continue;
            }

            ui.Refresh();
        }
    }
}
