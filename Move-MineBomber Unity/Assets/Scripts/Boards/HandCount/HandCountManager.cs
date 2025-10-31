using Bomb.Managers;
using Bomb.Repository;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Bomb.Views
{
    public class HandCountManager : MonoBehaviour
    {
        [Inject] private GameStateHolder _gameStateHolder;
        [Inject] private GameSceneRooter _gameSceneRooter;
        [Inject] private GameDataRepository _repo;

        [SerializeField] private TMP_Text _text;
        private int maxMoves = 10;      // 最大手数（制限）


        public int MaxMoves => maxMoves;
        private void Awake()
        {
            if (_gameSceneRooter.GameAwaked)
            {
                Subscribe();
            }
            else
            {
                _gameSceneRooter.OnGameInvoked.AsObservable().Subscribe(_ =>
                {
                    Subscribe();
                }).AddTo(this);
            }
        }

        private void Subscribe()
        {
            maxMoves = _repo.CurrentRule.Handling;
            _text.SetText(maxMoves.ToString());
            _gameSceneRooter.InputController.OnHit += DecreaceHandCount;
        }
        private void DecreaceHandCount()
        {
            maxMoves--;
            //Debug.Log($"Remaining Moves: {maxMoves}");
            _text.SetText(maxMoves.ToString());
            if (maxMoves <= 0)
            {
                _gameStateHolder.UpdateState(GameState.GameOver);
            }
        }
    }
}