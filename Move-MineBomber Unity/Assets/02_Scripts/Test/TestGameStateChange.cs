using UnityEngine;

/// <summary>
/// デバッグ用のゲーム状態切り替えクラス
/// </summary>
public class TestGameStateChange : MonoBehaviour
{
    [Header("切り替え先のゲーム状態")]
    [SerializeField] private GameState gameState = GameState.None;

    [Header("切り替えキー")]
    [SerializeField] private KeyCode keyCode = KeyCode.Return;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            GameManager.Instance.currentGameState = gameState;
        }
    }
}
