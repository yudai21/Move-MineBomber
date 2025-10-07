using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToWin : MonoBehaviour
{
    private int clickCount = 0; // クリック回数を管理

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enterキー検知
        {
            clickCount++;
            if (clickCount >= 2)
            {
                GameClear();
            }
        }
    }

    void GameClear()
    {
        Debug.Log("ゲームクリア！"); // ゲームクリア処理
        GameTimerManager.Instance.StopTimer(); // タイマーを停止
        SceneManager.LoadScene("TestClearScene");
    }
}
