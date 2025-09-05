using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class EnemyView : MonoBehaviour
{
    [Header("UI Components")]
    public Image hpBar;
    public TextMeshProUGUI hpText;

    [Header("Damage")]
    [SerializeField] private int clickDamage = 1;

    public EnemyModel model;

    public void Bind(EnemyModel model)
    {
        if (this.model != null)
        {
            this.model.OnDamaged -= UpdateUI;
            this.model.OnDead -= Die;
        }

        this.model = model;

        model.OnDamaged += UpdateUI;
        model.OnDead += Die;

        UpdateUI();
    }

    // StageManager에서 주입
    public void SetClickDamage(int dmg)
    {
        clickDamage = Mathf.Max(1, dmg);
    }

    private void OnMouseDown()
    {
        // 버그 수정: model이 없으면 리턴(== null 체크)
        if (model == null) return;
        model.TakeDamage(clickDamage);
    }

    private void UpdateUI()
    {
        if (model == null) return;

        if (hpBar != null)
        {
            float percent = (float)model.currentHP / model.data.maxHP;
            hpBar.fillAmount = percent;
        }

        if (hpText != null)
        {
            hpText.text = $"{model.currentHP} / {model.data.maxHP}";
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnDamaged -= UpdateUI;
            model.OnDead -= Die;
        }
    }

    // 외부에서 버튼 액션으로 호출 가능
    public void TakeDamage(int amount)
    {
        if (model == null) return; 
        model.TakeDamage(Mathf.Max(1, amount));
    }
}