using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Ŭ�� �� ȣ��Ǵ� ���� �Լ�
    void HandleClickAttack()
    {
        // ���� ������ ���⿡ ����
        Debug.Log("����!");
    }

    void Update()
    {
        // ���콺 ���� ��ư(��ġ)�� ���ȴ��� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            // ���� �����Ͱ� UI ���� ���� ������ Ȯ��
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleClickAttack();
            }
        }
    }
}
