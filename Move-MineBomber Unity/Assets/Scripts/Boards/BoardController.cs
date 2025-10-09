using Bomb.Boards.Flagged;
using Bomb.Boards.Slides;
using Bomb.Datas;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb.Boards
{
    public class BoardController
    {
        private BoardManager _boardManager = new();
        private FlagController _flagController;
        private MassManager _massManager;
        private SlideHandler _slideHandler;

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
        public event Action<MassInfo, FlagController.FlagToggleResult> OnFlagToggled;
        public event Action<BoardManager> OnBoardRebuilt;
        public event Action<List<SlideResult>> OnBoardMove; // 移動による影響を受けたマスを通知
        public event Action<bool> OnPause;
        public BoardController()
        {
            _massManager = new MassManager(this);
            _flagController = new(this);
            _slideHandler = new(this);
        }

        public void Invoke(GameRule rule)
        {
            // マップ構築
            _flagController.Init((int)(rule.FlagRate * Math.Pow(rule.MapSize, 2)));
            OnBoardRebuilt?.Invoke(_boardManager);
            _bombRemaining = BoardBuilder.Create(out _boardManager, rule);
            _slideHandler.Invoke(rule);
        }

        public void Pause(bool pause)
        {
            OnPause?.Invoke(pause);
        }
        // 通知
        public void NotifyBombHit(MassInfo info)
        {
            _bombRemaining--;
#if UNITY_EDITOR
            if (BombRemaining > 0)
                Debug.Log($"Bomb Found: {BombRemaining}");
            else
            {
                Debug.Log("Game Clear");
            }
#endif
            OnBombHit?.Invoke(info);
        }
        public void NotifyFlagCountChanged(int remaining)
        {
            OnFlagCountChanged?.Invoke(remaining);
        }
        public void NotifyFlagToggled(MassInfo info, FlagController.FlagToggleResult toggle)
        {
            OnFlagToggled?.Invoke(info, toggle);
        }
        public void NotifyMassHit(MassInfo info)
        {
            OnMassHit?.Invoke(info);
        }
        public void NotifyBoardMove(List<SlideResult> moves)
        {
            OnBoardMove?.Invoke(moves);
        }
        public bool Hit(MassInfo info) => _massManager.Hit(info);
        public bool Hit(int x, int y) => _massManager.Hit(x, y);
        public void ToggleFlag(MassInfo info) => _flagController.ToggleFlag(info);
        public void ToggleFlag(int x, int y) => _flagController.ToggleFlag(x, y);
    }
}