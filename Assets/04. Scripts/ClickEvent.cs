using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // 클릭 시 호출되는 공격 함수
    void HandleClickAttack()
    {
        // 공격 로직을 여기에 구현
        Debug.Log("공격!");
    }

    void Update()
    {
        // 마우스 왼쪽 버튼(터치)이 눌렸는지 확인
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 포인터가 UI 위에 있지 않은지 확인
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleClickAttack();
            }
        }
    }
}
