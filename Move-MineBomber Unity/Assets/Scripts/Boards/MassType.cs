using System;

namespace Bomb.Boards
{
    [Flags]
    public enum MassType
    {
        None = 0,
        Empty = 1 << 0, // 空
        Flags = 1 << 1, // フラグ付き
        Bomb = 1 << 2, // 爆弾
        Warning = 1 << 3, // 移動の警告表示
    }
}