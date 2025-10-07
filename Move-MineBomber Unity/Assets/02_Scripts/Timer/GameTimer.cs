using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = $"Time: {elapsedTime:F2} 秒";
            }
        }
    }

    // 0から計測開始
    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    // 停止
    public void StopTimer()
    {
        isRunning = false;
    }

    // 一時停止
    public void PauseTimer()
    {
        isRunning = false;
    }

    // 再開
    public void ResumeTimer()
    {
        isRunning = true;
    }

    // 現在の時間を取得
    public float GetTime()
    {
        return elapsedTime;
    }
}

