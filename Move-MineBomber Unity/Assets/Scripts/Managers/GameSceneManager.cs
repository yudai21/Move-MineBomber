using Bomb.Boards;
using Bomb.Datas;
using UnityEngine;

namespace Bomb.Managers
{
    public class GameSceneManager
    {
        private BoardController _board;

        public BoardController Board => _board;

        public int BombRemaining => _board.BombRemaining;
        public GameSceneManager()
        {
            _board = new BoardController();
        }
        public void Invoke(GameRule rule)
        {
            _board.Invoke(rule);
            Debug.Log("Bomb Remaining : " + BombRemaining);
            _board.OnBombHit += _ =>
            {
                if (BombRemaining > 0)
                    Debug.Log($"{BombRemaining}");
                else
                {
                    Debug.Log("Game Clear");
                }
            };
        }

    }
}