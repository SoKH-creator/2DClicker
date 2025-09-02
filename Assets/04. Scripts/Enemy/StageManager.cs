using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage = 1;
    public int killCount = 0;
    public int killTargetCount = 10;

    public TextMeshProUGUI stageText;
    public List<EnemyData> enemyPool;
    public Transform spawnPoints;
    public GameObject enemyPrefab;

    void Start()
    {
        UpdateStageUI();
        SpawnEnemy();
    }

    void SpawnEnemy()
    {

    }

    void UpdateStageUI()
    {
        stageText.text = $"Stage {currentStage}";
    }
}
