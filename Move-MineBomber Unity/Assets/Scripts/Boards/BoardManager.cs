using Bomb.Boards.Helpers;
using System.Collections.Generic;
using System.Text;

namespace Bomb.Boards
{
    public class BoardManager
    {
        public static readonly int VirtualHeight = 9;
        public static readonly int VirtualWidth = 14;

        private MassInfo[,] _board = new MassInfo[VirtualWidth, VirtualHeight];

        public MassInfo[,] Board => _board;

        public static int VirtualSize => VirtualHeight * VirtualWidth;
        // ===== 基本操作 =====
        public void SetBoard(MassInfo[,] board)
        {
            _board = board;
        }

        public void MoveMass(MassInfo oldPos, MassInfo newPos) => MoveMass(oldPos, newPos.x, newPos.y);
        public void MoveMass(MassInfo oldPos, int newX, int newY)
        {
            SetMass(newX, newY, oldPos);
            RemoveMass(oldPos);
        }
        public void SetMass(int x, int y, MassInfo mass)
        {
            if (_board == null) return;
            mass.x = x;
            mass.y = y;
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
            _board[x, y].x = 0;
            _board[x, y].y = 0;
        }
        public void RemoveMass(MassInfo info) => RemoveMass(info.x, info.y);
        // ===== 情報取得 =====

        /// <summary>ボードの中央座標を返す</summary>
        public (int x, int y) GetCenter() => (VirtualSize / 2, VirtualSize / 2);

        /// <summary>全てのマスを取得（Dummyを無視可能）</summary>
        public List<MassInfo> GetAllMasses(bool ignoreDummy = true)
        {
            List<MassInfo> list = new();
            int width = _board.GetLength(0);
            int height = _board.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var m = _board[x, y];
                    if (ignoreDummy && m.IsDummy) continue;
                    list.Add(m);
                }
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

        public void RefreshBombCount()
        {
            var ii = _board.GetLength(0);
            var jj = _board.GetLength(1);
            for(int i = 0; i < ii; i++)
            {
                for (int j = 0; j < jj; j++)
                {
                    if (_board[i,j].IsDummy) continue;
                    var m = _board[i, j];

                    if (m.type.Has(MassType.Opened) && !m.type.Has(MassType.Bomb))
                    {
                        _board[i, j].aroundBombCount = this.GetAroundBomb(m);
                    }
                }
            }
        }
        public override string ToString()
        {
            if (_board == null)
                return "[Board is null]";

            var sb = new StringBuilder();
            int width = _board.GetLength(0);
            int height = _board.GetLength(1);
            sb.AppendLine("[Board]");
            int count = 0;
            // 上から下に表示（y軸逆順）
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    var m = _board[x, y];

                    // マスのタイプに応じて1文字を割り当て
                    char c = m.type switch
                    {
                        MassType.None => '0',
                        MassType.Empty => '1',
                        MassType.Flagged => '2',
                        MassType.Bomb => '3',
                        MassType.Warning => '4',
                        MassType.Opened => '5',
                        _ => '6'
                    };
                    if (!m.IsDummy) count++;
                    sb.Append(c);
                }
                sb.AppendLine();
            }
            sb.AppendLine("Mass : " + count);
            return sb.ToString();
        }
    }
}
