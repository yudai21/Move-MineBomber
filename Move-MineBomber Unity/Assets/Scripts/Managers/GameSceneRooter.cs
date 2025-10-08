using Bomb.Datas;
using Bomb.Views;
using UnityEngine;
using HighElixir;
using Bomb.Inputs;

namespace Bomb.Managers
{
    public class GameSceneRooter : SingletonBehavior<GameSceneRooter>
    {
        [Header("Reference")]
        [SerializeField] private BoardViewer _viewer;
        [SerializeField] private Canvas _canvas;

        private GameSceneManager _gameManager = new();
        private ViewObjRooter _viewObjRooter;
        private InputController _inputController;
#if UNITY_EDITOR
        [SerializeField]
#endif
        private GameRule _rule;

        public ViewObjRooter View => _viewObjRooter;
        public InputController InputController => _inputController;
        public GameSceneManager Manager => _gameManager;
        public void Invoke()
        {
            _gameManager.Invoke(_rule);
            _viewObjRooter.Invoke(_gameManager.Board);
            _inputController = new InputController(this);
        }

        public void SetRule(GameRule rule)
        {
            _rule = rule;
        }

        private void Update()
        {
            _viewObjRooter.Update(Time.deltaTime);
        }
#if UNITY_EDITOR
        protected override void Awake()
        {
            base.Awake();
            _viewObjRooter = new(_viewer);
            _viewObjRooter.SetCamera(Camera.main);
            _viewObjRooter.SetCanvas(_canvas);
            Invoke();
        }
#endif
    }
}