using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndJudgement : MonoBehaviour
{
    private bool _IssStartChange = false;

    void Start()
    {
        _IssStartChange = false;
    }

    void Update()
    {
        switch (GameManager.Instance.currentGameState)
        {
            case GameState.GameClear:
                ClearSceneLoad();
                Debug.Log("<color=Green>クリア判定</color>");
                break;
            case GameState.GameOver:
                GameOverSceneLoad();
                Debug.Log("<color=Red>ゲームオーバー判定</color>");
                break;
            default:
                Debug.Log("<color=Yellow>どちらでもない</color> 現在→"+ GameManager.Instance.currentGameState);
                break;
        }
    }
    void ClearSceneLoad()
    {
        if (_IssStartChange) return;
        SceneManager.LoadSceneAsync("GameClear");
        _IssStartChange = true;
    }

    void GameOverSceneLoad()
    {
        if (_IssStartChange) return;
        SceneManager.LoadSceneAsync("GameOver");
        _IssStartChange = true;
    }
}
