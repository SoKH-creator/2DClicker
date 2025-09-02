using System.Collections;
using UnityEngine;

public class AutoAttackManager : MonoBehaviour
{
    private Coroutine autoAttackCoroutine;
    private float attackInterval; // 공격 간격 (초)
    

    // 공격력과 관련된 임의 변수
    // 공격력을 받아올 변수명으로 변경
    [Header("공격력 참조")]
    //public PlayerStats playerStats; // 임의 변수: 플레이어의 능력치를 관리하는 스크립트 참조
    public float autoAttackBaseDamage; // 임의 변수: 자동 공격의 기본 공격력

    // 공격 간격을 받아올 임의 변수
    [Header("공격 간격 참조")]
    public float autoAttackSkillLevel; // 임의 변수: 자동 공격 스킬 레벨


    // 자동 공격 시작
    public void StartAutoAttack(int level)
    {
        // 이전 코루틴이 있다면 정지
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }

        // 레벨에 따라 공격 간격 설정
        attackInterval = 10.0f - (0.1f * level);
        if (attackInterval < 0.1f) attackInterval = 0.1f; // 최소 간격 제한

        // 새로운 코루틴 시작
        autoAttackCoroutine = StartCoroutine(AutoAttackRoutine());
    }

    // 자동 공격 정지
    public void StopAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
            autoAttackCoroutine = null; // 코루틴 변수 초기화
        }
    }

    // 자동 공격 코루틴
    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            // 여기에 자동 공격 로직을 구현할 예정
            // 베이스 공격력과 스킬 레벨을 이용한 최종 공격력 계산
            // 임의 변수명을 사용
            float finalDamage = autoAttackBaseDamage * (1 + (autoAttackSkillLevel * 0.1f));

            // 설정된 간격만큼 대기
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
