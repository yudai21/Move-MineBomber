using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeSystem : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private Ease _easeType = Ease.Unset;
    public static string UnLoadSceneName;
    public static string LoadSceneName;

    void Start()
    {
        Debug.Log("load:" + LoadSceneName);
        Debug.Log("unload:" + UnLoadSceneName);
        _fadeImage.DOFade(1f, _fadeDuration).SetEase(_easeType).OnComplete(() =>
        {
            if (SceneLoadRequest.AllSceneDelete)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene.name != "Fade")
                    {
                        SceneManager.UnloadSceneAsync(scene);
                    }
                }
            }
            else
            {
                SceneManager.UnloadSceneAsync(UnLoadSceneName);
            }
            SceneManager.LoadSceneAsync(LoadSceneName, LoadSceneMode.Additive).completed += (AsyncOperation obj) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadSceneName));
            };
            _fadeImage.DOFade(0f, _fadeDuration).SetEase(_easeType).OnComplete(() =>
            {
                SceneManager.UnloadSceneAsync("Fade");
                SceneLoadRequest.AllSceneDelete = false;
            });

            
        });
    }


}
