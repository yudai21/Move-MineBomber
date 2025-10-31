using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameEndJudgement : MonoBehaviour
{
    [Inject] private GameStateHolder _gameState;

    private void Start()
    {
        _gameState.Observable.Subscribe(x =>
        {
            switch (x)
            {
                case GameState.GameClear:
                    ClearSceneLoad();
                    break;
                case GameState.GameOver:
                    GameOverSceneLoad();
                    break;
                default:
                    break;
            };
        });
    }

    void ClearSceneLoad()
    {
        SceneManager.LoadSceneAsync("GameClear");
    }

    void GameOverSceneLoad()
    {
        SceneManager.LoadSceneAsync("GameOver");
    }
}
