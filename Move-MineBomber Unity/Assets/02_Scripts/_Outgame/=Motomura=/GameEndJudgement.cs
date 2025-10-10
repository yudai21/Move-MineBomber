using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndJudgement : MonoBehaviour
{
    private bool _IssStartChange = false;
    void Update()
    {
        switch (GameManager.Instance.currentGameState)
        {
            case GameState.GameClear:
                ClearSceneLoad();
                break;
            case GameState.GameOver:
                GameOverSceneLoad();
                break;
        }
    }
    void ClearSceneLoad()
    {
        if (!_IssStartChange) return;
        SceneManager.LoadSceneAsync("GameClear");
        _IssStartChange = true;
    }

    void GameOverSceneLoad()
    {
        if (!_IssStartChange) return;
        SceneManager.LoadSceneAsync("GameOver");
        _IssStartChange = true;
    }
}
