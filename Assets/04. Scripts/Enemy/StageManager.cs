using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Stage")]
    public int currentStage = 1;
    public int killCount = 0;
    public int killTargetCount = 10; // 스테이지 당 처치해야 하는 적 수

    [Header("UI")]
    public TextMeshProUGUI stageText;

    [Header("Enemy")]
    public List<EnemyData> enemyPool;
    public Transform spawnPoints;
    public GameObject enemyPrefab;

    [Header("Balance")]
    public int clickDamage = 1;
    public float hpScalePerStage = 1.2f; // 스테이지 당 적 체력 증가 배수

    // 현재 소환된 적 모델(단일 적 기준)
    private EnemyModel _currentEnemyModel;

    void Start()
    {
        UpdateStageUI();
        SpawnEnemy();
    }

    void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage}/{killTargetCount}";
    }

    /// 적 소환 함수
    /// - enemyPool에서 현재 스테이지에 등장 가능한 후보를 골라 랜덤 선택
    /// - spawnPoints가 null이면 Vector3.zero, 있으면 spawnPoints의 임의 자식 위치 혹은 spawnPoints.position 사용
    /// - runtime에 체력을 스케일링한 임시 EnemyData를 만들어 EnemyModel에 넣어 바인딩
    void SpawnEnemy()
    {
        // 후보 필터링
        var candidates = enemyPool
            .Where(e => e != null && e.appearStage <= currentStage)
            .ToList();

        if (candidates.Count == 0)
        {
            Debug.LogWarning("StageManager: 등장 가능한 EnemyData가 없습니다. enemyPool과 appearStage를 확인하세요.");
            return;
        }

        // 랜덤으로 하나 선택
        var selectedData = candidates[Random.Range(0, candidates.Count)];

        // 스케일된 체력 계산
        int scaledMaxHP = Mathf.Max(1, Mathf.RoundToInt(selectedData.maxHP * Mathf.Pow(hpScalePerStage, currentStage - 1)));

        var runtimeData = ScriptableObject.CreateInstance<EnemyData>();
        runtimeData.enemyName = selectedData.enemyName;
        runtimeData.appearStage = selectedData.appearStage;
        runtimeData.icon = selectedData.icon;
        runtimeData.maxHP = scaledMaxHP;

        // 소환 위치 결정
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoints != null)
        {
            if (spawnPoints.childCount > 0)
            {
                // 자식이 있으면 임의의 자식 위치 사용
                var child = spawnPoints.GetChild(Random.Range(0, spawnPoints.childCount));
                spawnPos = child.position;
            }
            else
            {
                // 자식이 없으면 부모의 위치 사용
                spawnPos = spawnPoints.position;
            }
        }

        // 프리팹 인스턴스화
        var go = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // 스프라이트 교체
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null && runtimeData.icon != null)
        {
            sr.sprite = runtimeData.icon;
        }
        else
        {
            var srChild = go.GetComponentInChildren<SpriteRenderer>();
            if (srChild != null && runtimeData.icon != null)
                srChild.sprite = runtimeData.icon;
        }

        // EnemyView 찾기
        var view = go.GetComponent<EnemyView>() ?? go.GetComponentInChildren<EnemyView>();
        if (view == null)
        {
            Debug.LogError("StageManager: enemyPrefab에 EnemyView 컴포넌트가 필요합니다.");
            Destroy(go);
            return;
        }

        // (선택) EnemyView에 클릭 데미지 값 직접 제공하려면 EnemyView에 public setter 추가 필요
        // 예: view.SetClickDamage(clickDamage);

        // 모델 생성 및 바인딩
        _currentEnemyModel = new EnemyModel(runtimeData);
        view.Bind(_currentEnemyModel);

        // 적이 죽었을 때 호출될 콜백 등록
        _currentEnemyModel.OnDead += OnEnemyDead;
    }

    // 적 사망 처리: 킬 카운트 증가, 스테이지 클리어 체크, 다음 적 소환
    void OnEnemyDead()
    {
        if (_currentEnemyModel != null)
            _currentEnemyModel.OnDead -= OnEnemyDead;

        killCount++;

        if (killCount >= killTargetCount)
        {
            currentStage++;
            killCount = 0;
            UpdateStageUI();
            // 필요하면 스테이지 업 이벤트 / 보상 창 띄우기 등 여기에 추가
        }

        // 다음 적 소환
        SpawnEnemy();
    }
}