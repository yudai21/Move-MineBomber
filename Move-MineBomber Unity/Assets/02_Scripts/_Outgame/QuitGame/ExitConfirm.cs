using Unity.VisualScripting;
using UnityEngine;

public class ExitConfirm : MonoBehaviour
{
    [SerializeField] private GameObject endCanvas;
    void Start()
    { 
        
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
