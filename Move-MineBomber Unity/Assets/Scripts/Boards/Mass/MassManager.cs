using Bomb.Boards.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Bomb.Boards
{
    public class MassManager
    {
        private BoardController _controller;

        public MassManager(BoardController controller)
        {
            _controller = controller;
        }

        public bool Hit(int x, int y)
        {
            var m = _controller.Board.GetMass(x, y);
            if ((m.type & MassType.Opened) != 0)
                return false;

            // 爆弾クリック
            if ((m.type & MassType.Bomb) != 0)
            {
                m.type |= MassType.Opened;
                _controller.Board.CopyMassState(m);
                _controller.NotifyBombHit(m); // ← イベント通知
                _controller.NotifyMassHit(m);
                return false;
            }

            // 空マス開示（BFS）
            Queue<MassInfo> queue = new();
            queue.Enqueue(m);
            var res = false;
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                // 既に開いてたらスキップ
                if ((current.type & MassType.Opened) != 0) continue;

                res = true;
                current.type |= MassType.Opened;
                var around = _controller.Board.GetAroundMass(current.x, current.y);
                int bombs = _controller.Board.GetAroundBomb(current);
                if (bombs > 0)
                {
                    current.aroundBombCount = bombs;
                }
                else
                {
                    foreach (var a in around)
                    {
                        // 開いて無いかつ、空白
                        if ((a.type & MassType.Opened) == 0 && (a.type & MassType.Empty) != 0)
                        {
                            queue.Enqueue(a);
                        }
                    }
                }
                _controller.Board.CopyMassState(current);
                _controller.NotifyMassHit(current);
            }
            return res;
        }

        public bool Hit(MassInfo massInfo) => Hit(massInfo.x, massInfo.y);
    }
}
