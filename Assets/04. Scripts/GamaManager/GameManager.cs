using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public int gold = 30;
    public int exp = 30;

    private static GameManager instance = null;

    //public Temp_WeaponModel weaponModel;
    public WeaponRuntime weaponRuntime;
    public PlayerData playerData;
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    public AudioSource bgmSource;
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    public AudioClip titleBGM;
    public AudioClip mainBGM;

    public FinalStats finalStats;
    public GameObject individualStatUI; // 프리팹 연결
    public Transform uiParent; // 어디에 붙일지 부모 오브젝트
    public StatHandler statHandler; // StatHandler 오브젝트
    public UpgradeData[] upgradeDatas; // 업그레이드 대상 정보들
    public static GameManager Instance { get { return instance; } }

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateGoldUI();
        PlayBGM(titleBGM);
        CalculateFinalStats();
        volumeSlider.onValueChanged.AddListener(SetBGMVolume);
        CreateUpgradeUI();
        weaponRuntime = new WeaponRuntime();
        weaponRuntime.Init();
    }
    
    public void UpdateGoldUI()
    {
        goldText.text = $"Gold : {playerData.gold}";
    }
    public void TryUseGold(int amount)
    {
        if (playerData.gold < amount)
        {
            StartCoroutine(ShowWarning("골드가 부족합니다!"));
        }
        else
        {
            playerData.gold -= amount;
            UpdateGoldUI();
        }
    }
    public void WarningClose()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }
    IEnumerator ShowWarning(string msg)
    {
        warningText.text = msg;
        warningPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningPanel.SetActive(false);
    }
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }
    public void CalculateFinalStats()
    {
        Debug.Log("호출됨");
        finalStats = new FinalStats();
        finalStats.Calculate(playerData, weaponRuntime);

        // 확인용 로그
        Debug.Log($"최종 공격력: {finalStats.finalAttack}");
        Debug.Log($"치명타 확률: {finalStats.finalCritChance}");
    }
    public void CreateUpgradeUI()
    {
        foreach (var data in upgradeDatas)
        {
            GameObject go = Instantiate(individualStatUI, uiParent);
            go.SetActive(true);
            UpgradeUI ui = go.GetComponent<UpgradeUI>();
            ui.statHandler = statHandler;
            ui.upgradeData = data;
        }
    }

    public void SaveGame()
    {
        // GameSave 구조와 연결 예정
    }

    public void LoadGame()
    {
        // 저장된 GameSave 불러오기 예정
    }
}
