using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb.Boards
{
    public class BoardManager
    {
        public static readonly int VirtualSize = 50;
        private MassInfo[,] _board = new MassInfo[VirtualSize, VirtualSize];
        private int _currentBoardSize = 0;
        private bool _boardDirty = true;
        public MassInfo[,] Board => _board;
        public int CurrentBoardSize
        {
            get
            {
                if (_boardDirty)
                {
                    _currentBoardSize = GetMaxSpanOfOnes(_board);
                }
                return _currentBoardSize;
            }
        }

        public void SetBoard(MassInfo[,] board)
        {
            _board = board;
            _boardDirty = true;
        }

        public void SetMass(int x, int y, MassInfo mass)
        {
            if (_board == null) return;
            _board[x, y] = mass;
            //#if UNITY_EDITOR
            //            Debug.Log($"Set : x{x}, y{y}");
            //#endif
            _boardDirty = true;
        }
        public void SetMass(MassInfo info) => SetMass(info.x, info.y, info);

        /// <summary>
        /// マスの状態をコピーする
        /// </summary>
        public void CopyMassState(int x, int y, MassInfo info)
        {
            _board[x, y].type = info.type;
            _board[x, y].aroundBombCount = info.aroundBombCount;
            _boardDirty = true;
        }

        public void CopyMassState(MassInfo info) => CopyMassState(info.x, info.y, info);
        public (int x, int y) GetCenter()
        {
            return (VirtualSize / 2, VirtualSize / 2);
        }

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
        public MassInfo GetMass(int x, int y)
        {
            if (x < 0 || y < 0 || x >= VirtualSize || y >= VirtualSize)
                return default;
            return _board[x, y];
        }
        public MassInfo GetMass(MassInfo mass)
        {
            return GetMass(mass.x, mass.y);
        }
        public List<MassInfo> GetAroundMass(int x, int y, bool skippingDummy = true)
        {
            var res = new List<MassInfo>();
            (int x, int y)[] array =
            {
                (x - 1, y),
                (x + 1, y),
                (x, y - 1),
                (x, y + 1),
                (x + 1, y + 1),
                (x - 1, y - 1),
                (x - 1, y + 1),
                (x + 1, y - 1),
            };
            foreach ((int nx, int ny) in array)
            {
                if (nx < 0 || ny < 0 || nx >= VirtualSize || ny >= VirtualSize)
                    continue; // 範囲外はスキップ

                var m = _board[nx, ny];
                if (skippingDummy && m.IsDummy) continue; // Dummyも除外

                res.Add(m);
            }
            return res;
        }

        private int GetMaxSpanOfOnes(MassInfo[,] grid)
        {
            // 長い方を返す
            return Math.Max(GetHeight(grid), GetWidth(grid));
        }

        private int GetHeight(MassInfo[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            int maxVertical = 0;
            for (int j = 0; j < cols; j++)
            {
                int first = -1, last = -1;
                for (int i = 0; i < rows; i++)
                {
                    if (!grid[i, j].IsDummy)
                    {
                        if (first == -1) first = i;
                        last = i;
                    }
                }

                if (first != -1 && last != -1)
                {
                    int length = last - first + 1;
                    maxVertical = Math.Max(maxVertical, length);
                }
            }
            return maxVertical;
        }

        private int GetWidth(MassInfo[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            int maxHorizontal = 0;

            // 横方向の範囲
            for (int i = 0; i < rows; i++)
            {
                int first = -1, last = -1;
                for (int j = 0; j < cols; j++)
                {
                    if (!grid[i, j].IsDummy)
                    {
                        if (first == -1) first = j;
                        last = j;
                    }
                }

                if (first != -1 && last != -1)
                {
                    int length = last - first + 1;
                    maxHorizontal = Math.Max(maxHorizontal, length);
                }
            }
            return maxHorizontal;
        }
    }
}