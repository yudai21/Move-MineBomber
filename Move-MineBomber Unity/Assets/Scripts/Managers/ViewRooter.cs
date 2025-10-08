using Bomb.Boards;
using HighElixir.Pool;
using TMPro;
using UnityEngine;

namespace Bomb.Views
{
    /// <summary>
    /// ビュー系の参照をまとめるルーター/ルータ（ハブ）。
    /// - BoardViewerの呼び出し窓口
    /// - World座標→Canvas座標(anchoredPosition)変換
    /// RenderModeごとの扱いを吸収して安全に座標変換する。
    /// </summary>
    public class ViewObjRooter
    {
        private BoardViewer _board;
        private Camera _camera;              // 3Dワールドを映すカメラ（必須）
        private Canvas _canvas;              // UIキャンバス
        private RectTransform _canvasRect;   // キャッシュ
        private Pool<TMP_Text> _textPool;
        public BoardViewer Board => _board;
        public Camera Camera => _camera;
        public Canvas Canvas => _canvas;
        public Pool<TMP_Text> Pool => _textPool;
        public bool IsReady => _board != null && _camera != null && _canvasRect != null;

        public ViewObjRooter(BoardViewer boardViewer, Pool<TMP_Text> textPool)
        {
            _board = boardViewer;
            _textPool = textPool;
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public void SetCanvas(Canvas canvas)
        {
            _canvas = canvas;
            _canvasRect = canvas != null ? canvas.GetComponent<RectTransform>() : null;
        }

        public void Invoke(BoardController controller)
        {
            _board?.Invoke(controller);
        }

        public void Update(float deltaTime)
        {
            _board.BoardView();
        }
        /// <summary>
        /// World座標→Canvas座標(anchoredPosition)。失敗時はVector2.zero。
        /// CanvasのRenderMode差を吸収する。
        /// </summary>
        public Vector2 WorldToCanvasAnchored(Vector3 worldPos)
        {
            if (_canvasRect == null || _camera == null)
                return Vector2.zero;

            Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);

            // RenderModeごとにRectTransformUtility側の引数を変える
            Camera uiCam = null;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera ||
                _canvas.renderMode == RenderMode.WorldSpace)
            {
                // ScreenSpace-Camera / WorldSpace のときは canvas.worldCamera を使う
                uiCam = _canvas.worldCamera != null ? _canvas.worldCamera : _camera;
            }
            else
            {
                // Overrayの場合そのまま返す
                //return screenPos;
                // Overlayの場合
                return screenPos - new Vector3(Screen.width / 2f, Screen.height / 2f);
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvasRect,
                    (Vector2)screenPos,
                    uiCam,
                    out Vector2 localPos))
            {
                return localPos;
            }

            return Vector2.zero;
        }

        /// <summary>
        /// 逆変換：Canvas(anchoredPosition)→World。UI上の点から3D空間の平面(Z)へ。
        /// 使用する場合は targetZ (ワールドのZ) を指定する。
        /// </summary>
        public Vector3 CanvasAnchoredToWorld(Vector2 anchoredPos, float targetZ = 0f)
        {
            if (_canvasRect == null || _camera == null)
                return Vector3.zero;

            // RectTransform（Canvas空間）の anchoredPosition をワールドに変換
            Vector3 worldFromCanvas = _canvasRect.TransformPoint(anchoredPos);

            // その Canvas 上のワールド座標をカメラから見たスクリーン座標に変換
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_camera, worldFromCanvas);

            // スクリーン座標をワールド空間のZ平面上に投影
            Ray ray = _camera.ScreenPointToRay(screenPoint);
            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, targetZ));
            if (plane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);

            return Vector3.zero;
        }

        public MassInfo GetInfoFromPosition(Vector3 position) => _board.GetMassFromVector3(position);
    }
}
