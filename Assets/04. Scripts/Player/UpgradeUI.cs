using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public StatHandler statHandler;
    public UpgradeData upgradeData;

    [Header("UI References")]
    public TextMeshProUGUI titleText;           // 업그레이드 이름
    public TextMeshProUGUI descriptionText;     // 효과 설명
    public TextMeshProUGUI levelText;           // 현재 업그레이드 레벨
    public TextMeshProUGUI costText;            // 업그레이드 비용 
    public Button upgradeButton;              // 업그레이드 버튼
    private void Start()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        UpdateUI();

        // 골드 변경 이벤트 구독
        if (statHandler != null)
        {
            statHandler.OnGoldChanged += UpdateUI;
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        if (statHandler != null)
        {
            statHandler.OnGoldChanged -= UpdateUI;
        }
    }

    public void OnUpgradeButtonClicked()
    {
        if (statHandler.TryUpgrade(upgradeData))
        {
            UpdateUI();
        }
        else
        {
            UpdateUI(); // 실패해도 비용 색상 갱신 필요
        }
    }

    private void UpdateUI()
    {
        int currentLevel = statHandler.GetLevel(upgradeData.statType);

        titleText.text = upgradeData.upgradeName;
        descriptionText.text = $"▷ {upgradeData.GetValueAtLevel(currentLevel)}";
        levelText.text = $"Lv {currentLevel}";
        costText.text = $"Cost: {upgradeData.GetCostAtLevel(currentLevel)}";

        // 비용 색상 (골드 부족시 빨간색)
        if (statHandler.gold < upgradeData.GetCostAtLevel(currentLevel))
            costText.color = Color.red;
        else
            costText.color = Color.black;
    }
}
