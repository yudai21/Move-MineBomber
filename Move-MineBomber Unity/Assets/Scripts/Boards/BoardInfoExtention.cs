using System;

namespace Bomb.Boards.Helpers
{
    public static class BoardInfoExtention
    {

        // ===== 幅・高さ関連 =====

        /// <summary>指定列（x）の縦方向の範囲</summary>
        public static (int min, int max) GetBoardHeight(this BoardManager board, int x)
        {
            int rows = board.Board.GetLength(0);
            int first = -1, last = -1;

            for (int i = 0; i < rows; i++)
            {
                if (!board.Board[i, x].IsDummy)
                {
                    if (first == -1) first = i;
                    last = i;
                }
            }
            return (first, last);
        }

        /// <summary>指定行（y）の横方向の範囲</summary>
        public static (int min, int max) GetBoardWidth(this BoardManager board, int y)
        {
            int cols = board.Board.GetLength(1);
            int first = -1, last = -1;

            for (int j = 0; j < cols; j++)
            {
                if (!board.Board[y, j].IsDummy)
                {
                    if (first == -1) first = j;
                    last = j;
                }
            }
            return (first, last);
        }

        /// <summary>最大の高さを返す</summary>
        public static int GetMaxHeight(this BoardManager board)
        {
            int cols = board.Board.GetLength(1);
            int maxVertical = 0;

            for (int x = 0; x < cols; x++)
            {
                (int min, int max) = board.GetBoardHeight(x);
                if (min == -1 || max == -1) continue;

                int length = max - min + 1;
                if (maxVertical < length) maxVertical = length;
            }
            return maxVertical;
        }

        /// <summary>最大の幅を返す</summary>
        public static int GetMaxWidth(this BoardManager board)
        {
            int rows = board.Board.GetLength(0);
            int maxHorizontal = 0;

            for (int y = 0; y < rows; y++)
            {
                (int min, int max) = board.GetBoardWidth(y);
                if (min == -1 || max == -1) continue;

                int length = max - min + 1;
                if (maxHorizontal < length) maxHorizontal = length;
            }
            return maxHorizontal;
        }

        /// <summary>最も高い列とその高さを返す</summary>
        public static (int columnIndex, int height) GetTallestColumn(this BoardManager board)
        {
            int cols = board.Board.GetLength(1);
            int maxHeight = 0;
            int maxColIndex = -1;

            for (int x = 0; x < cols; x++)
            {
                (int min, int max) = board.GetBoardHeight(x);
                if (min == -1 || max == -1) continue;

                int height = max - min + 1;
                if (height > maxHeight)
                {
                    maxHeight = height;
                    maxColIndex = x;
                }
            }
            return (maxColIndex, maxHeight);
        }

        /// <summary>最も広い行とその幅を返す</summary>
        public static (int rowIndex, int width) GetWidestRow(this BoardManager board)
        {
            int rows = board.Board.GetLength(0);
            int maxWidth = 0;
            int maxRowIndex = -1;

            for (int y = 0; y < rows; y++)
            {
                (int min, int max) = board.GetBoardWidth(y);
                if (min == -1 || max == -1) continue;

                int width = max - min + 1;
                if (width > maxWidth)
                {
                    maxWidth = width;
                    maxRowIndex = y;
                }
            }
            return (maxRowIndex, maxWidth);
        }
        /// <summary>
        /// ダミーでないマスが存在する範囲（x, yの最小・最大）を取得
        /// </summary>
        /// <returns>(minX, maxX, minY, maxY)</returns>
        public static (int minX, int maxX, int minY, int maxY) GetActiveBounds(this BoardManager board)
        {
            int rows = board.Board.GetLength(0);
            int cols = board.Board.GetLength(1);

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            bool found = false;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (!board.Board[y, x].IsDummy)
                    {
                        found = true;
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            // ダミー以外が存在しない場合は (-1, -1, -1, -1)
            if (!found)
                return (-1, -1, -1, -1);

            return (minX, maxX, minY, maxY);
        }

        /// <summary>最大スパンを取得（高さと幅の大きい方）</summary>
        private static int GetMaxSpanOfOnes(this BoardManager board)
        {
            return Math.Max(board.GetMaxHeight(), board.GetMaxWidth());
        }

    }
}