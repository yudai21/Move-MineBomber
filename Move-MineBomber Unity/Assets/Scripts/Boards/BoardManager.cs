using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

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
                    _currentBoardSize = GetMaxSpanOfOnes();
                }
                return _currentBoardSize;
            }
        }
        public (int min, int max) GetBoardHeight(int x)
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            int first = -1, last = -1;
            for (int i = 0; i < rows; i++)
            {
                if (!_board[i, x].IsDummy)
                {
                    if (first == -1) first = i;
                    last = i;
                }
            }
            return (first, last);
        }

        public (int min, int max) GetBoardWidth(int y)
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            // 横方向の範囲
            int first = -1, last = -1;
            for (int j = 0; j < cols; j++)
            {
                if (!_board[y, j].IsDummy)
                {
                    if (first == -1) first = j;
                    last = j;
                }
            }
            return (first, last);
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

        private int GetMaxSpanOfOnes()
        {
            // 長い方を返す
            return Math.Max(GetHeight(), GetWidth());
        }

        private int GetHeight()
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            int maxVertical = 0;
            for (int j = 0; j < cols; j++)
            {
                (int min, int max) = GetBoardHeight(j);
                var length = max - min + 1;
                if (maxVertical < length) maxVertical = length;
            }
            return maxVertical;
        }

        private int GetWidth()
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            int maxHorizontal = 0;

            // 横方向の範囲
            for (int i = 0; i < rows; i++)
            {
                (int min, int max) = GetBoardWidth(i);
                var length = max - min + 1;
                if (maxHorizontal < length) maxHorizontal = length;
            }
            return maxHorizontal;
        }
    }
}