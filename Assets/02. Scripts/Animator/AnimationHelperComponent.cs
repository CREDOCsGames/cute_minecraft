using Flow;
using UnityEngine;
using UnityEngine.Events;

public class AnimationHelperComponent : MonoBehaviour
{
    private Timer _timer = new();
    private bool _playing;
    private Animator _animator;
    [SerializeField] private string _clipName;
    [SerializeField] private Vector2 _speedRange;
    [SerializeField] private Vector2 _intervalRange;
    [SerializeField] private UnityEvent _onAnimationFinished;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            return;
        }
        _onAnimationFinished.AddListener(() => _animator.speed = Random.Range(_speedRange.x, _speedRange.y));
        _onAnimationFinished.AddListener(() => _timer.SetTimeout(Random.Range(_intervalRange.x, _intervalRange.y)));
        _onAnimationFinished.AddListener(() => _timer.DoStart());
        _timer.OnTimeout += (t) => _animator.Play(_clipName,0,0);
        _timer.OnTimeout += (t) => _playing = true;
        _onAnimationFinished.Invoke();
    }
    void Update()
    {
        var clip = _animator.GetCurrentAnimatorStateInfo(0);
        if (_playing && !clip.loop && 1 <= clip.normalizedTime)
        {
            _playing = false;
            _onAnimationFinished?.Invoke();
        }
        else
        if (!_playing && clip.normalizedTime < 1)
        {
            _playing = true;
        }
        _timer.DoTick();
    }
}
