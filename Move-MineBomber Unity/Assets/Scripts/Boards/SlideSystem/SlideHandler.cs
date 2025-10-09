using Bomb.Boards.Helpers;
using Bomb.Datas;
using HighElixir;
using HighElixir.Timers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb.Boards.Slides
{
    public class SlideHandler : IDisposable
    {
        //
        private BoardController _controller;
        private int _slide;
        // Timer
        public static readonly string _timerKey = "slide_Interval";
        private TimerTicket _timerTicket;

        public TimerTicket Ticket => _timerTicket;
        public SlideHandler(BoardController controller)
        {
            _controller = controller;
            _controller.OnPause -= OnPause;
            _controller.OnPause += OnPause;
        }
        public void Invoke(GameRule rule)
        {
            _slide = rule.MoveLength;
            _timerTicket = GlobalTimer.Update.PulseRegister(rule.IntervalOfMove, _timerKey, OnPulse);
            GlobalTimer.Update.Start(_timerTicket);
        }
        public void SlideLine()
        {
            // スライド方向（1: 正方向, -1: 逆方向）
            var dir = RandomExtensions.Chance(0.5) ? 1 : -1;
            var dist = dir * _slide;

            // アクティブ範囲取得
            var bounds = _controller.Board.GetActiveBounds();
            Debug.Log($"minX:{bounds.minX}, maxX:{bounds.maxX}, minY:{bounds.minY}, maxY:{bounds.maxY}");

            List<SlideResult> affectedMasses = new();

            // スライド対象が縦列か横列かをランダムに決定
            if (RandomExtensions.Chance(0.5f))
            {
                //  縦スライド（列単位） 
                int randX = RandomExtensions.Rand(bounds.minX, bounds.maxX);
                var masses = new List<MassInfo>();
                // 移動対象マスを抽出
                for (int y = bounds.minY; y <= bounds.maxY; y++)
                {
                    var mass = _controller.Board.GetMass(y, randX);
                    if (mass.IsDummy) continue;
                    masses.Add(mass);
                }

                // スライド処理（下方向 or 上方向）
                for (int i = masses.Count - 1; i >= 0; i--)
                {
                    var oldMass = masses[i];
                    var newMass = oldMass;
                    int newY = oldMass.y + dist;

                    if (newY < 0 || newY >= BoardManager.VirtualSize)
                        continue; // 範囲外は無視

                    var target = _controller.Board.GetMass(oldMass.x, newY);
                    if (target.IsDummy)
                    {
                        // 新しい位置に移動
                        newMass.y = newY;
                        _controller.Board.SetMass(newMass);
                        _controller.Board.RemoveMass(oldMass);
                        // スライドに成功した場合にのみ追加
                        affectedMasses.Add(new SlideResult(oldMass, newMass));
                    }
                }

                Debug.Log($"縦列 {randX} を {dist} スライド、影響マス数: {affectedMasses.Count}");
            }
            else
            {
                //  横スライド（行単位） 
                int randY = RandomExtensions.Rand(bounds.minY, bounds.maxY);
                var masses = new List<MassInfo>();
                for (int x = bounds.minX; x <= bounds.maxX; x++)
                {
                    var mass = _controller.Board.GetMass(randY, x);
                    if (mass.IsDummy) continue;
                    masses.Add(mass);
                }

                // スライド処理（右 or 左）
                for (int i = masses.Count - 1; i >= 0; i--)
                {
                    var oldMass = masses[i];
                    var newMass = oldMass;
                    int newX = oldMass.x + dist;

                    if (newX < 0 || newX >= BoardManager.VirtualSize)
                        continue;

                    var target = _controller.Board.GetMass(newX, oldMass.y);
                    if (target.IsDummy)
                    {
                        newMass.x = newX;
                        _controller.Board.SetMass(newMass);
                        _controller.Board.RemoveMass(oldMass);
                        affectedMasses.Add(new SlideResult(oldMass, newMass));
                    }
                }

                Debug.Log($"横行 {randY} を {dist} スライド、影響マス数: {affectedMasses.Count}");
            }

            _controller.NotifyBoardMove(affectedMasses);
        }


        public void Dispose()
        {
            GlobalTimer.Update.Unregister(_timerTicket);
        }

        private void OnPulse()
        {
            Debug.Log("Move!");
            SlideLine();
        }

        private void OnPause(bool pause)
        {
            if (string.IsNullOrEmpty(_timerTicket.Key)) return;
            if (pause)
            {
                GlobalTimer.Update.Stop(_timerTicket);
            }
            else
            {
                GlobalTimer.Update.Start(_timerTicket, init: false);
            }
        }
    }
}