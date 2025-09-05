using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Gold and Exp")]
    public TextMeshProUGUI expText;
    public TextMeshProUGUI goldText;
    [SerializeField] private int gold = 30;
    [SerializeField] private int exp = 30;

    public int Gold 
    {
        get { return gold; }
        set 
        { 
            gold = value;
            OnGoldChanged.Invoke();
        }
    }
    public int Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            OnExpChanged.Invoke();
        }
    }

    [Header("Warning Panel")]
    public WeaponRuntime weaponRuntime;
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public AudioClip titleBGM;
    public AudioClip mainBGM;

    [Header("MonoB")]
    public FinalStats finalStats;
    public StatHandler statHandler; // StatHandler 오브젝트

    public event Action OnGoldChanged;
    public event Action OnExpChanged;
    
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

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
        weaponRuntime = new WeaponRuntime();
        weaponRuntime.Init();
        finalStats.Init();

        UpdateGoldUI();
        UpdateExpUI();

        PlayBGM(titleBGM);

        CalculateFinalStats();

        volumeSlider.onValueChanged.AddListener(SetBGMVolume);
    }
    
    public void UpdateGoldUI()
    {
        goldText.text = $"{gold}";
    }
    public void TryUseGold(int amount)
    {
        if (gold < amount)
        {
            StartCoroutine(ShowWarning("골드가 부족합니다!"));
        }
        else
        {
            gold -= amount;
            UpdateGoldUI();
        }
    }
    public void UpdateExpUI()
    {
        expText.text = $"{exp}";
    }
    public void TryUseExp(int amount)
    {
        if (exp < amount)
        {
            StartCoroutine(ShowWarning("경험치가 부족합니다!"));
        }
        else
        {
            exp -= amount;
            UpdateExpUI();
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
        float min = Mathf.Pow(10f, -80f / 20f);
        float max = Mathf.Pow(10f, 6f / 20f);
        float amp = Mathf.Lerp(min, max, value);
        float dB = Mathf.Log10(amp) * 20f;

        audioMixer.SetFloat("Master", dB);
    }
    public void CalculateFinalStats()
    {
        //Debug.Log("호출됨");
        finalStats.Calculate(statHandler, weaponRuntime);

        // 확인용 로그
        Debug.Log($"최종 공격력: {finalStats.finalAttack}");
        Debug.Log($"치명타 확률: {finalStats.finalCritChance}");
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
