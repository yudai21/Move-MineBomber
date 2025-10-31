using Bomb.Boards;
using Bomb.Datas;
using Bomb.Inputs;
using Bomb.Managers.Timers;
using Bomb.Repository;
using Bomb.Views;
using HighElixir;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Bomb.Managers
{
    [DefaultExecutionOrder(-99)]
    public class GameSceneRooter : MonoBehaviour
    {
        // Injects
        [Inject] private ITimer _timer;
        [Inject] private GameDataRepository _dataRepository;
        [Inject] private BoardController _board;
        [Inject] private InputController _inputController;

        [Header("Reference")]
        [SerializeField] private TextPool _textPool;

        [SerializeField]
        private GameRule _rule;
        public GameRule Rule => _rule;
        public BoardController Board => _board;
        public InputController InputController => _inputController;

        public bool GameAwaked { get; private set; } = false;
        public UnityEvent OnGameInvoked { get; private set; } = new();
        public void Invoke()
        {
            try
            {
                _board.Invoke();
                OnGameInvoked?.Invoke();
                GameAwaked = true;
            }
            catch (System.Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public void SetRule(GameRule rule)
        {
            _rule = rule;
        }

        private void Update()
        {
            var dT = Time.deltaTime;
        }

        protected void Awake()
        {
            GameAwaked = false;
            //Invoke();
        }

        private void Start()
        {
            _dataRepository.CurrentRule = _rule;
            Invoke();
        }
        private void OnApplicationQuit()
        {
            _board.Dispose();
        }
    }
}