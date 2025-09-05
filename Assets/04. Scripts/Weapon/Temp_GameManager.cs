// Temp_GameManager.cs
// 테스트용 게임메니저입니다
using System;
using System.IO;
using UnityEngine;

public class Temp_GameManager : MonoBehaviour
{
    public WeaponRuntime weaponRuntime;

    private static Temp_GameManager instance = null;
    public static Temp_GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    [Header("Save Settings")]
    [SerializeField] private string fileName = "weaponsave.json";

    private string savePath => Path.Combine(Application.persistentDataPath, fileName);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


            weaponRuntime = new WeaponRuntime();
        weaponRuntime.Init();

        LoadAndApply();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        try
        {
            var save = weaponRuntime.ToSave(); // 필요한 데이터만 받기
            var json = JsonUtility.ToJson(save, prettyPrint: true);

            var dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            File.WriteAllText(savePath, json);
            Debug.Log($"[Temp_GameManager] Saved: {savePath}\n{json}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Temp_GameManager] Save 실패: {e}");
        }
    }

    public void LoadAndApply()
    {
        try
        {
            if (!File.Exists(savePath))
            {
                Debug.Log("[Temp_GameManager] 저장 파일이 없어 기본 상태로 시작합니다.");
                return;
            }

            var json = File.ReadAllText(savePath);
            var save = JsonUtility.FromJson<WeaponsSave>(json);
            if (save == null)
            {
                Debug.LogWarning("[Temp_GameManager] 저장 파일 파싱 실패. 기본 상태로 시작합니다.");
                return;
            }

            weaponRuntime.Apply(save);
            Debug.Log($"[Temp_GameManager] Loaded & Applied: {savePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Temp_GameManager] Load 실패: {e}");
        }
    }
}
