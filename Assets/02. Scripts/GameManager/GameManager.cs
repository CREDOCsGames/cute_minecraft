using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame
{
    public class GameManager : Singleton<GameManager>
    {
        public bool IsGameStart { get; private set; }

        [Header("[Options]")]
        [SerializeField] UnityEvent OnStartGame;
        [SerializeField] UnityEvent OnStopGame;
        [SerializeField] UnityEvent OnExitGame;

        public void ExitGame()
        {
            OnExitGame.Invoke();
            Debug.Assert(OnExitGame.GetPersistentEventCount() > 0, $"You need to tie the shutdown logic to an event : {name}");
        }

        public void StartGame()
        {
            IsGameStart = true;
            OnStartGame.Invoke();
        }

        public void StopGame()
        {
            IsGameStart = false;
            OnStopGame.Invoke();
        }

        public void AddOnStartGameEvent(UnityAction action)
        {
            OnStartGame.AddListener(action);
        }

        public void RemoveOnStartGameEvent(UnityAction action)
        {
            OnStartGame.RemoveListener(action);
        }

        public void AddOnStopGameEvent(UnityAction action)
        {
            OnStopGame.AddListener(action);
        }

        public void RemoveOnStopGameEvent(UnityAction action)
        {
            OnStopGame.RemoveListener(action);
        }

        public void AddOnExitGameEvent(UnityAction action)
        {
            OnExitGame.AddListener(action);
        }

        public void RemoveOnExitGameEvent(UnityAction action)
        {
            OnExitGame.RemoveListener(action);
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

    }
}