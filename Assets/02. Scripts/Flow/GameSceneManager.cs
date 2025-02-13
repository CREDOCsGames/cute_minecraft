using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow
{

    public static class GameSceneManager
    {
        public static string CurrentMap { get; private set; } = MAP_EMPTY;
        private static readonly string MAP_EMPTY = "EMPTY";
        private static readonly string MAP_INTRO = "Intro";
        private static readonly string MAP_CUTSCENE = "CutScene";
        private static readonly string MAP_STAGE = "Stage";
        private static bool _isFirstLoad = true;
        public static event Action<Scene> OnLoadedGameScene;
        public static event Action<Scene> OnLoadedFirstTime;
        public static event Action<Scene> OnLoadedMainMenuScene;
        public static event Action<Scene> OnLoadedQuitGameScene;

        static GameSceneManager()
        {
            SceneManager.sceneLoaded += OnLoadedScene;
            SceneManager.sceneUnloaded += OnUnloadedScene;
        }

        public static void UpdateState()
        {
            var scene = SceneManager.GetActiveScene();
            var sceneCount = SceneManager.loadedSceneCount;
            if (scene.name.Equals("Entry") && sceneCount == 1)
            {
                OnLoadedScene(scene, LoadSceneMode.Single);
            }
        }
        private static void OnUnloadedScene(Scene scene)
        {
            if (scene.isSubScene)
            {
                CurrentMap = IsStage(SceneManager.GetActiveScene().name) ? MAP_STAGE : MAP_EMPTY;
            }
            else
            {
                CurrentMap = MAP_EMPTY;
            }
        }
        private static void OnLoadedScene(Scene scene, LoadSceneMode mode)
        {
            if (_isFirstLoad)
            {
                _isFirstLoad = false;
                CurrentMap = MAP_INTRO;
                OnLoadedFirstTime?.Invoke(scene);
            }
            else if (mode is LoadSceneMode.Additive)
            {
                CurrentMap = MAP_CUTSCENE;
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
            return sceneName.ToUpper().Contains(MAP_STAGE.ToUpper());
        }
    }

}
