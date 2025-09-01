using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    public PlayerData playerData;
    public GameObject warningPanel;
    public TextMeshPro warningText;

    public AudioSource bgmSource;
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    public AudioClip titleBGM;
    public AudioClip mainBGM;
    // Start is called before the first frame update
    void Start()
    {
        UpdateGoldUI();
        PlayBGM(titleBGM);
        volumeSlider.onValueChanged.AddListener(SetBGMVolume);
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
}
