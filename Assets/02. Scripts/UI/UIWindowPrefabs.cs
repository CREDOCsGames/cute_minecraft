using UnityEngine;

[CreateAssetMenu(fileName = "UIWindows", menuName = "Custom/UIWindows")]
public class UIWindowPrefabs : ScriptableObject
{
    [SerializeField] LoadingWindow mLoadingWindowPrefab;
    public LoadingWindow LoadingWindowPrefab => mLoadingWindowPrefab;
}

public static class UIWindowContainer
{
    static readonly UIWindowPrefabs mPrefabs = Resources.Load<UIWindowPrefabs>("UIWindows");
    static LoadingWindow mLoadingWindowInstance;

    public static LoadingWindow GetLoadingWindow()
    {
        Debug.Assert(mPrefabs, "Resource not found: Resources/UIWindows");
        if (mLoadingWindowInstance == null)
        {
            mLoadingWindowInstance = Object.Instantiate(mPrefabs.LoadingWindowPrefab);
        }

        return mLoadingWindowInstance;
    }
}