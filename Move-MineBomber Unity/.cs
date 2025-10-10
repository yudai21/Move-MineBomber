using Bomb.Managers;
using TMPro;
using UniRx;
using UnityEngine;

public class FlagViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private int maxMoves = 10;      // Å‘åŽè”i§ŒÀj


    public int MaxMoves => maxMoves;
    private void Awake()
    {
        var gr = GameSceneRooter.instance;
        gr.OnGameInvoked.AsObservable().Subscribe(_ =>
        {
            maxMoves = gr.Rule.Handling;
            _text.SetText(maxMoves.ToString());
            gr.InputController.OnHit += DecreaceHandCount;
        }).AddTo(this);
    }

    private void DecreaceHandCount()
    {
        maxMoves--;
        //Debug.Log($"Remaining Moves: {maxMoves}");
        _text.SetText(maxMoves.ToString());
        if (maxMoves <= 0)
        {
            GameManager.Instance.CurrentGameState = GameState.GameOver;
        }
    }
}


