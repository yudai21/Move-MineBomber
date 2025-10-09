using Bomb.Boards;
using Bomb.Datas;

namespace Bomb.Managers
{
    public class GameSceneManager
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
    }
}