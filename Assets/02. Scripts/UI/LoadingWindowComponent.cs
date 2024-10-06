using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindowComponent : MonoBehaviour
{
    [SerializeField] Slider mProgressBar;

    public Slider ProgressBar
    {
        get
        {
            Debug.Assert(mProgressBar);
            return mProgressBar;
        }
        private set => mProgressBar = value;
    }

    [SerializeField] TextMeshProUGUI mLoadSceneNameText;

    public TextMeshProUGUI LoadSceneNameText
    {
        get
        {
            Debug.Assert(mLoadSceneNameText);
            return mLoadSceneNameText;
        }
        private set => mLoadSceneNameText = value;
    }

    public MonoBehaviour CoroutineRunner => this;

    public void ShowWindow(bool show)
    {
        Debug.Assert(FindObjectsOfType<LoadingWindowComponent>(true).Length == 1);
        gameObject.SetActive(show);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}