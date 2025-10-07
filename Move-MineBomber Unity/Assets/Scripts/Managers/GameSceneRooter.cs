using Bomb.Datas;
using Bomb.Views;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using HighElixir;

namespace Bomb.Managers
{
    public class GameSceneRooter : SingletonBehavior<GameSceneRooter>
    {
        [Header("Reference")]
        [SerializeField] private BoardViewer _viewer;
        private GameSceneManager _gameManager = new();
#if UNITY_EDITOR
        [SerializeField]
#endif
        private GameRule _rule;

        public BoardViewer BoardViewer => _viewer;
        public void Invoke()
        {
            _gameManager.Invoke(_rule);
            _viewer.Invoke(_gameManager.Board);
        }

        public void SetRule(GameRule rule)
        {
            _rule = rule;
        }

        private void Update()
        {
            _viewer.BoardView();
        }
#if UNITY_EDITOR
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("AAAAA");
            Invoke();
        }
#endif
    }
}