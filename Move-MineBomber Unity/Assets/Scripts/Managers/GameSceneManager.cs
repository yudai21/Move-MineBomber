using Bomb.Boards;
using Bomb.Datas;

namespace Bomb.Managers
{
    public class GameSceneManager
    {
        private BoardManager _board;

        public BoardManager Board => _board;
        public void Invoke(GameRule rule)
        {
            BoardBuilder.Create(out _board, rule);
        }
    }
}