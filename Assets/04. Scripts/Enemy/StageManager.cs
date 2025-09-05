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

    [Header("UI Group Appear")]          
    public CanvasGroup infoGroup;         
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
        if (infoGroup != null) SetGroupVisible(false, instant: true);

        UpdateStageUI();
        SpawnEnemy();
    }

    void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage}";
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // 후보 찾기
        var candidates = enemyPool.Where(e => e != null && e.appearPhase == phaseIndex).ToList();
        if (candidates.Count == 0) candidates = enemyPool.Where(e => e != null && e.appearPhase <= phaseIndex).ToList();
        if (candidates.Count == 0) candidates = enemyPool.Where(e => e != null).ToList();

        var selectedData = candidates[Random.Range(0, candidates.Count)];

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
                ? spawnPoints.GetChild(Random.Range(0, spawnPoints.childCount)).position
                : spawnPoints.position)
            : Vector3.zero;

        var go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        if (enemiesParent) go.transform.SetParent(enemiesParent, true);

        // 스프라이트
        var sr = go.GetComponent<SpriteRenderer>() ?? go.GetComponentInChildren<SpriteRenderer>();
        if (sr && runtimeData.icon) sr.sprite = runtimeData.icon;

        // 랜덤 스케일
        float s = Random.Range(enemyScaleRange.x, enemyScaleRange.y);
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

        // 스폰될 때만 InfoGroup을 부드럽게 보여주기
        if (infoGroup != null)
        {
            SetGroupVisible(false, instant: true); // 먼저 숨기고
            StartCoroutine(ShowInfoRoutine());
        }
    }

    void OnEnemyDead()
    {
        if (_currentEnemyModel != null) _currentEnemyModel.OnDead -= OnEnemyDead;

        killCount++;
        currentStage++;

        if (currentStage > maxStagePerCycle)
        {
            currentStage = 1;
            phaseIndex++;
        }

        // 적이 죽으면 잠깐 숨기기(다음 스폰에서 다시 나타남)
        if (hideBetweenSpawns && infoGroup != null)
            SetGroupVisible(false, instant: false);

        UpdateStageUI();
        SpawnEnemy();
    }

    void OnDisable()
    {
        if (_currentEnemyModel != null) _currentEnemyModel.OnDead -= OnEnemyDead;
    }

    // ---------- UI 페이드 유틸 ----------

    void SetGroupVisible(bool visible, bool instant = false)
    {
        if (infoGroup == null) return;

        infoGroup.interactable = visible;
        infoGroup.blocksRaycasts = visible;

        if (instant)
        {
            infoGroup.alpha = visible ? 1f : 0f;
        }
        else
        {
            StopCoroutine(nameof(FadeGroup));
            StartCoroutine(FadeGroup(visible ? 1f : 0f, fadeDuration));
        }
    }

    IEnumerator ShowInfoRoutine()
    {
        if (showDelay > 0f) yield return new WaitForSeconds(showDelay);
        SetGroupVisible(true, instant: false);
    }

    IEnumerator FadeGroup(float target, float duration)
    {
        float start = infoGroup.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            infoGroup.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        infoGroup.alpha = target;
    }
}