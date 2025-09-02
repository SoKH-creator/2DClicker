using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRuntime : MonoBehaviour
{
    private Dictionary<string, WeaponActor> _actors;

    [Header("Equipped Weapon ID")]
    public string _equippedID;

    public void Init(IEnumerable<WeaponData> catalog) { }
    public void Apply(WeaponsSave save) { }
    public WeaponsSave ToSave() { return null; }
    public bool TryEquip(string id) { return true; }
}
