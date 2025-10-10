using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイマー動作切り替えテスト
/// </summary>
public class ClickToWin : MonoBehaviour
{
    [SerializeField] private GameTimer _gameTimer;
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
        //Debug.Log("ゲームクリア！"); // ゲームクリア処理
        _gameTimer.IsRunning = false;
        SceneManager.LoadScene("TestClearScene");
    }
}
