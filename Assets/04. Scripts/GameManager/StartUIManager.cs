using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        // 새로 시작
        SceneManager.LoadScene("MainScene");
    }

    public void OnLoadGameClicked()
    {
        // 추후에 저장된 데이터 불러오고
        SceneManager.LoadScene("MainScene");
    }
}
