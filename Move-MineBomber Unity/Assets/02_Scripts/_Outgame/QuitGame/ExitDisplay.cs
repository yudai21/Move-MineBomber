using UnityEngine;

public class ExitDisplay : MonoBehaviour
{
    [SerializeField] private GameObject endCanvas;
    private KeyCode activationKey = KeyCode.Escape;
    void Start()
    {
        endCanvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(activationKey))
        {
            if(endCanvas!=null)
            {
                endCanvas.SetActive(true);
            }
        }
    }
}
