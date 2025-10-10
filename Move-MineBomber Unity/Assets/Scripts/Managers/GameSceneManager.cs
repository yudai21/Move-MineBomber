using Bomb.Boards;
using Bomb.Datas;
using System;

namespace Bomb.Managers
{
    public class GameSceneManager : IDisposable
    {
        private BoardController _board;
        public BoardController Board => _board;

        public GameSceneManager()
        {
            _board = new BoardController();
        }
        public void Invoke(GameRule rule)
        {
            _board.Invoke(rule);
        }

        public void Dispose()
        {
            _board.Dispose();
        }
    }
}