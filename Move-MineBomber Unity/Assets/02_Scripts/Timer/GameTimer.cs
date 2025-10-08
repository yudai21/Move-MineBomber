using UnityEngine;
using TMPro;

/// <summary>
/// 時間を計測し、テキストに反映させる
/// </summary>
public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;
    public bool IsRunning // 基本使用しない
    {
        get { return isRunning; }
        set { isRunning = value; }
    }

    void Start()
    {
        isRunning = false;

        // 計測した時間のリセット
        if (GameTimerManager.Instance.GetCurrentTime() != 0)
        {
            GameTimerManager.Instance.ResetCurrentTime();
        }
        
        ResetTimer(); // シーン開始時に自動で0からスタート
    }

    void Update()
    {
        GameStateCheck();

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            if (timerText != null)
            {
                int seconds = Mathf.FloorToInt(elapsedTime);
                timerText.text = seconds.ToString();
            }
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    private void GameStateCheck()
    {
        // ゲームの状態に応じてタイマー動作切り替え
        if (GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            isRunning = true;
        }
        else if (GameManager.Instance.CurrentGameState == GameState.GameClear)
        {
            isRunning = false;
            GameTimerManager.Instance.SetCurrentTime(elapsedTime); // 計測した時間を保存
        }
        else
        {
            isRunning = false;
        }
    }


}
