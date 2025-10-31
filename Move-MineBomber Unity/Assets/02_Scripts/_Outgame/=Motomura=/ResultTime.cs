using Bomb.Repository;
using TMPro;
using UnityEngine;
using Zenject;

namespace Bomb.Views {
    public class ResultTime : MonoBehaviour
    {
        [Inject] private GameDataRepository _gameDataRepository;
        [SerializeField] private TextMeshProUGUI _timeText;
        void Start()
        {
            _timeText.SetText($"タイム\n {_gameDataRepository.CurrentTime.ToString("F0")}");
        }
    }
}