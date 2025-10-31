using HighElixir.Unity.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HighElixir.Unity.SceneManagement
{
    internal static class SceneLoaderAsyncInternal
    {
        internal static async Task SceneLoaderAsync(AsyncOperation operation, Func<Scene> scene, CancellationToken token = default, IProgress<float> progress = null)
        {
            await GetProgress(operation, token, progress);
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            var from = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(scene());
            await FinalizeLoadAsync(from);
        }

        internal static async Task FinalizeLoadAsync(Scene scene)
        {
            try
            {
                await Resources.UnloadUnusedAssets().AsTask();
                await SceneManager.UnloadSceneAsync(scene);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Scene cleanup failed: {ex}");
            }
        }

        internal static async Task GetProgress(AsyncOperation operation, CancellationToken token, IProgress<float> progress)
        {
            while (!operation.isDone)
            {
                if (token.IsCancellationRequested) return;
                progress?.Report(operation.progress);
                await Task.Yield();
            }
        }
    }
}