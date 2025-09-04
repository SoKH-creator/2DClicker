using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickAttackManager : MonoBehaviour
{
    // GameManager와 StageManager 스크립트를 참조
    // 유니티 인스펙터에서 드래그하여 연결하거나, Start()에서 FindObjectOfType<>()을 사용해 참조.
    public GameManager gameManager;
    public StageManager stageManager;

    void Start()
    {
        // 씬에서 GameManager와 StageManager를 찾아 자동으로 연결.        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (stageManager == null)
        {
            stageManager = FindObjectOfType<StageManager>();
        }
    }

    void Update()
    {
        // 마우스 왼쪽 버튼(터치)이 눌렸는지 확인
        if (Input.GetMouseButtonDown(0))
        {
            // UI를 누른 상태에서는 공격이 진행되지 않도록 함.
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                AttackMonster();
            }
        }
    }

    // 몬스터를 공격하는 로직
    public void AttackMonster()
    {
        // 공격할 대상(현재 몬스터)이 있는지 확인.
        if (stageManager != null && stageManager.currentEnemyModel != null)
        {
            // GameManager의 최종 공격력을 가져옴.
            float finalDamage = gameManager.finalStats.finalAttack;

            // 몬스터의 TakeDamage 함수에 최종 공격력을 전달.
            // 몬스터 스크립트(EnemyModel)는 정수형(int) 대미지를 받으므로 형변환.
            stageManager.currentEnemyModel.TakeDamage(Mathf.RoundToInt(finalDamage));

            Debug.Log($"클릭! 몬스터에게 {finalDamage}만큼의 피해를 입혔습니다.");
        }
        else
        {
            Debug.Log("공격할 몬스터가 없습니다.");
        }
    }
}