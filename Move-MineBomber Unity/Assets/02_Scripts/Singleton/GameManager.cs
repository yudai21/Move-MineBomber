using System;
using UnityEngine;

public enum GameState
{
    None,
    Menu,
    Playing,
    Paused,
    GameClear,
    GameOver
}

/// <summary>
/// ゲームの状態を管理するシングルトン
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // シングルトンインスタンスを保持


    public GameState currentGameState = GameState.None; // ゲーム状態
    public GameState CurrentGameState // ゲーム状態取得プロパティ
    {
        get { return currentGameState; }
        set
        {
            if (currentGameState != value)
            {
                //Debug.Log($"GameState changed: {currentGameState} -> {value}");
                currentGameState = value;
                OnGameStateChanged?.Invoke(currentGameState);
            }
        }
    }
    /* 他スクリプトで状態切り替え　使用例
     * GameManager.Instance.currentGameState = GameState.Playing;
     */
    
    public event Action<GameState> OnGameStateChanged; // ゲーム状態変更イベント


    void Awake()
    {
        // シングルトンインスタンスの基本設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject); // 既にインスタンスが存在する場合は、重複しないように破棄
            return;
        }
    }
}
