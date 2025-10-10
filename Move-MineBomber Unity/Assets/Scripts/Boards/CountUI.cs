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
        private FlagController _flagController;
        public TextMeshProUGUI BombValue;
        public TextMeshProUGUI FlagValue;

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
            GameSceneRooter.instance.OnGameInvoke.AsObservable().Subscribe(_ =>
            {
                _boardController = GameSceneRooter.instance.Manager.Board;
                Sub();
            });
        }
    }
}