using Bomb.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace Bomb.Inputs
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputWatcher : MonoBehaviour
    {
        private Vector2 _vec;
        public void OnMouse(InputValue value)
        {
            _vec = value.Get<Vector2>();
        }
        public void OnLeftClick(InputValue value)
        {
            var bV = GameSceneRooter.instance.BoardViewer;
            Debug.Log($"クリックしたマス:{bV.GetMassFromVector3(_vec).type.ToString()}");
        }
    }
}