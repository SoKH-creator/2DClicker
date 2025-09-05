using System.Collections;
using UnityEngine;

public class AutoAttackManager : MonoBehaviour
{
    private Coroutine autoAttackCoroutine;

    // GameManager와 StageManager 스크립트를 참조.
    public GameManager gameManager;
    public StageManager stageManager;

    [Header("자동 공격 레벨")]
    public int autoAttackSkillLevel = 1; // 자동 공격 레벨

    public AttackEffectManager effectManager;

    void Awake()
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        if (stageManager == null) stageManager = FindObjectOfType<StageManager>();
        if (effectManager == null) effectManager = FindObjectOfType<AttackEffectManager>();

        // 게임 시작 시 자동 공격을 시작합.
        StartAutoAttack();
    }

    // 자동 공격을 시작하거나 레벨을 업데이트할 때 호출합니다.
    public void StartAutoAttack()
    {
        // 기존 코루틴이 있다면 정지.
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }

        // 레벨에 따라 공격 간격을 설정.
        float attackInterval = 1.0f - (0.1f * autoAttackSkillLevel);
        if (attackInterval < 0.1f) attackInterval = 0.1f; // 최소 간격 제한

        // 새로운 코루틴을 시작합니다.
        autoAttackCoroutine = StartCoroutine(AutoAttackRoutine(attackInterval));
    }

    // 자동 공격을 정지할 때 호출. (예: UI 창 진입 시)
    public void StopAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
            autoAttackCoroutine = null;
        }
    }

    // 자동 공격을 수행하는 코루틴
    private IEnumerator AutoAttackRoutine(float interval)
    {
        while (true)
        {
            // 공격할 대상(현재 몬스터)이 있는지 확인.
            if (stageManager != null && stageManager.currentEnemyModel != null)
            {
                // GameManager의 최종 공격력을 가져옴.
                float finalDamage = gameManager.finalStats.finalAttack;

                // 몬스터의 TakeDamage 함수에 최종 공격력을 전달.
                // 몬스터 스크립트는 정수형(int) 대미지를 받으므로 형변환.
                stageManager.currentEnemyModel.TakeDamage(Mathf.RoundToInt(finalDamage));


                // 몬스터 위치에 파티클 생성
                if (effectManager != null)
                {
                    effectManager.SpawnAttackParticle(stageManager.currentEnemyView.transform.position);
                }
                Debug.Log($"자동 공격! 몬스터에게 {finalDamage}만큼의 피해를 입혔습니다.");
            }


            // 설정된 간격만큼 대기.
            yield return new WaitForSeconds(interval);
        }
    }
}