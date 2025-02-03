using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Util;

public class FootstepComponent : MonoBehaviour
{
    [SerializeField] private Transform _leftFoot;
    [SerializeField] private Transform _rightFoot;
    [SerializeField] private ParticleSystem _footstepEffect;
    [SerializeField] private ParticleSystem _footStempEffect;

    public void StepLeft()
    {
        if (_leftFoot == null)
        {
            return;
        }
        PlayEffect(_leftFoot.position, _footstepEffect);
    }
    public void StepRight()
    {
        if (_rightFoot == null)
        {
            return;
        }
        PlayEffect(_rightFoot.position, _footstepEffect);
    }
    public void JumpUp()
    {
        if (_leftFoot && _rightFoot)
        {
            PlayEffect(Vector3.Lerp(_leftFoot.position, _rightFoot.position, 0.5f), _footstepEffect);
        }
    }
    public void OnLand()
    {
        if (_leftFoot && _rightFoot)
        {
            PlayEffect(Vector3.Lerp(_leftFoot.position, _rightFoot.position, 0.5f), _footStempEffect);
        }
    }
    private void PlayEffect(Vector3 stepPoint, ParticleSystem particle)
    {
        if (particle == null)
        {
            return;
        }
        var effect = ParticlePlayer.GetPool(particle).Get();
        effect.transform.position = stepPoint;
        effect.Play();
        CoroutineRunner.instance.StartCoroutine(ReleaseEffect(particle, effect));
    }
    private IEnumerator ReleaseEffect(ParticleSystem origin, ParticleSystem particle)
    {
        yield return new WaitForSeconds(particle.main.duration);
        ParticlePlayer.GetPool(origin).Release(particle);
    }
}
