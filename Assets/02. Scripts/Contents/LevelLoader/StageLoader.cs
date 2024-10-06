using PlatformGame.Contents.Loader;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlatformGame.Contents
{
    public class StageLoader : ILevelLoader
    {
        public WorkState State { get; private set; }
        StageManager mStages;
        StageManager Stages
        {
            get
            {
                if (mStages == null)
                {
                    mStages = Resources.Load<StageManager>("Stage/StageManager");
                }
                return mStages;
            }
        }
        Slider mProgressBar => LoadingWindow.ProgressBar;
        TextMeshProUGUI mTitle => LoadingWindow.LoadSceneNameText;
        LoadingWindowComponent mLoadingWindow;
        LoadingWindowComponent LoadingWindow
        {
            get
            {
                if (mLoadingWindow == null)
                {
                    mLoadingWindow = UIWindowContainer.GetLoadingWindow();
                    mLoadingWindow.ShowWindow(false);
                }
                return mLoadingWindow;
            }
        }
        MonoBehaviour mCoroutineRunner => LoadingWindow.CoroutineRunner;

        public void LoadNext()
        {
            State = WorkState.Action;
            var sceneName = Stages.Stage;
            Stages.NextStage();
            LoadingWindow.ShowWindow(true);
            mCoroutineRunner.StartCoroutine(LoadSceneProcess(sceneName));
        }

        IEnumerator LoadSceneProcess(string sceneName)
        {
            mTitle.text = sceneName;
            var timer = 0f;
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress < 0.9f)
                {
                    mProgressBar.normalizedValue = asyncOperation.progress;
                }
                else
                {
                    timer += Time.unscaledDeltaTime;
                    mProgressBar.normalizedValue = Mathf.Lerp(0.9f, 1f, timer);
                    if (mProgressBar.normalizedValue >= 1f)
                    {
                        asyncOperation.allowSceneActivation = true;
                        break;
                    }
                }
                yield return new WaitForSeconds(1);
            }

            LoadingWindow.ShowWindow(false);
            State = WorkState.Ready;
        }
    }
}