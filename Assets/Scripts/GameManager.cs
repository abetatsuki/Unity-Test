using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float minimumLoadingScreenTime = 0.5f;

    private Coroutine loadingCoroutine;

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

        StartLoading(LoadSceneAsync(sceneName));
    }

    public void LoadScene(int sceneBuildIndex)
    {
        if (sceneBuildIndex < 0 || sceneBuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"LoadScene failed: invalid build index {sceneBuildIndex}.");
            return;
        }

        StartLoading(LoadSceneAsync(sceneBuildIndex));
    }

    private void StartLoading(IEnumerator loadRoutine)
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }

        loadingCoroutine = StartCoroutine(loadRoutine);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        yield return RunLoadingSequence(asyncOperation);
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);
        yield return RunLoadingSequence(asyncOperation);
    }

    private IEnumerator RunLoadingSequence(AsyncOperation asyncOperation)
    {
        if (asyncOperation == null)
        {
            yield break;
        }

        SetLoadingProgress(0f);
        SetLoadingScreenActive(true);

        asyncOperation.allowSceneActivation = false;
        float elapsedTime = 0f;

        while (asyncOperation.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            SetLoadingProgress(progress);
            yield return null;
        }

        SetLoadingProgress(1f);

        while (elapsedTime < minimumLoadingScreenTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        SetLoadingScreenActive(false);
        loadingCoroutine = null;
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
