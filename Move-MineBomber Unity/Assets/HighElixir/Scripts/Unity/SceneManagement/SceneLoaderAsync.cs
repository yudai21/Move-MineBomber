using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HighElixir.Unity.SceneManagement
{
    public static class SceneLoaderAsync
    {
        public static async Task LoadByBuildIdxAsync(int buildIdx, bool autoUnload = true, CancellationToken token = default, IProgress<float> progress = null)
        {
            await SceneLoaderAsyncInternal.SceneLoaderAsync(
                SceneManager.LoadSceneAsync(buildIdx, LoadSceneMode.Additive),
                () => SceneManager.GetSceneByBuildIndex(buildIdx),
                token,
                progress
            );
        }

        public static async Task LoadSceneWithManagerAsync(int buildIdx, bool autoUnload = true, CancellationToken token = default, IProgress<float> progress = null)
        {
            if (ManagerSceneHolder.TryGetManagerScene(out var m))
            {
                var s = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(m);
                _ = SceneManager.UnloadSceneAsync(s);
            }
            await LoadByBuildIdxAsync(buildIdx, autoUnload, token, progress);
        }
        public static async Task LoadByNameAsync(string name, bool autoUnload = true, CancellationToken token = default, IProgress<float> progress = null)
        {
            await SceneLoaderAsyncInternal.SceneLoaderAsync(
                SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive),
                () => SceneManager.GetSceneByName(name),
                token,
                progress
            );
        }

        public static async Task LoadSceneWithManagerAsync(string name, bool autoUnload = true, CancellationToken token = default, IProgress<float> progress = null)
        {
            if (ManagerSceneHolder.TryGetManagerScene(out var m))
            {
                var s = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(m);
                _ = SceneManager.UnloadSceneAsync(s);
            }
            await LoadByNameAsync(name, autoUnload, token, progress);
        }
    }
}