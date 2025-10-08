using UnityEngine;

/// <summary>
/// タイマーの値を保持するシングルトン
/// </summary>
public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance { get; private set; }
    private float currentTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたぐなら残す
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void SetCurrentTime(float Time)
    {
        currentTime = Time;
    }

    public void ResetCurrentTime()
    {
        currentTime = 0;
    }
}

