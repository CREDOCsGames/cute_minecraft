using PlatformGame.Manager;
using UnityEngine;
using UnityEngine.Events;

public class MovieEventHandler : MonoBehaviour
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

    void Awake()
    {
        GameManager.MovieCutscene.OnPlayEvent += () => OnPlayEvent.Invoke();
        GameManager.MovieCutscene.OnEndEvent += () => OnEndEvent.Invoke();
        GameManager.MovieCutscene.OnSkipEvent += () => OnSkipEvent.Invoke();
    }

    void OnDestroy()
    {
        GameManager.MovieCutscene.OnPlayEvent -= () => OnPlayEvent.Invoke();
        GameManager.MovieCutscene.OnEndEvent -= () => OnEndEvent.Invoke();
        GameManager.MovieCutscene.OnSkipEvent -= () => OnSkipEvent.Invoke();
    }
}
