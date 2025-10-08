using System;

namespace Bomb.Datas
{
    [Serializable]
    public struct GameRule
    {
        public int MapSize; // マップのサイズ
        public float BombRate; // マップのボムの割合
        public float FlagRate; // マップに対するフラグの割合
        public int Handling; // 行動可能な手数
        public int IntervalOfMove; // マップが移動するまでの行動間隔
        public int MoveLength; // マップが移動するまでの行動間隔

        public GameRule(int mapSize, int handling, int intervalOfMove = 15, float bombRate = 0.142f, float flagRate = 0.5f, int moveLength = 3)
        {
            MapSize = mapSize;
            BombRate = bombRate;
            FlagRate = flagRate;
            Handling = handling;
            IntervalOfMove = intervalOfMove;
            MoveLength = moveLength;
        }
    }
}