using Bomb.Boards;
using Bomb.Boards.Flagged;
using Bomb.Managers;
using TMPro;
using UniRx;
using UnityEngine;

namespace Bomb.Views
{
    public class FlagView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

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
                });
            }
        }

        private void TextUpdate(MassInfo _, FlagController.FlagToggleResult result)
        {
            var gr = GameSceneRooter.instance;
            if (result == Boards.Flagged.FlagController.FlagToggleResult.Placed ||
                    result == Boards.Flagged.FlagController.FlagToggleResult.Removed)
            {
                _text.SetText(gr.Manager.Board.FlagController.FlagsRemaining.ToString());
            }
        }
        private void Subscribe()
        {
            var gr = GameSceneRooter.instance;
            _text.SetText(gr.Manager.Board.FlagController.FlagsRemaining.ToString());
            gr.Manager.Board.OnFlagToggled -= TextUpdate;
            gr.Manager.Board.OnFlagToggled += TextUpdate;
        }
    }
}