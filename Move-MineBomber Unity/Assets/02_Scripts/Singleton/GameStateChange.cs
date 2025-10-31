using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

/// <summary>
/// シーンに配置してゲームの状態を切り替える
/// </summary>
public class GameStateChange : MonoBehaviour
{
    [Inject] private GameStateHolder _stateHolder;
    [Header("シーンの初期ゲーム状態")]
    [SerializeField] private GameState _gameState = GameState.None;

    [Header("状態切り替えまでの遅延時間")]
    [SerializeField, Min(0)] private float _delayTime = 0;

    private async UniTask ChangeAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));
        _stateHolder.UpdateState(_gameState);
    }

    private void Start()
    {
        if (_delayTime == -1)
            _stateHolder.UpdateState(_gameState);
        else
            ChangeAsync().Forget();
    }
}
