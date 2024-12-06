using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "UIWindows", menuName = "Custom/UIWindows")]
    public class UIWindowPrefabs : ScriptableObject
    {
        [SerializeField] private LoadingWindowComponent _loadingWindowPrefab;
        public LoadingWindowComponent LoadingWindowPrefab => _loadingWindowPrefab;
    }

    public static class UIWindowContainer
    {
        private static readonly UIWindowPrefabs _prefabs = Resources.Load<UIWindowPrefabs>("UIWindows");
        private static LoadingWindowComponent _loadingWindowInstance;

        public static LoadingWindowComponent GetLoadingWindow()
        {
            Debug.Assert(_prefabs, "Resource not found: Resources/UIWindows");
            if (_loadingWindowInstance == null)
            {
                _loadingWindowInstance = Object.Instantiate(_prefabs.LoadingWindowPrefab);
            }

            return _loadingWindowInstance;
        }
    }
}