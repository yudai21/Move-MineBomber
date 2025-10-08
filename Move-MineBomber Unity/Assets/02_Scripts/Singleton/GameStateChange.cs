using System.Collections;
using UnityEngine;

/// <summary>
/// シーンに配置してゲームの状態を切り替える
/// </summary>
public class GameStateChange : MonoBehaviour
{
    [Header("シーンの初期ゲーム状態")]
    [SerializeField] private GameState gameState = GameState.None;

    [Header("状態切り替えまでの遅延時間")]
    [SerializeField] private bool DelayChange = false;
    [SerializeField] private float DelayTime = 0;


    void Start()
    {
        if (DelayChange == false)
        {
            StateChange();
        }
        else
        {
            StartCoroutine(DelayStateChange());
        }
    }

    private void StateChange()
    {
        GameManager.Instance.CurrentGameState = gameState;
    }

    private IEnumerator DelayStateChange()
    {
        yield return new WaitForSeconds(DelayTime);

        StateChange();

        yield return null;
    }
}
