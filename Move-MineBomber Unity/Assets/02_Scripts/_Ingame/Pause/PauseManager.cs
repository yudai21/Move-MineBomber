using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseManager : MonoBehaviour
{
    [Inject] private GameStateHolder _state;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button pauseButton;

    private bool isActive;

    void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
        pauseButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        isActive = !isActive;
        pauseCanvas.SetActive(isActive);
        //Debug.Log("Click");

        if (isActive)
        {
            //Debug.Log("Active");
            _state.UpdateState(GameState.Paused);
        }
        else
        {
            //Debug.Log("NotActive");
            _state.UpdateState(GameState.Playing);
        }
    }
}
