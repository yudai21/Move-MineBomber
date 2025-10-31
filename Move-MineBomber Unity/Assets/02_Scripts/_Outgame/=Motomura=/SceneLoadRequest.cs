using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneLoadRequest : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        LoaderWithFade.LoadWithFade(sceneName);
    }
    public void Button_Interactable()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
    }

    public void Return()
    {
        Scene scene = SceneManager.GetActiveScene();
        //Debug.Log("リトライ" + scene.name);
        FadeSystem.LoadSceneName = scene.name;
    }

    public void RetryGame()
    {
        LoaderWithFade.LoadWithFade("GamePlay");
    }
}
