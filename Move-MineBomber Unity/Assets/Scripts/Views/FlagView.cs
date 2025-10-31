using Bomb.Boards;
using Bomb.Boards.Flagged;
using Bomb.Managers;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Bomb.Views
{
    public class FlagView : MonoBehaviour
    {
        [Inject] private GameSceneRooter _gameSceneRooter;
        [Inject] private BoardController _boardController;

        [SerializeField] private TMP_Text _text;

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
                });
            }
        }

        private void TextUpdate(MassInfo _, FlagController.FlagToggleResult result)
        {
            if (result == FlagController.FlagToggleResult.Placed ||
                    result == FlagController.FlagToggleResult.Removed)
            {
                _text.SetText(_boardController.FlagController.FlagsRemaining.ToString());
            }
        }
        private void Subscribe()
        {
            _text.SetText(_boardController.FlagController.FlagsRemaining.ToString());
            _boardController.OnFlagToggled -= TextUpdate;
            _boardController.OnFlagToggled += TextUpdate;
        }
    }
}