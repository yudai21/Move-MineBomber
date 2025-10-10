using Bomb.Datas;
using Bomb.Views;
using UnityEngine;
using HighElixir;
using Bomb.Inputs;
using HighElixir.Pool;
using UnityEngine.Events;
using System;

namespace Bomb.Managers
{
    [DefaultExecutionOrder(-99)]
    public class GameSceneRooter : SingletonBehavior<GameSceneRooter>
    {
        [Header("Reference")]
        [SerializeField] private BoardViewer _viewer;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextPool _textPool;
        private GameSceneManager _gameManager = new();
        private ViewObjRooter _viewObjRooter;
        private InputController _inputController;

        [SerializeField]
        private GameRule _rule;
        public GameRule Rule => _rule;
        public ViewObjRooter View => _viewObjRooter;
        public InputController InputController => _inputController;
        public GameSceneManager Manager => _gameManager;

        public bool GameAwaked { get; private set; } = false;
        public UnityEvent OnGameInvoked { get; private set; } = new();
        public void Invoke()
        {
            Debug.Log("AAAAAA");
            try
            {
                _gameManager.Invoke(_rule);
                _viewObjRooter.Invoke(_gameManager.Board);
                _inputController = new InputController(this);
                OnGameInvoked?.Invoke();
                GameAwaked = true;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        public void SetRule(GameRule rule)
        {
            _rule = rule;
        }

        private void Update()
        {
            var dT = Time.deltaTime;
            _viewObjRooter.Update(dT);
        }

        protected override void Awake()
        {
            GameAwaked = false;
            base.Awake();
            _viewObjRooter = new(_viewer, _textPool.Pool);
            _viewObjRooter.SetCamera(Camera.main);
            _viewObjRooter.SetCanvas(_canvas);
            Invoke();
        }
        private void OnApplicationQuit()
        {
            _gameManager.Dispose();
        }
    }
}