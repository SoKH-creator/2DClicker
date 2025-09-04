using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 데이터 list
[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    private Dictionary<string, WeaponData> _dict;

    public void Init()
    {
        _dict = new Dictionary<string, WeaponData>();

        var weaponDatas = Resources.LoadAll<WeaponData>("WeaponData");

        foreach (var data in weaponDatas)
        {
            if (!_dict.ContainsKey(data.id))
                _dict.Add(data.id, data);
        }
    }

    public WeaponData GetWeaponData(string id)
    {
        if (_dict == null) Init();
        return _dict.TryGetValue(id, out WeaponData data) ? data : null;
    }
}
