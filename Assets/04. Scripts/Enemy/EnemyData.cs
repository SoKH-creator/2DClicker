using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StageHP { public int stage; public int maxHP; }

[CreateAssetMenu(menuName = "Game/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string enemyName;
    public int maxHP;
    public int appearStage;   // (선택) 스테이지 내 등장 기준이 필요하면 사용
    public Sprite icon;

    [Header("HP Table (Optional)")]
    public List<StageHP> hpTable = new();

    [Header("Spawn Rules (Cycle)")]
    public int appearPhase = 0;  // ★ 추가: 0=1~10, 1=11~20, 2=21~30 ... 같은 개념

    public int GetHPForStage(int stage, int fallback)
    {
        if (hpTable != null && hpTable.Count > 0)
        {
            for (int i = 0; i < hpTable.Count; i++)
                if (hpTable[i].stage == stage) return Mathf.Max(1, hpTable[i].maxHP);
        }
        return Mathf.Max(1, fallback);
    }
}