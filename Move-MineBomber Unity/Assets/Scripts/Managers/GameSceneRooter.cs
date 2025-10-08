using Bomb.Datas;
using Bomb.Views;
using UnityEngine;
using HighElixir;
using Bomb.Inputs;
using HighElixir.Timers;
using HighElixir.Pool;

namespace Bomb.Managers
{
    public class GameSceneRooter : SingletonBehavior<GameSceneRooter>
    {
        [Header("Reference")]
        [SerializeField] private BoardViewer _viewer;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextPool _textPool;
        private GameSceneManager _gameManager = new();
        private ViewObjRooter _viewObjRooter;
        private InputController _inputController;
        private Timer _timer;
#if UNITY_EDITOR
        [SerializeField]
#endif
        private GameRule _rule;
        public ViewObjRooter View => _viewObjRooter;
        public InputController InputController => _inputController;
        public GameSceneManager Manager => _gameManager;
        public Timer Timer => _timer;
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
            var dT = Time.deltaTime;
            _viewObjRooter.Update(dT);
            _timer.Update(dT);
        }
#if UNITY_EDITOR
        protected override void Awake()
        {
            base.Awake();
            _timer = new Timer(typeof(GameSceneRooter));
            _viewObjRooter = new(_viewer, _textPool.Pool);
            _viewObjRooter.SetCamera(Camera.main);
            _viewObjRooter.SetCanvas(_canvas);
            Invoke();
        }
#endif
    }
}