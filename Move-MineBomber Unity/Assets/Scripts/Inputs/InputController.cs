using Bomb.Managers;
using UnityEngine;

namespace Bomb.Inputs
{
    public class InputController
    {
        private GameSceneRooter _rooter;

        public InputController(GameSceneRooter sceneManager)
        {
            _rooter = sceneManager;
        }

        public void Input(Vector2 pos, bool isLeft = true)
        {
            // なんやかんや判定
            var mass = _rooter.View.GetInfoFromPosition(pos);
            if (isLeft)
                _rooter.Manager.Board.Hit(mass);
            else
                _rooter.Manager.Board.ToggleFlag(mass);
        }
    }
}