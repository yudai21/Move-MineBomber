using System;
using UniRx;
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
/// ゲームの状態を管理する
/// </summary>
public class GameStateHolder
{
    private readonly ReactiveProperty<GameState> _state = new(GameState.None);

    public GameState CurrentGameState  => _state.Value;
    public IObservable<GameState> Observable => _state;

    public void UpdateState(GameState state) => _state.Value = state;
}
