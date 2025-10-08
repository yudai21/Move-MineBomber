using Bomb.Managers;
using System;
using UnityEngine;

namespace Bomb.Inputs
{
    public class InputController
    {
        private GameSceneRooter _rooter;

        public event Action OnHit;
        public InputController(GameSceneRooter sceneManager)
        {
            _rooter = sceneManager;
            OnHit += () => Debug.Log("Hit!!!");
        }

        public void Input(Vector2 pos, bool isLeft = true)
        {
            // なんやかんや判定
            var mass = _rooter.View.GetInfoFromPosition(pos);
            if (isLeft)
            {
                if (_rooter.Manager.Board.Hit(mass))
                    OnHit?.Invoke();
            }
            else
                _rooter.Manager.Board.ToggleFlag(mass);
        }
    }
}