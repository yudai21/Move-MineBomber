using Bomb.Boards.Flagged;
using Bomb.Datas;
using System;

namespace Bomb.Boards
{
    public class BoardController
    {
        private BoardManager _boardManager = new();
        private FlagController _flagController;
        private MassManager _massManager;

        // 数値管理
        private int _bombRemaining = 0;

        public BoardManager Board => _boardManager;
        public MassManager MassManager => _massManager;

        //
        public int BombRemaining => _bombRemaining;
        // 
        public event Action<MassInfo> OnBombHit;
        public event Action<MassInfo> OnMassHit; // マスの種類にかかわらず開かれた場合に呼ばれる
        public event Action<int> OnFlagCountChanged;
        public event Action<MassInfo, bool> OnFlagToggled;
        public event Action<BoardManager> OnBoardRebuilt;
        public BoardController()
        {
            _massManager = new MassManager(this);
            _flagController = new(this);
            
        }

        public void Invoke(GameRule rule)
        {
            _flagController.Init((int)(rule.FlagRate * Math.Pow(rule.MapSize, 2))); 
            OnBoardRebuilt?.Invoke(_boardManager);
            _bombRemaining = BoardBuilder.Create(out _boardManager, rule);
        }
        // 通知
        public void NotifyBombHit(MassInfo info)
        {
            _bombRemaining--;
            OnBombHit?.Invoke(info);
        }
        public void NotifyFlagCountChanged(int remaining)
        {
            OnFlagCountChanged?.Invoke(remaining);
        }
        public void NotifyFlagToggled(MassInfo info, bool toggle)
        {
            OnFlagToggled?.Invoke(info, toggle);
        }
        public void NotifyMassHit(MassInfo info)
        {
            OnMassHit?.Invoke(info);
        }
        public void Hit(MassInfo info) => _massManager.Hit(info);
        public void Hit(int x, int y) => _massManager.Hit(x, y);
        public void ToggleFlag(MassInfo info) => _flagController.ToggleFlag(info);
        public void ToggleFlag(int x, int y) => _flagController.ToggleFlag(x, y);
    }
}