using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBagUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform parentTransform;

    private void OnEnable()
    {
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
