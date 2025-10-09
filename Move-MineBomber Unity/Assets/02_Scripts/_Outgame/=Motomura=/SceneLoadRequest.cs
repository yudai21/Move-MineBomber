using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneLoadRequest : MonoBehaviour
{
    public static bool AllSceneDelete = false;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync("Fade", LoadSceneMode.Additive);
        FadeSystem.LoadSceneName = sceneName;
        FadeSystem.UnLoadSceneName = this.gameObject.scene.name;
    }
    public void Button_Interactable()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
    }

    public void Return()
    {
        AllSceneDelete = true;
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("リトライ" + scene.name);
        FadeSystem.LoadSceneName = scene.name;

    }
}
