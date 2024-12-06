using System.Collections;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Flow
{
    public class StageLoader : ILevelLoader
    {
        public WorkState State { get; private set; }
        private StageManager _stages;

        public StageManager Stages
        {
            get
            {
                if (_stages == null)
                {
                    _stages = Resources.Load<StageManager>("Stage/StageManager");
                }

                return _stages;
            }
        }

        private Slider _progressBar => LoadingWindow.ProgressBar;
        private TextMeshProUGUI _title => LoadingWindow.LoadSceneNameText;
        private LoadingWindowComponent _loadingWindow;

        private LoadingWindowComponent LoadingWindow
        {
            get
            {
                if (_loadingWindow != null)
                {
                    return _loadingWindow;
                }

                _loadingWindow = UIWindowContainer.GetLoadingWindow();
                _loadingWindow.ShowWindow(false);

                return _loadingWindow;
            }
        }

        private MonoBehaviour _coroutineRunner => LoadingWindow.CoroutineRunner;

        public void LoadNext()
        {
            State = WorkState.Action;
            var sceneName = Stages.Stage;
            Stages.NextStage();
            LoadingWindow.ShowWindow(true);
            _coroutineRunner.StartCoroutine(LoadSceneProcess(sceneName));
        }

        private IEnumerator LoadSceneProcess(string sceneName)
        {
            _title.text = sceneName;
            var timer = 0f;
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress < 0.9f)
                {
                    _progressBar.normalizedValue = asyncOperation.progress;
                }
                else
                {
                    timer += Time.unscaledDeltaTime;
                    _progressBar.normalizedValue = Mathf.Lerp(0.9f, 1f, timer);
                    if (_progressBar.normalizedValue >= 1f)
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