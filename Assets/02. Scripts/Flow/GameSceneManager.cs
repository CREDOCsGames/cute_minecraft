using System;
using UnityEngine.SceneManagement;

namespace Flow
{
    public static class GameSceneManager
    {
        public static string CurrentMap { get; private set; } = MAP_EMPTY;
        private static readonly string MAP_EMPTY = "EMPTY";
        private static readonly string MAP_INTRO = "Intro";
        private static readonly string MAP_STAGE = "Stage";
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
                CurrentMap = MAP_INTRO;
                OnLoadedFirstTime?.Invoke(scene);
            }
            else if (IsStage(scene.name))
            {
                CurrentMap = MAP_STAGE;
                OnLoadedGameScene?.Invoke(scene);
            }
            else
            {
                CurrentMap = MAP_EMPTY;
            }
        }
        private static bool IsStage(string sceneName)
        {
            return sceneName.Contains(MAP_STAGE);
        }
    }

}
