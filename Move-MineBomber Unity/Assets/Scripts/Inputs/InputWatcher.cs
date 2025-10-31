using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
namespace Bomb.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputWatcher : MonoBehaviour
    {
        [Inject] InputController _controller;

        private Vector2 _vec;
        private Camera _camera;
        public void OnMouse(InputValue value)
        {
            if (_camera == null) _camera = Camera.main;
            _vec = _camera.ScreenToWorldPoint(value.Get<Vector2>());
        }
        public void OnLeftClick(InputValue value)
        {
            _controller.Input(_vec);
        }
        public void OnRightClick(InputValue value)
        {
            _controller.Input(_vec, false);
        }
    }
}