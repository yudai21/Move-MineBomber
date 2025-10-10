using Bomb.Datas;
using HighElixir.Timers;
using System;
using System.Collections.Generic;

namespace Bomb.Boards.Slides
{
    public class SlideSystem : IDisposable
    {
        //
        private BoardController _controller;
        private GameRule _rule;
        // Timer
        public static readonly string _timerKey = "slide_Interval";
        private TimerTicket _timerTicket;

        public TimerTicket Ticket => _timerTicket;
        public SlideSystem(BoardController controller)
        {
            _controller = controller;
            _controller.OnPause -= OnPause;
            _controller.OnPause += OnPause;
        }
        public void Invoke(GameRule rule)
        {
            _rule = rule;
            _timerTicket = GlobalTimer.Update.PulseRegister(rule.IntervalOfMove, _timerKey, OnPulse);
            GlobalTimer.Update.Start(_timerTicket);
        }
        public void SlideLine()
        {
            int count = 0;
            List<SlideResult> affectedMasses;
            do
            {
                var masses = _controller.Board.GetRandLine();
                ISlideHandler handler = SlideFactory.CreateRandom(false, _rule.MoveLength, masses.ToArray());
                affectedMasses = handler.Slide(_controller.Board);
                count++;
            } while (affectedMasses.Count <= 0 && count < 100);
            _controller.Board.RefreshBombCount();
            _controller.NotifyBoardMove(affectedMasses);
        }


        public void Dispose()
        {
            GlobalTimer.Update.Unregister(_timerTicket);
        }

        private void OnPulse()
        {
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