using System.Collections;
using UnityEngine;

public class AutoAttackManager : MonoBehaviour
{
    private Coroutine autoAttackCoroutine;
    private float attackInterval = 1.0f; // ���� ���� (��)

    // �ڵ� ���� ����
    public void StartAutoAttack(int level)
    {
        // ���� �ڷ�ƾ�� �ִٸ� ����
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }

        // ������ ���� ���� ���� ����
        attackInterval = 3.0f - (0.1f * level);
        if (attackInterval < 0.1f) attackInterval = 0.1f; // �ּ� ���� ����

        // ���ο� �ڷ�ƾ ����
        autoAttackCoroutine = StartCoroutine(AutoAttackRoutine());
    }

    // �ڵ� ���� ����
    public void StopAutoAttack()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
            autoAttackCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
        }
    }

    // �ڵ� ���� �ڷ�ƾ
    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            // �ڵ� ���� ������ ���⿡ ����
            Debug.Log("�ڵ� ����!");

            // ������ ���ݸ�ŭ ���
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
