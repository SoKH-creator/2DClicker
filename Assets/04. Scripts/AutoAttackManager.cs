using System.Collections;
using UnityEngine;

public class AutoAttackManager : MonoBehaviour
{
    private Coroutine autoAttackCoroutine;
    private float attackInterval; // 공격 간격 (초)

    // 자동 공격 시작
    public void StartAutoAttack(int level)
    {
        // 이전 코루틴이 있다면 정지
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }

        // 레벨에 따라 공격 간격 설정
        attackInterval = 3.0f - (0.1f * level);
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
            // 자동 공격 로직을 여기에 구현
            Debug.Log("자동 공격!");

            // 설정된 간격만큼 대기
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
