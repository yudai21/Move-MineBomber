using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        StartTimer(); // シーン開始時に自動で0からスタート
    }

    void Update()
    {
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

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}
