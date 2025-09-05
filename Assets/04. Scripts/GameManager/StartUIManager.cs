using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        // 새로 시작 = 그냥 GameScene으로 이동
        SceneManager.LoadScene("ManagerText_lwh");
    }

    public void OnLoadGameClicked()
    {
        // 추후에 저장된 데이터 불러오고 → GameScene 이동
        SceneManager.LoadScene("ManagerText_lwh");
    }
}
