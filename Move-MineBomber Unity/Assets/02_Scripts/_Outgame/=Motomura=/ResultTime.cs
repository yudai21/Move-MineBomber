using TMPro;
using UnityEngine;

public class ResultTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TimeText;
    void Start()
    {
        _TimeText.text = "タイム\n" + GameTimerManager.Instance.GetCurrentTime().ToString("F0");
    }
}
