using UnityEngine;
using TMPro;

public class HandleView : MonoBehaviour
{
    [SerializeField] private HandCountManager handCountManager;
    [SerializeField] private TextMeshProUGUI handleText;


    // Update is called once per frame
    void Update()
    {
        handleText.SetText($"Moves: {handCountManager.MaxMoves}");
    }
}
