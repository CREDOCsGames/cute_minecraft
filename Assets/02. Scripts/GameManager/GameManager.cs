using PlatformGame.Character.Controller;
using PlatformGame.Contents;
using PlatformGame.Contents.Loader;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using static PlatformGame.Character.Character;

namespace PlatformGame
{
    public class GameManager : MonoBehaviour
    {
        static GameManager mInstance;
        public static GameManager Instance
        {
            get
            {
                Debug.Assert(mInstance != null, $"Not found : Game Manager");
                return mInstance;
            }
            private set => mInstance = value;
        }
        public List<Character.Character> JoinCharacters => JoinCharactersController.Select(x => x.ControlledCharacter).ToList();
        float mLastSwapTime;
        Contents.ContentsLoader mContents;
        ActionController mCurrentController;
        List<ActionController> JoinCharactersController
        {
            get
            {
                var playerControllers = ActionController.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();
                Debug.Assert(playerControllers.Count > 0 && playerControllers.All(x => x),
                    $"No controllers with the {TAG_PLAYER} tag found.");
                return playerControllers;
            }
        }


        [Header("[Debug]")]
        [SerializeField, ReadOnly(false)] LoaderType mLoaderType;
        [SerializeField, ReadOnly(false)] bool mbGameStart;

        public void ExitGame()
        {
            Application.Quit();
        }

        public void LoadGame(float pauseTime = 0f)
        {
            PauseGame();
            Invoke(nameof(StopGame), pauseTime);
            Invoke(nameof(MoveNext), pauseTime);
        }

        void MoveNext()
        {
            Debug.Log("Load");
            mContents.LoadNextLevel();
        }

        void StartGame()
        {
            Debug.Log("Start Game");
            mbGameStart = true;
            ControlDefaultCharacter();
        }
        void StopGame()
        {
            Debug.Log("Stop Game");
            mbGameStart = false;
        }

        void PauseGame()
        {
            Debug.Log("Pause Game");
            ReleaseController();
        }

        void ResumeGame()
        {
            mbGameStart = true;
            ControlDefaultCharacter();
        }

        void ControlDefaultCharacter()
        {
            JoinCharactersController.ForEach(x => x.SetActive(false));
            var defaultCharacter = JoinCharactersController.First();
            ReplaceControlWith(defaultCharacter);
        }

        void ReplaceControlWith(ActionController controller)
        {
            mCurrentController?.SetActive(false);
            mCurrentController = controller;
            mCurrentController.SetActive(true);
        }

        void ReleaseController()
        {
            mCurrentController?.SetActive(false);
            mCurrentController = null;
        }

        void Awake()
        {
            Debug.Assert(mInstance == null, $"already exists {gameObject.name}.");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            mContents = new Contents.ContentsLoader(mLoaderType);
            LoadGame();
        }

        void Update()
        {
            if (mbGameStart)
            {
                return;
            }

            if (mContents.State != WorkState.Ready)
            {
                return;
            }
            
            Debug.Log("Loaded");
            StartGame();
        }

    }
}