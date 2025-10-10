using Bomb.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Bomb.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputWatcher : MonoBehaviour
    {
        private Vector2 _vec;
        private Camera _camera;
        public void OnMouse(InputValue value)
        {
            if (_camera == null) _camera = Camera.main;
            _vec = _camera.ScreenToWorldPoint(value.Get<Vector2>());
        }
        public void OnLeftClick(InputValue value)
        {
            var iC = GameSceneRooter.instance.InputController;
            iC.Input(_vec);
            
        }
        public void OnRightClick(InputValue value)
        {
            //Debug.Log("Right Click");
            var iC = GameSceneRooter.instance.InputController;
            iC.Input(_vec, false);
        }
    }
}