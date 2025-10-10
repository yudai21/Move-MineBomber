using Bomb.Managers;
using TMPro;
using UniRx;
using UnityEngine;

namespace Bomb.Views
{
    public class HandCountManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private int maxMoves = 10;      // 最大手数（制限）


        public int MaxMoves => maxMoves;
        private void Awake()
        {
            var gr = GameSceneRooter.instance;
            if (gr.GameAwaked)
            {
                Subscribe();
            }
            else
            {
                gr.OnGameInvoked.AsObservable().Subscribe(_ =>
                {
                    Subscribe();
                }).AddTo(this);
            }
        }

        private void Subscribe()
        {
            var gr = GameSceneRooter.instance;
            maxMoves = gr.Rule.Handling;
            _text.SetText(maxMoves.ToString());
            gr.InputController.OnHit += DecreaceHandCount;
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
}