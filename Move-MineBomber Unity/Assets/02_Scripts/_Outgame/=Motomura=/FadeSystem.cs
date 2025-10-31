using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using HighElixir.Unity.SceneManagement;
using Cysharp.Threading.Tasks;
using HighElixir.Unity.Tasks;

public static class LoaderWithFade
{
    internal static int loaded = 0;
    public static void LoadWithFade(string name)
    {
        if (loaded > 0) return;
        loaded++;
        FadeSystem.LoadSceneName = name;
        SceneLoaderAsync.LoadByNameAsync("Fade", autoUnload: false).AsUniTask().Forget();
    }
}

public class FadeSystem : MonoBehaviour, ISceneReceiver
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private Ease _easeType = Ease.Unset;

    internal static string LoadSceneName;

    public void Receive(SceneDataContainer container)
    {
        var load = LoadSceneName;
        UniTask.Create(async () =>
        {
            await UniTask.SwitchToMainThread();
            await UniTask.WhenAll(
                _fadeImage.DOFade(1f, _fadeDuration).SetEase(_easeType).AsyncWaitForCompletion().AsUniTask(),
                SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive).AsTask().AsUniTask());

            await _fadeImage.DOFade(0f, _fadeDuration).SetEase(_easeType).AsyncWaitForCompletion().AsUniTask();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(load));
            await SceneManager.UnloadSceneAsync("Fade");
            LoaderWithFade.loaded--;
        });
    }
}
