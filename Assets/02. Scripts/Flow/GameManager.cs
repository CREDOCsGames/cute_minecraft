using Character;
using Sound;
using UnityEngine;

namespace Flow
{
    public static class GameManager
    {
        public static Movie MovieCutscene { get; private set; } = new();
        public static Area PuzzleArea { get; private set; } = new();
        public static Area StageArea { get; private set; } = new();
        public static Selection Title { get; private set; } = new();
        public static Selection StageSelect { get; private set; } = new();
        private static Timer _gameTimer { get; set; } = new();
        private static bool _visitTitle;

        static GameManager()
        {

            Title.OnEnterEvent += () =>
            {
                if (_visitTitle) Title.Skip();
                _visitTitle = true;
            };

            StageArea.OnClearEvent += (area) => StageManager.Instance.ClearCurrentStage();
            StageArea.OnEnterEvent += (area) =>
            {
                SoundManagerComponent.Instance.PlayMusic($"Stage_{StageManager.Instance.StageIndex.x}");
            };


            ContentsLoader.OnStartLoad += _gameTimer.Stop;
            ContentsLoader.OnLoaded += _gameTimer.Start;

            MovieCutscene.OnPlayEvent += PlayerCharacterManager.Instance.ReleaseController;
            MovieCutscene.OnEndEvent += PlayerCharacterManager.Instance.ControlDefaultCharacter;
            ContentsLoader.OnStartLoad += PlayerCharacterManager.Instance.ReleaseController;
            ContentsLoader.OnLoaded += PlayerCharacterManager.Instance.ControlDefaultCharacter;

            ContentsLoader.SetLoaderType(LoaderType.LevelLoader);
            ContentsLoader.LoadContents();

            Debug.Log("Run Game");
        }
    }
}