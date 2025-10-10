using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
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
            GameManager.Instance.CurrentGameState = GameState.Paused;
        }
        else
        {
            //Debug.Log("NotActive");
            GameManager.Instance.CurrentGameState = GameState.Playing;
        }
    }
}
