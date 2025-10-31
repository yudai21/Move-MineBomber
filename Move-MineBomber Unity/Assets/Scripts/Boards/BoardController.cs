using Bomb.Boards.Builders;
using Bomb.Boards.Flagged;
using Bomb.Boards.Slides;
using Bomb.Datas;
using Bomb.Repository;
using System;
using System.Collections.Generic;
using Zenject;

namespace Bomb.Boards
{
    public class BoardController : IDisposable, IInitializable
    {
        [Inject] private GameStateHolder _gameStateHolder;
        [Inject] private IBoardBuilder _boardBuilder;
        [Inject] private GameDataRepository _repo;

        private BoardManager _boardManager = new();
        private FlagController _flagController;
        private MassManager _massManager;
        private SlideSystem _slideSystem;

        // 数値管理
        private int _bombRemaining = 0;

        public BoardManager Board => _boardManager;
        public MassManager MassManager => _massManager;
        public FlagController FlagController => _flagController;
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
            Initialize();
        }
        public void Invoke()
        {
            // マップ構築
            _flagController.Init((int)(_repo.CurrentRule.FlagRate * Math.Pow(_repo.CurrentRule.MapSize, 2)));
            OnBoardRebuilt?.Invoke(_boardManager);
            _bombRemaining = _boardBuilder.Create(out _boardManager);
            _slideSystem.Invoke(_repo.CurrentRule);

            //OnMassHit += m => Debug.Log($"[Board]:Hitted. info:[{m.ToString()}]");
        }

        public void Pause(bool pause)
        {
            OnPause?.Invoke(pause);
        }
        // 通知
        public void NotifyBombHit(MassInfo info)
        {
            _bombRemaining--;
            //if (BombRemaining > 0)
            //    Debug.Log($"Bomb Found: {BombRemaining}");
            if (_bombRemaining <= 0)
            {
                //Debug.Log("Game Clear");
                _gameStateHolder.UpdateState(GameState.GameClear);
            }
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

        public void Dispose()
        {
            _slideSystem?.Dispose();
        }

        public void Initialize()
        {
            _massManager = new MassManager(this);
            _flagController = new(this);
            _slideSystem = new(this);
        }
    }
}