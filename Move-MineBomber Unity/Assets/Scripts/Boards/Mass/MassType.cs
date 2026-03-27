using System;

namespace Bomb.Boards
{
    [Flags]
    public enum MassType
    {
        None = 0,
        Empty = 1 << 0, // 空
        Flagged = 1 << 1, // フラグ付き
        Bomb = 1 << 2, // 爆弾
        Warning = 1 << 3, // 移動の警告表示
        Opened = 1 << 4, // 開示済み
    }

    public static class MassTypeEx 
    {
        public static bool Has(this MassType value, MassType needType)
        {
            return (value & needType) != 0;
        }
    }
}