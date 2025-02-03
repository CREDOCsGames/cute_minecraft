using Cinema;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flow
{
    public static class GameManager
    {
        public static string CurrentMap { get; private set; } = "EMPTY";
        public static Timer Timer { get; private set; } = new();

        static GameManager()
        {
            AddEventToLoadScene();
            AddEventToIntroMovie();
            AddEventToEnterGameMovie();
            AddEventToEnterBossMovie();
            AddEventToClearGameMovie();
            AddEventToMovieSkipUI();
            Debug.Log("Run Game");
        }
        private static void AddEventToLoadScene()
        {
            GameSceneManager.OnLoadedFirstTime += (s) => Movie.DoPlay(Movie.INTRO);
            GameSceneManager.OnLoadedGameScene += (s) => Movie.DoPlay(Movie.ENTER_GAME);
        }
        private static void AddEventToIntroMovie()
        {
            Movie.INTRO.OnPlay += Character.StopUpdate;
            Movie.INTRO.OnPlay += UIResources.CutSceneUI.Instance.OnUI;
            Movie.INTRO.OnEnd += Character.StartUpdate;
            Movie.INTRO.OnEnd += UIResources.CutSceneUI.Instance.CloseUI;
            Movie.INTRO.OnEnd += () => SceneManager.LoadScene("Stage01");
        }
        private static void AddEventToEnterGameMovie()
        {
            Movie.ENTER_GAME.OnPlay += Character.StopUpdate;
            Movie.ENTER_GAME.OnPlay += UIResources.CutSceneUI.Instance.OnUI;
            Movie.ENTER_GAME.OnEnd += Character.StartUpdate;
            Movie.ENTER_GAME.OnEnd += UIResources.CutSceneUI.Instance.CloseUI;
        }
        private static void AddEventToEnterBossMovie()
        {
            Movie.ENTER_BOSS.OnPlay += Character.StopUpdate;
            Movie.ENTER_BOSS.OnPlay += UIResources.CutSceneUI.Instance.OnUI;
            Movie.ENTER_BOSS.OnEnd += Character.StartUpdate;
            Movie.ENTER_BOSS.OnEnd += UIResources.CutSceneUI.Instance.CloseUI;
        }
        private static void AddEventToClearGameMovie()
        {
            Movie.INTRO.OnPlay += Character.StopUpdate;
            Movie.INTRO.OnPlay += UIResources.CutSceneUI.Instance.OnUI;
            Movie.INTRO.OnEnd += Character.StartUpdate;
            Movie.INTRO.OnEnd += UIResources.CutSceneUI.Instance.CloseUI;
        }
        private static void AddEventToMovieSkipUI()
        {
            UIResources.CutSceneUI.Instance.OnSkip += Movie.DoSkip;
        }

    }
}