using UnityEngine;

[CreateAssetMenu(fileName = "UIWindows", menuName = "Custom/UIWindows")]
public class UIWindowPrefabs : ScriptableObject
{
    [SerializeField] LoadingWindowComponent mLoadingWindowPrefab;
    public LoadingWindowComponent LoadingWindowPrefab => mLoadingWindowPrefab;
}

public static class UIWindowContainer
{
    static readonly UIWindowPrefabs mPrefabs = Resources.Load<UIWindowPrefabs>("UIWindows");
    static LoadingWindowComponent mLoadingWindowInstance;

    public static LoadingWindowComponent GetLoadingWindow()
    {
        Debug.Assert(mPrefabs, "Resource not found: Resources/UIWindows");
        if (mLoadingWindowInstance == null)
        {
            mLoadingWindowInstance = Object.Instantiate(mPrefabs.LoadingWindowPrefab);
        }

        return mLoadingWindowInstance;
    }
}