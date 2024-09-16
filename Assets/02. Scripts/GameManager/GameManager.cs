using PlatformGame.Contents;
using PlatformGame.Contents.Puzzle;
using UnityEngine;

namespace PlatformGame.Manager
{
    public static class GameManager
    {
        public static Movie MovieCutscene { get; private set; } = new();
        public static Area PuzzleArea { get; private set; } = new();
        public static Area StageArea { get; private set; } = new();
        public static Selection Title { get; private set; } = new();
        public static Selection StageSelect { get; private set; } = new();
        public static Timer GameTimer { get; private set; } = new();

        static GameManager()
        {
            PuzzleArea.OnEnterEvent += PuzzlePiece.EnablePieceInArea;
            PuzzleArea.OnClearEvent += PuzzlePiece.DisablePieceInArea;
            PuzzleArea.OnExitEvent += PuzzlePiece.DisablePieceInArea;

            Title.OnEnterEvent += Title.Skip;

            StageArea.OnClearEvent += (area) => StageManager.Instance.ClearCurrentStage();

            ContentsLoader.OnStartLoad += GameTimer.Stop;
            ContentsLoader.OnLoaded += GameTimer.Start;

            MovieCutscene.OnPlayEvent += PlayerCharacterManager.Instance.ReleaseController;
            MovieCutscene.OnEndEvent += PlayerCharacterManager.Instance.ControlDefaultCharacter;
            ContentsLoader.OnStartLoad += PlayerCharacterManager.Instance.ReleaseController;
            ContentsLoader.OnLoaded += PlayerCharacterManager.Instance.ControlDefaultCharacter;

            PuzzleArea.OnEnterEvent += (x) => AreaManager.DisConnect();
            PuzzleArea.OnClearEvent += (b) => AreaManager.Connect();

            ContentsLoader.SetLoaderType(LoaderType.LevelLoader);
            ContentsLoader.LoadContents();

            Debug.Log("Run Game");
        }
    }
}