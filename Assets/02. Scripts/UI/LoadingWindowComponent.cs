using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingWindowComponent : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;

        public Slider ProgressBar
        {
            get
            {
                Debug.Assert(_progressBar);
                return _progressBar;
            }
            private set => _progressBar = value;
        }

        [SerializeField] private TextMeshProUGUI _loadSceneNameText;

        public TextMeshProUGUI LoadSceneNameText
        {
            get
            {
                Debug.Assert(_loadSceneNameText);
                return _loadSceneNameText;
            }
            private set => _loadSceneNameText = value;
        }

        public MonoBehaviour CoroutineRunner => this;

        public void ShowWindow(bool show)
        {
            Debug.Assert(FindObjectsOfType<LoadingWindowComponent>(true).Length == 1);
            gameObject.SetActive(show);
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}