using Bomb.Boards;
using UnityEngine;
using TMPro;
using Bomb.Managers;
using Bomb.Boards.Flagged;
using UniRx;

namespace Bomb.Views
{
    public class CountUI : MonoBehaviour
    {
        private BoardController _boardController;
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
            if (GameSceneRooter.instance.GameAwaked)
            {
                _boardController = GameSceneRooter.instance.Manager.Board;
                Sub();
            }
            else
            {
                GameSceneRooter.instance.OnGameInvoked.AsObservable().Subscribe(_ =>
                {
                    _boardController = GameSceneRooter.instance.Manager.Board;
                    Sub();
                });
            }
        }
    }
}