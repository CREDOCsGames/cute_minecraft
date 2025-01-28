using System;
using UnityEngine.SceneManagement;

namespace Flow
{
    public static class GameSceneManager
    {
        private static bool _isFirstLoad = true;
        public static event Action<Scene> OnLoadedGameScene;
        public static event Action<Scene> OnLoadedFirstTime;
        public static event Action<Scene> OnLoadedMainMenuScene;
        public static event Action<Scene> OnLoadedQuitGameScene;

        static GameSceneManager()
        {
            SceneManager.sceneLoaded += OnLoadedScene;
        }
        private static void OnLoadedScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.isSubScene)
            {
            }
            else if (_isFirstLoad)
            {
                _isFirstLoad = false;
                OnLoadedFirstTime?.Invoke(scene);
            }
            else if (IsStage(scene.name))
            {
                OnLoadedGameScene?.Invoke(scene);
            }
        }
        private static bool IsStage(string sceneName)
        {
            return sceneName.Contains("Stage");
        }
    }

}
