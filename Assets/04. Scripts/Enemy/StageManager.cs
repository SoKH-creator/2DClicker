using System.Linq;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StageManager : MonoBehaviour
{
    [Header("Stage / Cycle")]
    public int currentStage = 1;
    public int maxStagePerCycle = 10;
    public int phaseIndex = 0;
    public int killCount = 0;

    [Header("UI")]
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI enemyNameText;

    [Header("Stage UI Appear (CanvasGroup)")]
    public CanvasGroup stageUI;        // ← infoGroup → stageUI 로 변경
    public float showDelay = 0.15f;
    public float fadeDuration = 0.25f;
    public bool hideBetweenSpawns = true;

    [Header("Enemy")]
    public EnemyData[] enemyPool;
    public Transform spawnPoints;
    public GameObject enemyPrefab;
    public Transform enemiesParent;

    [Header("Balance")]
    public int clickDamage = 1;
    public float hpScalePerStage = 1.2f;

    [Header("Per-Spawn Random Scale & Name")]
    public Vector2 enemyScaleRange = new Vector2(0.8f, 1.6f);
    public float smallCutoff = 0.95f;
    public float bigCutoff = 1.25f;

    public EnemyModel _currentEnemyModel;
    public EnemyModel currentEnemyModel => _currentEnemyModel;

    public EnemyView _currentEnemyView;
    public EnemyView currentEnemyView => _currentEnemyView;

    void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("StageManager: enemyPrefab이 비어 있습니다. Project 프리팹 에셋을 연결하세요.");
        }
        else if (enemyPrefab.scene.IsValid())
        {
            Debug.LogError("StageManager: enemyPrefab에 씬 오브젝트가 연결됨. Project 프리팹 에셋으로 교체하세요.");
        }
    }

    void Start()
    {
        // 시작 시 UI 감추기
        if (stageUI != null) SetStageUIVisible(false, instant: true);
        UpdateStageUI();
        SpawnEnemy();
    }

    void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage} / {maxStagePerCycle}";
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // 안전 가드: 풀 비었으면 종료
        if (enemyPool == null || enemyPool.Length == 0)
        {
            Debug.LogError("StageManager: enemyPool이 비었습니다.");
            return;
        }

        // 후보 찾기 (phase 우선 → 완화 → 전체)
        var candidates = enemyPool.Where(e => e != null && e.appearPhase == phaseIndex).ToList();
        if (candidates.Count == 0) candidates = enemyPool.Where(e => e != null && e.appearPhase <= phaseIndex).ToList();
        if (candidates.Count == 0) candidates = enemyPool.Where(e => e != null).ToList();
        if (candidates.Count == 0)
        {
            Debug.LogError("StageManager: 유효한 EnemyData가 없습니다.");
            return;
        }

        var selectedData = candidates[UnityEngine.Random.Range(0, candidates.Count)];

        int globalStageIndex = phaseIndex * maxStagePerCycle + currentStage;
        int fallbackHP = Mathf.Max(1,
            Mathf.RoundToInt(selectedData.maxHP * Mathf.Pow(hpScalePerStage, globalStageIndex - 1)));

        int finalMaxHP = (selectedData.hpTable != null && selectedData.hpTable.Count > 0)
            ? selectedData.GetHPForStage(currentStage, fallbackHP)
            : fallbackHP;

        // 런타임 EnemyData
        var runtimeData = ScriptableObject.CreateInstance<EnemyData>();
        runtimeData.enemyName = selectedData.enemyName;
        runtimeData.appearStage = selectedData.appearStage;
        runtimeData.icon = selectedData.icon;
        runtimeData.maxHP = finalMaxHP;
        runtimeData.appearPhase = selectedData.appearPhase;

        // 스폰 위치
        Vector3 pos = spawnPoints
            ? (spawnPoints.childCount > 0
                ? spawnPoints.GetChild(UnityEngine.Random.Range(0, spawnPoints.childCount)).position
                : spawnPoints.position)
            : Vector3.zero;

        var go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        if (enemiesParent) go.transform.SetParent(enemiesParent, true);

        // 스프라이트
        var sr = go.GetComponent<SpriteRenderer>() ?? go.GetComponentInChildren<SpriteRenderer>();
        if (sr && runtimeData.icon) sr.sprite = runtimeData.icon;

        // 랜덤 스케일
        float s = UnityEngine.Random.Range(enemyScaleRange.x, enemyScaleRange.y);
        go.transform.localScale = Vector3.one * s;

        // 이름 덮어쓰기
        string sizedName = (s < smallCutoff) ? "귀여운 동그라미"
                         : (s >= bigCutoff) ? "뚱뚱한 동그라미"
                                             : "평범한 동그라미";
        runtimeData.enemyName = sizedName;

        // UI 텍스트 최신화
        if (enemyNameText) enemyNameText.text = runtimeData.enemyName;
        UpdateStageUI();

        // EnemyView
        var view = go.GetComponent<EnemyView>() ?? go.GetComponentInChildren<EnemyView>();
        if (view == null)
        {
            Debug.LogError("StageManager: enemyPrefab에 EnemyView 컴포넌트가 필요합니다.");
            Destroy(go);
            return;
        }

        _currentEnemyModel = new EnemyModel(runtimeData);
        _currentEnemyView = view;
        view.Bind(_currentEnemyModel);
        // view.SetClickDamage(clickDamage); // 클릭커 스크립트로 넘길 예정

        _currentEnemyModel.OnDead += OnEnemyDead;

        // 스폰될 때만 StageUI를 부드럽게 보여주기
        if (stageUI != null)
        {
            SetStageUIVisible(false, instant: true); // 먼저 숨기고
            StartCoroutine(ShowStageUIRoutine());
        }
    }

    void OnEnemyDead()
    {
        if (_currentEnemyModel != null) _currentEnemyModel.OnDead -= OnEnemyDead;

        // --- 보상 지급 ---
        var data = _currentEnemyModel?.data;
        if (data != null && GameManager.Instance != null)
        {
            // 골드
            int gMin = Mathf.Max(0, data.goldRange.x);
            int gMax = Mathf.Max(gMin, data.goldRange.y);
            int goldGain = UnityEngine.Random.Range(gMin, gMax + 1);
            GameManager.Instance.gold += goldGain;
            GameManager.Instance.UpdateGoldUI();

            // 무기강화 EXP
            if (data.enhanceExp > 0)
            {
                GameManager.Instance.exp += data.enhanceExp;
                // GameManager.Instance.UpdateExpUI(); // 만들었으면 호출
            }
        }
        // -----------------------------------------------------

        killCount++;
        currentStage++;

        if (currentStage > maxStagePerCycle)
        {
            currentStage = 1;
            phaseIndex++;
        }

        if (hideBetweenSpawns && stageUI != null)
            SetStageUIVisible(false, instant: false);

        UpdateStageUI();
        SpawnEnemy();
    }

    void OnDisable()
    {
        if (_currentEnemyModel != null) _currentEnemyModel.OnDead -= OnEnemyDead;
    }

    // ---------- Stage UI 페이드 유틸 ----------
    void SetStageUIVisible(bool visible, bool instant = false)
    {
        if (stageUI == null) return;

        stageUI.interactable = visible;
        stageUI.blocksRaycasts = visible;

        if (instant)
        {
            stageUI.alpha = visible ? 1f : 0f;
        }
        else
        {
            StopCoroutine(nameof(FadeStageUI));
            StartCoroutine(FadeStageUI(visible ? 1f : 0f, fadeDuration));
        }
    }

    IEnumerator ShowStageUIRoutine()
    {
        if (showDelay > 0f) yield return new WaitForSeconds(showDelay);
        SetStageUIVisible(true, instant: false);
    }

    IEnumerator FadeStageUI(float target, float duration)
    {
        float start = stageUI.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            stageUI.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        stageUI.alpha = target;
    }
}