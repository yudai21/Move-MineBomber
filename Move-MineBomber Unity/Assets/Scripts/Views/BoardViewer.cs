using Bomb.Boards;
using UnityEngine;

namespace Bomb.Views
{
    public class BoardViewer : MonoBehaviour
    {
        [SerializeField] private Vector2 _centerPos = Vector2.zero;
        [SerializeField] private float _massScale = 1.0f;

        private BoardManager _boardManager;
        public void Invoke(BoardManager manager)
        {
            _boardManager = manager;
        }

        public void BoardView()
        {
            // TODO : ボードの表示更新処理
        }

        public MassInfo GetMassFromVector3(Vector3 position)
        {
            if (_boardManager == null) return default;

            (var centerX, var centerY) = _boardManager.GetCenter();

            int gridX = Mathf.RoundToInt((position.x - _centerPos.x) / _massScale) + centerX;
            int gridY = Mathf.RoundToInt((position.y - _centerPos.y) / _massScale) + centerY;

            var all = _boardManager.Board;
            if (gridX < 0 || gridY < 0 ||
                gridX >= all.GetLength(0) || gridY >= all.GetLength(1))
                return default;

            return all[gridX, gridY];
        }


        private void OnDrawGizmos()
        {
            if (_boardManager == null) return;
            (var centerX, var centerY) = _boardManager.GetCenter();
            foreach (var mass in _boardManager.GetAllMasses())
            {
                var x = mass.x - centerX + _centerPos.x;
                var y = mass.y - centerY + _centerPos.y;

                Color col;
                switch (mass.type)
                {
                    case MassType.Bomb:
                        col = Color.red;
                        break;
                    case MassType.Empty:
                        col = Color.blue;
                        break;
                    default:
                        col = Color.green;
                        break;
                }
                Gizmos.color = col;
                Gizmos.DrawSphere(new Vector3(x * _massScale, y * _massScale, 0), _massScale / 5);
            }
        }
    }
}