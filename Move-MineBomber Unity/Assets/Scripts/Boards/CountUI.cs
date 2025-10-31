using Bomb.Boards;
using UnityEngine;
using TMPro;
using Bomb.Managers;
using Bomb.Boards.Flagged;
using UniRx;
using Zenject;

namespace Bomb.Views
{
    public class CountUI : MonoBehaviour
    {
        [Inject] private GameSceneRooter _gameSceneRooter;
        [Inject] private BoardController _boardController;
        public TextMeshProUGUI BombValue;

        private void Sub()
        {
            BombValue.SetText(_boardController.BombRemaining.ToString());
            _boardController.OnBombHit += _ =>
                        {
                            BombValue.SetText(_boardController.BombRemaining.ToString());
                        };
        }
        private void Awake()
        {
            if (_gameSceneRooter.GameAwaked)
            {
                Sub();
            }
            else
            {
                _gameSceneRooter.OnGameInvoked.AsObservable().Subscribe(_ =>
                {
                    Sub();
                });
            }
        }
    }
}