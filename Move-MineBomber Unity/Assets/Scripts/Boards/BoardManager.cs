using System;
using System.Collections.Generic;

namespace Bomb.Boards
{
    public class BoardManager
    {
        public static readonly int VirtualSize = 50;

        private MassInfo[,] _board = new MassInfo[VirtualSize, VirtualSize];

        public MassInfo[,] Board => _board;

        // ===== 基本操作 =====

        public void SetBoard(MassInfo[,] board)
        {
            _board = board;
        }

        public void SetMass(int x, int y, MassInfo mass)
        {
            if (_board == null) return;
            _board[x, y] = mass;
        }

        public void SetMass(MassInfo info) => SetMass(info.x, info.y, info);

        public void CopyMassState(int x, int y, MassInfo info)
        {
            _board[x, y].type = info.type;
            _board[x, y].aroundBombCount = info.aroundBombCount;
        }

        public void CopyMassState(MassInfo info) => CopyMassState(info.x, info.y, info);

        public void RemoveMass(int x, int y)
        {
            _board[x, y].type = MassType.None;
        }
        public void RemoveMass(MassInfo info) => RemoveMass(info.x, info.y);
        // ===== 情報取得 =====

        /// <summary>ボードの中央座標を返す</summary>
        public (int x, int y) GetCenter() => (VirtualSize / 2, VirtualSize / 2);

        /// <summary>全てのマスを取得（Dummyを無視可能）</summary>
        public List<MassInfo> GetAllMasses(bool ignoreDummy = true)
        {
            List<MassInfo> list = new();
            foreach (MassInfo m in _board)
            {
                if (ignoreDummy && m.IsDummy) continue;
                list.Add(m);
            }
            return list;
        }

        /// <summary>指定位置のマスを取得</summary>
        public MassInfo GetMass(int x, int y)
        {
            if (x < 0 || y < 0 || x >= VirtualSize || y >= VirtualSize)
                return default;
            return _board[x, y];
        }

        public MassInfo GetMass(MassInfo mass) => GetMass(mass.x, mass.y);

        /// <summary>周囲8方向のマスを取得</summary>
        public List<MassInfo> GetAroundMass(int x, int y, bool skippingDummy = true)
        {
            var res = new List<MassInfo>();
            (int x, int y)[] array =
            {
                (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1),
                (x + 1, y + 1), (x - 1, y - 1), (x - 1, y + 1), (x + 1, y - 1),
            };

            foreach ((int nx, int ny) in array)
            {
                if (nx < 0 || ny < 0 || nx >= VirtualSize || ny >= VirtualSize)
                    continue;

                var m = _board[nx, ny];
                if (skippingDummy && m.IsDummy) continue;

                res.Add(m);
            }
            return res;
        }
    }
}
