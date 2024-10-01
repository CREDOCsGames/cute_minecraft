using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class MovieComponent : MonoBehaviour
    {
        [SerializeField] UnityEvent OnPlayEvent;
        [SerializeField] UnityEvent OnEndEvent;
        [SerializeField] UnityEvent OnSkipEvent;

        public void OnPlay()
        {
            GameManager.MovieCutscene.OnPlay();
        }

        public void OnEnd()
        {
            GameManager.MovieCutscene.OnEnd();
        }

        public void OnSkip()
        {
            GameManager.MovieCutscene.OnSkip();
        }

        void InvokeOnPlayEvent()
        {
            OnPlayEvent.Invoke();
        }

        void InvokeOnEndEvent()
        {
            OnEndEvent.Invoke();
        }

        void InvokeOnSkipEvent()
        {
            OnSkipEvent.Invoke();
        }

        void Awake()
        {
            GameManager.MovieCutscene.OnPlayEvent += InvokeOnPlayEvent;
            GameManager.MovieCutscene.OnEndEvent += InvokeOnEndEvent;
            GameManager.MovieCutscene.OnSkipEvent += InvokeOnSkipEvent;
        }

        void OnDestroy()
        {
            GameManager.MovieCutscene.OnPlayEvent -= InvokeOnPlayEvent;
            GameManager.MovieCutscene.OnEndEvent -= InvokeOnEndEvent;
            GameManager.MovieCutscene.OnSkipEvent -= InvokeOnSkipEvent;
        }
    }
}