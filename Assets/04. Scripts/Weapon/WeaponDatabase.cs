using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 데이터 list
public static class WeaponDatabase
{
    private static Dictionary<string, WeaponData> _dict;

    public static Dictionary<string, WeaponData> Dict { get { return _dict; } }

    public static void Init()
    {
        if (_dict != null) return;

        _dict = new Dictionary<string, WeaponData>();
        
        var weaponDatas = Resources.LoadAll<WeaponData>("WeaponData");
        foreach (var data in weaponDatas)
            _dict[data.id] = data;
    }

    public static WeaponData GetWeaponData(string id)
    {
        return _dict.TryGetValue(id, out WeaponData data) ? data : null;
    }
}
