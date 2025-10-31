using Bomb.Boards;
using Bomb.Managers;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace Bomb.Views
{
    [RequireComponent(typeof(SpriteResolver))]
    public class MassViewer : MonoBehaviour
    {
        [Inject] private ViewObjRooter _viewObjRooter;
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteResolver _resolver;
        // キャッシュ
        private TMP_Text _countText;

#if UNITY_EDITOR
        [Header("Info")]
        [Tooltip("インスペクター監視用")]
        [SerializeField] private MassInfo _massInfo;
#endif

        // ===== 公開API =====
        public void UpdateMass(MassInfo info)
        {
            SetIcon(info);
            // 数字の表示（0は非表示）
            UpdateNumber(info);

#if UNITY_EDITOR
            _massInfo = info;
#endif
        }

        public void SetIcon(MassInfo info)
        {
            var t = info.type;
            if (!t.Has(MassType.Opened))
                Set("Hidden");
            else if (t.Has(MassType.Bomb))
                Set("Bomb");
            else
                Set("Opened");
        }
        // ===== 内部実装 =====
        private void Set(string label)
        {
            _resolver.SetCategoryAndLabel("Panel", label);
        }
        private void UpdateNumber(MassInfo info)
        {
            if (info.aroundBombCount > 0 && info.type.Has(MassType.Opened))
            {
                ActiveText();
                // 表示更新
                _countText.SetText(info.aroundBombCount.ToString());
            }
            else if (info.type.Has(MassType.Flagged))
            {
                ActiveText();
                // 表示更新
                _countText.SetText("F");
            }
            else
            {
                if (_countText != null)
                {
                    _viewObjRooter.Pool.Pool.Release(_countText);
                    _countText = null;
                }
            }
        }

        private void ActiveText()
        {
            if (_countText == null)
            {
                var obj = _viewObjRooter.Pool.Pool.Get();
                _countText = obj;
            }
            // 位置更新（ワールド→UI）
            Vector2 uiPos = _viewObjRooter.WorldToCanvasAnchored(transform.position);
            _countText.rectTransform.anchoredPosition = uiPos;

            _countText.gameObject.SetActive(true);
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
