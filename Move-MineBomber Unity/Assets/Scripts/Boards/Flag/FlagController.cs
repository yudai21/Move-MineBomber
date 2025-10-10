using System;
using UnityEngine;

namespace Bomb.Boards.Flagged
{
    public class FlagController
    {
        private readonly BoardController _boardController;
        private int _flagsRemaining = 0;

        public int FlagsRemaining => _flagsRemaining;
        public event Action<int> OnFlagsChanged;

        public FlagController(BoardController controller)
        {
            _boardController = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <summary>初期フラグ数を設定（例：爆弾数と同じ）</summary>
        public void Init(int initialFlags)
        {
            _flagsRemaining = Math.Max(0, initialFlags);
            _boardController.NotifyFlagCountChanged(_flagsRemaining);
            _boardController.OnMassHit += m =>
            {
                if ((m.type & MassType.Flagged) != 0) ToggleFlag(m);
            };
        }

        /// <summary>マスにフラグをトグル。成功したら true</summary>
        public bool ToggleFlag(MassInfo info)
        {
            // 既にフラグあり → 外す（残数を戻す）
            if ((info.type & MassType.Flagged) != 0)
            {
                info.type &= ~MassType.Flagged;
                _flagsRemaining++;
                _boardController.Board.CopyMassState(info.x, info.y, info);
                _boardController.NotifyFlagCountChanged(_flagsRemaining);
                OnFlagsChanged?.Invoke(_flagsRemaining);

                _boardController.NotifyFlagToggled(info, false);
                return true;
            }

            // 無効/開示済み/ダミーには置かない
            if (info.IsDummy || (info.type & MassType.Opened) != 0)
                return false;
            // これから置く → 残数チェック
            if (_flagsRemaining <= 0)
                return false;

            info.type |= MassType.Flagged;
            _flagsRemaining--;
            _boardController.Board.CopyMassState(info.x, info.y, info);
            _boardController.NotifyFlagCountChanged(_flagsRemaining);            
            _boardController.NotifyFlagToggled(info, true);
            Debug.Log("Flag");
            return true;
        }

        public bool ToggleFlag(int x, int y)
        {
            var m = _boardController.Board.GetMass(x, y);
            return ToggleFlag(m);
        }
    }
}
