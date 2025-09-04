using System.Linq;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class StageManager : MonoBehaviour
{
    [Header("Stage")]
    public int currentStage = 1;   // 처치할 때마다 +1
    public int killCount = 0;      // 누적 처치 수(표시용)

    [Header("UI (비어 있으면 자동 탐색)")]
    public TextMeshProUGUI stageText;      // "Stage X  •  Kills N"
    public TextMeshProUGUI enemyNameText;  // "귀여운/평범한/뚱뚱한 동그라미"

    [Header("Enemy")]
    public EnemyData[] enemyPool;
    public Transform spawnPoints;
    public GameObject enemyPrefab;

    [Header("Balance")]
    public int clickDamage = 1;
    public float hpScalePerStage = 1.2f;   // HP 테이블 없을 때만 사용

    [Header("Per-Spawn Random Scale & Name")]
    public Vector2 enemyScaleRange = new Vector2(0.8f, 1.6f);
    public float smallCutoff = 0.95f; // 이보다 작으면 "귀여운"
    public float bigCutoff = 1.25f; // 이 이상이면 "뚱뚱한"

    private EnemyModel _currentEnemyModel;
    public EnemyModel currentEnemyModel => _currentEnemyModel;

    void Awake()
    {
        AutoWireUI();  // 비어 있으면 자동으로 찾아 연결
    }

    void Start()
    {
        UpdateStageUI();
        SpawnEnemy();
    }

    // ---------- UI ----------
    void AutoWireUI()
    {
        // 이미 연결되어 있으면 패스
        if (stageText == null || enemyNameText == null)
        {
            var all = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);

            if (stageText == null)
            {
                stageText = all.FirstOrDefault(t =>
                    t.name.ToLower().Contains("stage"))    // 이름에 Stage 포함
                    ?? all.FirstOrDefault(t => t.text.Contains("Stage")) // 초기 텍스트가 Stage로 시작
                    ?? all.FirstOrDefault(t => t != null); // 아무거나 첫 번째
                if (stageText != null)
                    Debug.Log($"[UI] stageText auto-wired -> {GetPath(stageText.gameObject)}");
                else
                    Debug.LogWarning("[UI] stageText를 찾지 못했어요. 인스펙터에 드래그해서 연결해줘!");
            }

            if (enemyNameText == null)
            {
                enemyNameText = all
                    .Where(t => t != stageText) // stageText와 같은 객체는 제외
                    .FirstOrDefault(t =>
                        t.name.ToLower().Contains("name") || t.text.Contains("동그라미"))
                    ?? all.FirstOrDefault(t => t != stageText);
                if (enemyNameText != null)
                    Debug.Log($"[UI] enemyNameText auto-wired -> {GetPath(enemyNameText.gameObject)}");
                else
                    Debug.LogWarning("[UI] enemyNameText를 찾지 못했어요. 인스펙터에 드래그해서 연결해줘!");
            }
        }
    }

    string GetPath(GameObject go)
    {
        string path = go.name;
        Transform t = go.transform.parent;
        while (t != null)
        {
            path = t.name + "/" + path;
            t = t.parent;
        }
        return path;
    }

    void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage}  |  Kills {killCount}";
    }

    // ---------- Spawn ----------
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("StageManager: enemyPrefab 비어있음");
            return;
        }

        var candidates = enemyPool?.Where(e => e != null && e.appearStage <= currentStage).ToList();
        if (candidates == null || candidates.Count == 0)
        {
            Debug.LogWarning("StageManager: 등장 가능한 EnemyData 없음 (enemyPool/appearStage 확인)");
            return;
        }

        var selectedData = candidates[Random.Range(0, candidates.Count)];

        int fallbackHP = Mathf.Max(1,
            Mathf.RoundToInt(selectedData.maxHP * Mathf.Pow(hpScalePerStage, currentStage - 1)));

        int finalMaxHP = (selectedData.hpTable != null && selectedData.hpTable.Count > 0)
            ? selectedData.GetHPForStage(currentStage, fallbackHP)
            : fallbackHP;

        var runtimeData = ScriptableObject.CreateInstance<EnemyData>();
        runtimeData.enemyName = selectedData.enemyName;
        runtimeData.appearStage = selectedData.appearStage;
        runtimeData.icon = selectedData.icon;
        runtimeData.maxHP = finalMaxHP;

        Vector3 spawnPos = Vector3.zero;
        if (spawnPoints != null)
        {
            spawnPos = (spawnPoints.childCount > 0)
                ? spawnPoints.GetChild(Random.Range(0, spawnPoints.childCount)).position
                : spawnPoints.position;
        }

        var go = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        var sr = go.GetComponent<SpriteRenderer>() ?? go.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && runtimeData.icon != null) sr.sprite = runtimeData.icon;

        // 매번 랜덤 스케일
        float s = Random.Range(enemyScaleRange.x, enemyScaleRange.y);
        go.transform.localScale = Vector3.one * s;

        // 스케일에 따라 이름 변경
        string sizedName =
            (s < smallCutoff) ? "귀여운 동그라미" :
            (s >= bigCutoff) ? "뚱뚱한 동그라미" :
                                "평범한 동그라미";
        runtimeData.enemyName = sizedName;

        if (enemyNameText != null) enemyNameText.text = runtimeData.enemyName;

        var view = go.GetComponent<EnemyView>() ?? go.GetComponentInChildren<EnemyView>();
        if (view == null)
        {
            Debug.LogError("StageManager: enemyPrefab에 EnemyView 필요");
            Destroy(go);
            return;
        }

        _currentEnemyModel = new EnemyModel(runtimeData);
        view.Bind(_currentEnemyModel);
        view.SetClickDamage(clickDamage);

        _currentEnemyModel.OnDead += OnEnemyDead;

        Debug.Log($"[Spawn] Stage {currentStage}, Scale {s:F2}, Name {runtimeData.enemyName}");
        UpdateStageUI();
    }

    // ---------- Kill ----------
    void OnEnemyDead()
    {
        if (_currentEnemyModel != null)
            _currentEnemyModel.OnDead -= OnEnemyDead;

        killCount++;
        currentStage++; // 처치 즉시 스테이지 상승

        Debug.Log($"[Kill] -> Stage {currentStage}, Kills {killCount}");
        UpdateStageUI();
        SpawnEnemy();
    }

    void OnDisable()
    {
        if (_currentEnemyModel != null)
            _currentEnemyModel.OnDead -= OnEnemyDead;
    }
}