using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance { get; private set; }

    [SerializeField] GameTimer gameTimer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたぐなら残す
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopTimer()
    {
        gameTimer.StopTimer();
    }

    public void PauseTimer()
    {
        gameTimer.PauseTimer();
    }

    public void ResumeTimer()
    {
        gameTimer.ResumeTimer();
    }

    public float GetTime()
    {
        return gameTimer.GetTime();
    }
}

