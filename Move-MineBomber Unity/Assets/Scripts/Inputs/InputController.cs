using Bomb.Boards;
using Bomb.Managers;
using Bomb.Views;
using System;
using UnityEngine;

namespace Bomb.Inputs
{
    public class InputController
    {
        private BoardController _cont;
        private ViewObjRooter _view;

        public event Action OnHit;

        public InputController(BoardController cont, ViewObjRooter view)
        {
            _cont = cont;
            _view = view;
        }

        public void Input(Vector2 pos, bool isLeft = true)
        {
            var mass = _view.GetInfoFromPosition(pos);
            if (isLeft)
            {
                if (_cont.Hit(mass))
                    OnHit?.Invoke();
            }
            else
                _cont.ToggleFlag(mass);
        }
    }
}