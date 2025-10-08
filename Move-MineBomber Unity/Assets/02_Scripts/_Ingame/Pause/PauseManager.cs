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
    }

    void Update()
    {
        pauseButton.onClick.AddListener(() =>
        {

            pauseCanvas.SetActive(!isActive);

            if (!isActive)
            {
                Debug.Log("Active");
            }
            else
            {
                Debug.Log("NotActive");
            }
        });
    }

    public void OnClickButton()
    {
        isActive = !isActive;
    }
}
