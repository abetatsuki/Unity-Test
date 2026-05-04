using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float minimumLoadingScreenTime = 0.5f;

    private void Awake()
    {
        SetLoadingScreenActive(false);
        SetLoadingProgress(0f);
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("LoadScene failed: sceneName is empty.");
            return;
        }

        LoadSceneAsync(sceneName).Forget();
    }

    public void LoadScene(int sceneBuildIndex)
    {
        if (sceneBuildIndex < 0 || sceneBuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"LoadScene failed: invalid build index {sceneBuildIndex}.");
            return;
        }

        LoadSceneAsync(sceneBuildIndex).Forget();
    }

    private async UniTask LoadSceneAsync(string sceneName)
    {
        await RunLoadingSequence(SceneManager.LoadSceneAsync(sceneName));
    }

    private async UniTask LoadSceneAsync(int sceneBuildIndex)
    {
        await RunLoadingSequence(SceneManager.LoadSceneAsync(sceneBuildIndex));
    }

    private async UniTask RunLoadingSequence(AsyncOperation asyncOperation)
    {
        if (asyncOperation == null)
        {
            return;
        }

        SetLoadingProgress(0f);
        SetLoadingScreenActive(true);

        asyncOperation.allowSceneActivation = false;
        float elapsedTime = 0f;
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();

        while (asyncOperation.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            SetLoadingProgress(Mathf.Clamp01(asyncOperation.progress / 0.9f));
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
        }

        SetLoadingProgress(1f);

        if (elapsedTime < minimumLoadingScreenTime)
        {
            int delayMilliseconds = Mathf.CeilToInt((minimumLoadingScreenTime - elapsedTime) * 1000f);
            await UniTask.Delay(delayMilliseconds, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
        }

        asyncOperation.allowSceneActivation = true;
        await asyncOperation.ToUniTask(cancellationToken: cancellationToken);

        SetLoadingScreenActive(false);
    }

    private void SetLoadingScreenActive(bool isActive)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(isActive);
        }
    }

    private void SetLoadingProgress(float progress)
    {
        if (loadingSlider != null)
        {
            loadingSlider.value = progress;
        }
    }
}
