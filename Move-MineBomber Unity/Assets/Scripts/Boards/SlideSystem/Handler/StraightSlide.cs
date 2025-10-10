using Bomb.Datas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bomb.Boards.Slides
{
    public class StraightSlide : ISlideHandler
    {
        private readonly Vector2Int _direction;
        private readonly int _distance;
        private readonly List<MassInfo> _targets = new();

        /// <param name="direction">各値は-1, 0, 1のみを取ること</param>
        public StraightSlide(Vector2Int direction, int distance, params MassInfo[] infos)
        {
            _direction = direction;
            _distance = distance;
            _targets = (direction.y >= 0
                ? infos.OrderByDescending(m => m.y)
                : infos.OrderBy(m => m.y))
                .ThenBy(m => direction.x >= 0 ? -m.x : m.x)
                .ToList();
        }
        public List<SlideResult> Slide(BoardManager board)
        {
            var res = new List<SlideResult>();
            var move = _direction * _distance;

            foreach (var info in _targets)
            {
                MassInfo @new = new();
                // 指定距離から1マスずつ距離を減らして再試行
                for (int d = _distance; d > 0; d--)
                {
                    int nX = info.x + _direction.x * d;
                    int nY = info.y + _direction.y * d;

                    if (IsValid(board, nX, nY))
                    {
                        // 成功：移動確定
                        @new = info;
                        @new.x = nX;
                        @new.y = nY;

                        board.MoveMass(info, @new);
                        res.Add(new SlideResult(@new, info));
                        //Debug.Log(@new.ToString());
                        break;
                    }
                }

                //Debug.Log($"Result : old [x:{old.x}, y:{old.y}], new [x:{@new.x}, y:{@new.y}]");
            }

            return res;
        }
        private bool IsValid(BoardManager board, int x, int y)
        {
            if (y < 0 || y >= BoardManager.VirtualSize) return false;
            if (x < 0 || x >= BoardManager.VirtualSize) return false;
            var m = board.GetMass(x, y);
            return m.IsDummy;
        }
    }
}