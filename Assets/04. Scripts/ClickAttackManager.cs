using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAttackManager : MonoBehaviour
{
    // 임의의 클릭 공격력 변수. 인스펙터에서 설정.    
    [Header("클릭 공격 대미지 설정")]
    public float clickDamage = 10f; // 임의 변수명: 클릭 공격력

    // 현재 공격 대상이 될 몬스터. 인스펙터에서 드래그하여 연결
    // 몬스터를 생성하거나 바뀔 때마다 GameManager 등에서 이 변수를 업데이트
    [Header("공격 대상 몬스터 참조")]
    public MonoBehaviour currentTargetMonster;

    // 클릭 시 호출되는 공격 함수
    void ClickAttack()
    {
        // 공격 로직을 여기에 구현
        Debug.Log("공격!");
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            // UI를 클릭한 것이 아닌지 확인
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 공격 대상 몬스터가 존재할 경우
                if (currentTargetMonster != null)
                {
                    ClickAttack();
                    // 몬스터 스크립트의 "TakeDamage" 함수를 호출
                    // 몬스터 스크립트에 이 이름의 함수가 있다고 가정
                    // 만약 함수명이 다르다면 아래 문자열을 몬스터 스크립트의 함수명으로 변경
                    currentTargetMonster.Invoke("TakeDamage", clickDamage);
                }
                else
                {
                    Debug.Log("공격할 대상이 없습니다. 몬스터를 지정해주세요.");
                }
            }
        }
    }

    
    // 외부에서 공격 대상을 설정할 때 사용하는 함수.
    // 예를 들어, GameManager가 새 몬스터를 생성할 때 이 함수를 호출
    
    // <param name="newMonster">새로운 몬스터 오브젝트</param>
    public void SetTargetMonster(MonoBehaviour newMonster)
    {
        currentTargetMonster = newMonster;
    }

}