using Flow;
using UnityEngine;

public class AnimationRandomSpeed : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private Vector2 _speedRange;
    [SerializeField] private Vector2 _intervalRange;
    private Timer _interval = new();

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _interval.SetTimeout(Random.Range(_intervalRange.x, _intervalRange.y));
        _interval.OnTimeout += (t) => _anim.speed = Random.Range(_speedRange.x, _speedRange.y);
        _interval.OnTimeout += (t) => _interval.SetTimeout(Random.Range(_intervalRange.x, _intervalRange.y));
        _interval.OnTimeout += (t) => _interval.DoStart();
        _interval.DoStart();
    }

    void Update()
    {
        _interval.DoTick();
    }
}
