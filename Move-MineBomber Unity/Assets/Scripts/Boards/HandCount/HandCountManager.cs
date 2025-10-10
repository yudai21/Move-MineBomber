using Bomb.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HandCountManager : MonoBehaviour
{
    [SerializeField] int maxMoves = 10;      // 最大手数（制限）
    

    public int MaxMoves => maxMoves;
    private void Awake()
    {
        var gr = GameSceneRooter.instance;
        gr.OnGameInvoked.AsObservable().Subscribe(_ =>
        {
            maxMoves = gr.Rule.Handling;
            gr.InputController.OnHit += DecreaceHandCount;
        }).AddTo(this);
    }
    
    private void DecreaceHandCount()
    {
        maxMoves--;
        Debug.Log($"Remaining Moves: {maxMoves}");
        if (maxMoves <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}


