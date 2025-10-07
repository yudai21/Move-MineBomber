using Bomb.Boards;
using HighElixir.Pool;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Bomb.Views
{
    public class BoardViewer : MonoBehaviour
    {
        [SerializeField] private Vector2 _centerPos = Vector2.zero; // 画面上の原点オフセット（マス単位）
        [SerializeField] private float _massScale = 1.0f;           // 1マスのスケール（ワールド単位）
        [SerializeField] private GameObject _pref;

        private Pool<GameObject> _pool;
        private BoardManager _boardManager;
        private Dictionary<MassInfo, GameObject> _maps = new();
        public void Invoke(BoardManager manager)
        {
            _boardManager = manager;
            // 必要ならここで初期描画（Sprite/Tilemap）を起動
            // BoardView();
        }

        /// <summary>
        /// 見た目の更新（後でTilemap/Poolに差し替え前提の雛形）
        /// </summary>
        public void BoardView()
        {
            if (_boardManager == null || _massScale <= 0f) return;

            (int centerX, int centerY) = _boardManager.GetCenter();
            foreach (var mass in _boardManager.GetAllMasses()) // 既定でダミー除外ならOK
            {
                float x = (mass.x - centerX + _centerPos.x) * _massScale;
                float y = (mass.y - centerY + _centerPos.y) * _massScale;

                if (!_maps.TryGetValue(mass, out var go))
                    go = _pool.Get();
                go.transform.position = new Vector2(x, y);
                go.GetComponent<SpriteRenderer>().color = PickColor(mass.type);

                _maps.TryAdd(mass, go);
            }
            // 例：
            // 1) _boardManager.GetAllMasses()でアクティブマスを走査
            // 2) 各マスの表示(色/スプライト/エフェクト)を更新
            // 3) 既存インスタンスはPool再利用
            //
            // 将来：
            // - スライド演出中は EffectManager 経由でTransformを補間
            // - UI更新はイベント購読（OnBombCountChanged など）
        }

        /// <summary>
        /// ワールド座標から盤面マスを逆算して取得（無効なら default を返す）
        /// </summary>
        public MassInfo GetMassFromVector3(Vector3 position)
        {
            if (_boardManager == null || _massScale <= 0f) return default;

            (int centerX, int centerY) = _boardManager.GetCenter();

            // 描画式の逆変換：
            //world = (mass - center + _centerPos) * _massScale
            //mass = round(world / _massScale) + center - _centerPos
            float gx = position.x / _massScale;
            float gy = position.y / _massScale;

            int mx = Mathf.RoundToInt(gx + centerX - _centerPos.x);
            int my = Mathf.RoundToInt(gy + centerY - _centerPos.y);
            Debug.Log($"Checked: x[{mx}], y[{my}]");
            // 境界チェック
            var board = _boardManager.Board;
            if (board == null) return default;

            int w = board.GetLength(0);
            int h = board.GetLength(1);
            if (mx < 0 || my < 0 || mx >= w || my >= h) return default;

            var m = board[mx, my];
            // ダミーは無効扱い（必要ならここで IsDummy を見て弾く）
            if (m.IsDummy) return default;

            return m;
        }

        /// <summary>
        /// フラグ前提の色決定（switch等価比較はNG）
        /// </summary>
        private static Color PickColor(MassType type)
        {
            // 優先度例：Bomb > Empty > その他
            if ((type & MassType.Bomb) != 0) return Color.red;
            if ((type & MassType.Empty) != 0) return Color.blue;
            return Color.green;
        }

        private void Awake()
        {
            _pool = new Pool<GameObject>(_pref, 100, transform, false);
        }
    }
}
