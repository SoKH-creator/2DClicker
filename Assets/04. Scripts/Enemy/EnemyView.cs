using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class EnemyView : MonoBehaviour
{
    [Header("UI Components")]
    public Image hpBar;
    public TextMeshProUGUI hpText;

    private EnemyModel model;

    public void Bind(EnemyModel model)
    {
        if(this.model != null)
        {
            this.model.OnDamaged -= UpdateUI;
            this.model.OnDead -= Die;
        }

        this.model = model;

        model.OnDamaged += UpdateUI;
        model.OnDead += Die;

        UpdateUI();
    }

    private void OnMouseDown()
    {
        if (model != null)
            return;
        model.TakeDamage(10);

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
        Debug.Log("Enemy Died");
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

    public void TakeDamege(int amount) // 외부에서 버튼같은걸로 때리는 메서드
    {
        if (model != null)
            return;
        model.TakeDamage(amount);
    }
}
