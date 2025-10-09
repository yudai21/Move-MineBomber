using Bomb.Boards;
using Bomb.Boards.Flagged;
using Bomb.Datas;
using HighElixir.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb.Views
{
    public class BoardViewer : MonoBehaviour
    {
        private class MassWrapper
        {
            public MassViewer viewer;
            public bool isDirty;
        }
        [SerializeField] private Vector2 _centerPos = Vector2.zero; // 画面上の原点オフセット（マス単位）
        [SerializeField] private float _massScale = 1.0f;           // 1マスのスケール（ワールド単位）
        [SerializeField] private MassViewer _pref;

        private Pool<MassViewer> _pool;
        private BoardController _controller;
        private Dictionary<(int x, int y), MassWrapper> _maps = new();
        public void Invoke(BoardController controller)
        {
            _controller = controller;
            _controller.OnMassHit -= OnMassHit;
            _controller.OnMassHit += OnMassHit;
            _controller.OnFlagToggled -= OnFlagToggled;
            _controller.OnFlagToggled += OnFlagToggled;

            _controller.OnBoardMove -= UpdateMovedBoard;
            _controller.OnBoardMove += UpdateMovedBoard;
        }

        /// <summary>
        /// 見た目の更新（後でTilemap/Poolに差し替え前提の雛形）
        /// </summary>
        public void BoardView()
        {
            if (_controller == null) return;

            SetMasses();
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
            if (_controller == null || _massScale <= 0f) return default;

            (int centerX, int centerY) = _controller.Board.GetCenter();

            // 描画式の逆変換：
            //world = (mass - center + _centerPos) * _massScale
            //mass = round(world / _massScale) + center - _centerPos
            float gx = position.x / _massScale;
            float gy = position.y / _massScale;

            int mx = Mathf.RoundToInt(gx + centerX - _centerPos.x);
            int my = Mathf.RoundToInt(gy + centerY - _centerPos.y);
            //Debug.Log($"Checked: x[{mx}], y[{my}]");
            // 境界チェック
            var board = _controller.Board.Board;
            if (board == null) return default;

            int w = board.GetLength(0);
            int h = board.GetLength(1);
            if (mx < 0 || my < 0 || mx >= w || my >= h) return default;

            var m = board[mx, my];
            // ダミーは無効扱い（必要ならここで IsDummy を見て弾く）
            if (m.IsDummy) return default;

            return m;
        }

        public void UpdateMovedBoard(List<SlideResult> results)
        {
            if (results == null || results.Count == 0) return;
            if (_controller == null || _massScale <= 0f) return;

            foreach (var result in results)
            {
                // 古いマス位置に対応するViewerを探す
                if (!_maps.TryGetValue((result.Old.x, result.Old.y), out var wrapper))
                    continue;

                // 座標計算
                SetMass(wrapper.viewer, result.New);

                // 辞書キーを更新（旧位置→新位置）
                _maps.Remove((result.Old.x, result.Old.y));
                _maps[(result.New.x, result.New.y)] = wrapper;

                // Dirtyフラグもリセット
                wrapper.isDirty = false;
            }
        }

        private void SetMasses()
        {
            if (_massScale <= 0f) return;
            (int centerX, int centerY) = _controller.Board.GetCenter();
            foreach (var mass in _controller.Board.GetAllMasses()) // 既定でダミー除外ならOK
            {
                if (!_maps.TryGetValue((mass.x, mass.y), out var wr))
                {
                    wr = new()
                    {
                        viewer = _pool.Get(),
                        isDirty = true
                    };
                    _maps[(mass.x, mass.y)] = wr;
                }
                if (!wr.isDirty) continue;
                wr.isDirty = false;
                SetMass(wr.viewer, mass);
            }
        }

        private void OnMassHit(MassInfo info)
        {
            if (_maps.TryGetValue((info.x, info.y), out var wrapper))
            {
                wrapper.isDirty = true;
            }
        }
        private void OnFlagToggled(MassInfo info, FlagController.FlagToggleResult _)
        {
            if (_maps.TryGetValue((info.x, info.y), out var wrapper))
            {
                wrapper.isDirty = true;
            }
        }

        private void SetMass(MassViewer viewer, MassInfo info)
        {
            (int centerX, int centerY) = _controller.Board.GetCenter();
            float x = (info.x - centerX + _centerPos.x) * _massScale;
            float y = (info.y - centerY + _centerPos.y) * _massScale;
            viewer.transform.position = new Vector2(x, y);
            viewer.transform.localScale = Vector3.one * _massScale;
            viewer.UpdateMass(info);
        }
        //
        private void Awake()
        {
            _pool = new Pool<MassViewer>(_pref, 100, transform, false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            (int cX, int cY) = _controller.Board.GetCenter();
            var mass = _controller.Board.GetAllMasses();
            foreach (var m in mass)
            {
                float x = (m.x - cX + _centerPos.x) * _massScale;
                float y = (m.y - cY + _centerPos.y) * _massScale;
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(new Vector2(x, y), 0.2f);
            }
        }
#endif
    }
}
