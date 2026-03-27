using Bomb.Boards;
using Bomb.Managers;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bomb.Views
{
    public class MassViewer : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Canvas _uiCanvas; // UI配置先（注入）
        [SerializeField] private RectTransform _uiRoot; // UIの親（注入）

        // キャッシュ
        private TMP_Text _countText;
        private MaterialPropertyBlock _mpb;

#if UNITY_EDITOR
        [Header("Info")]
        [Tooltip("インスペクター監視用")]
        [SerializeField] private MassInfo _massInfo;
#endif

        // ===== 公開API =====
        public void UpdateMass(MassInfo info)
        {
            EnsureBlocks();

            // 1) 色の決定（優先度：未開示 > 爆弾 > 空 > その他）
            var color = PickColor(info.type);
            _mpb.SetColor("_Color", color);
            _spriteRenderer.SetPropertyBlock(_mpb);

            // 2) 数字の表示（0は非表示）
            UpdateNumber(info);

#if UNITY_EDITOR
            _massInfo = info;
#endif
        }

        // ===== 内部実装 =====

        private void UpdateNumber(MassInfo info)
        {
            if (info.aroundBombCount > 0 && (info.type & MassType.Opened) != 0)
            {
                ActiveText();
                // 表示更新
                _countText.SetText(info.aroundBombCount.ToString());
            }
            else if ((info.type & MassType.Flagged) != 0)
            {
                ActiveText();
                // 表示更新
                _countText.SetText("F");
            }
            else
            {
                if (_countText != null)
                {
                    GameSceneRooter.instance.View.Pool.Release(_countText);
                    _countText = null;
                }
            }
        }

        private void ActiveText()
        {
            if (_countText == null)
            {
                var obj = GameSceneRooter.instance.View.Pool.Get();
                _countText = obj;
                if (_uiRoot != null)
                    _countText.rectTransform.SetParent(_uiRoot, worldPositionStays: false);
            }
            // 位置更新（ワールド→UI）
            Vector2 uiPos = GameSceneRooter.instance.View.WorldToCanvasAnchored(transform.position);
            _countText.rectTransform.anchoredPosition = uiPos;

            _countText.gameObject.SetActive(true);
        }
        private static Color PickColor(MassType type)
        {
            // 未開示（Closed）
            if ((type & MassType.Opened) == 0)
                return Color.green;

            // 開示済み
            if ((type & MassType.Bomb) != 0)
                return Color.red;

            if ((type & MassType.Empty) != 0)
                return Color.blue;

            return Color.green;
        }

        private void EnsureBlocks()
        {
            if (_mpb == null) _mpb = new MaterialPropertyBlock();
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.white;
            Handles.Label(transform.position, $"({_massInfo.x},{_massInfo.y})");
        }
#endif
    }
}
