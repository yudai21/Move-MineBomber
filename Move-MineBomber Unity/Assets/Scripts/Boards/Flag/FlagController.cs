using System;
using UnityEngine;

namespace Bomb.Boards.Flagged
{
    public class FlagController
    {
        public enum FlagToggleResult { Placed, Removed, DeniedOpened, DeniedDummy, DeniedNoStock }

        private readonly BoardController _boardController;
        private int _flagsRemaining = 0;

        public int FlagsRemaining => _flagsRemaining;

        public FlagController(BoardController controller)
        {
            _boardController = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <summary>初期フラグ数を設定（例：爆弾数と同じ）</summary>
        public void Init(int initialFlags)
        {
            _flagsRemaining = Math.Max(0, initialFlags);
            _boardController.NotifyFlagCountChanged(_flagsRemaining);
            _boardController.OnMassHit -= OnMassHit;
            _boardController.OnMassHit += OnMassHit;
        }

        /// <summary>マスにフラグをトグル。成功したら true</summary>
        public FlagToggleResult ToggleFlag(MassInfo info)
        {
            // 既にフラグあり → 外す（残数を戻す）
            if ((info.type & MassType.Flagged) != 0)
            {
                info.type &= ~MassType.Flagged;
                _flagsRemaining++;
                Commit(info, FlagToggleResult.Removed);
                return FlagToggleResult.Removed;
            }

            // 無効/開示済み/ダミーには置かない
            if (info.IsDummy || (info.type & MassType.Opened) != 0) 
                return FlagToggleResult.DeniedDummy;
            if ((info.type & MassType.Opened) != 0) 
                return FlagToggleResult.DeniedOpened;

            // これから置く → 残数チェック
            if (_flagsRemaining <= 0)
                return FlagToggleResult.DeniedNoStock;

            info.type |= MassType.Flagged;
            _flagsRemaining--;
            Commit(info, FlagToggleResult.Placed);
            //Debug.Log( FlagToggleResult.Placed);
            return FlagToggleResult.Placed;
        }

        public FlagToggleResult ToggleFlag(int x, int y)
        {
            var m = _boardController.Board.GetMass(x, y);
            return ToggleFlag(m);
        }
        private void Commit(MassInfo info, FlagToggleResult toggledOn)
        {
            _boardController.Board.CopyMassState(info.x, info.y, info);
            _boardController.NotifyFlagCountChanged(_flagsRemaining);
            _boardController.NotifyFlagToggled(info, toggledOn);
        }
        private void OnMassHit(MassInfo info)
        {
            if ((info.type & MassType.Flagged) != 0) ToggleFlag(info);
        }
    }
}
