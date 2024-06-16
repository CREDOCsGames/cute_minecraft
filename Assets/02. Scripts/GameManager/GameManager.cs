using PlatformGame.Character.Controller;
using PlatformGame.Contents;
using PlatformGame.Contents.Loader;
using PlatformGame.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using static PlatformGame.Character.Character;
using static PlatformGame.Input.ActionKey;

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
        Contents.Contents mContents;
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

        void PauseGame()
        {
            Debug.Log("Pause Game");
            ReleaseController();
        }

        void StopGame()
        {
            Debug.Log("Stop Game");
            mbGameStart = false;
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

        void SwapCharacter()
        {
            if (JoinCharactersController.Count < 2)
            {
                return;
            }

            var first = JoinCharactersController.First();
            JoinCharactersController.RemoveAt(0);
            JoinCharactersController.Add(first);
            ControlDefaultCharacter();
        }

        void Awake()
        {
            Debug.Assert(mInstance == null, $"already exists {gameObject.name}.");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            mContents = new Contents.Contents(mLoaderType);
            LoadGame();
        }

        void Update()
        {
            if (mbGameStart)
            {
                // TODO : 분리
                if (Time.time < mLastSwapTime + 0.5f)
                {
                    return;
                }

                var map = ActionKey.GetKeyDownMap();
                if (!map[KEY_SWAP])
                {
                    return;
                }
                SwapCharacter();

                mLastSwapTime = Time.time;
                // TODOEND
            }
            else
            {
                if (mContents.State == WorkState.Ready)
                {
                    Debug.Log("Loaded");
                    StartGame();
                }
            }
        }

    }
}