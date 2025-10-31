using UnityEngine;
using TMPro;
using Zenject;
using Bomb.Managers.Timers;
using UniRx;

namespace Bomb.Views
{
    public class GameTimerView : MonoBehaviour
    {
        [Inject] private ITimer _timer;
        [SerializeField] TMP_Text _text;

        private void Start()
        {
            _timer.ReactiveProperty.Subscribe(x => _text.SetText($"{Mathf.RoundToInt(x)}")).AddTo(this);
        }
    }
}