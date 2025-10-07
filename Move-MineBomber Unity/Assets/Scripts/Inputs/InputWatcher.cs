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
            var bV = GameSceneRooter.instance.BoardViewer;
            var mass = bV.GetMassFromVector3(_vec);
            Debug.Log($"[{_vec}]クリックしたマス:{mass.type.ToString()}, x:{mass.x}, y:{mass.y}");
        }
    }
}