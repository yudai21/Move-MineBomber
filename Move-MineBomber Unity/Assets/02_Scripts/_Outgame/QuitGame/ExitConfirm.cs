using UnityEngine;

public class ExitConfirm : MonoBehaviour
{
    [SerializeField] private GameObject endCanvas;

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
}
