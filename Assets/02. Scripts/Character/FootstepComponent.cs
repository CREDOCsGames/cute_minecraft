using UnityEngine;
using Util;

public class FootstepComponent : MonoBehaviour
{
    private bool _bLanding;
    private bool _bLeftStep;
    private bool _bRightStep;
    [SerializeField] private Transform _leftFoot;
    [SerializeField] private Transform _rightFoot;
    [SerializeField] private ParticleSystem _footstepEffect;
    [SerializeField] private ParticleSystem _stempEffect;

    public void StepLeft()
    {
        _bLeftStep = true;
        PlayEffect();
    }
    public void StepRight()
    {
        _bRightStep = true;
        PlayEffect();
    }
    public void JumpUp()
    {
        _bLeftStep = true;
        _bRightStep = true;
        PlayEffect();
    }
    public void OnLand()
    {
        _bLanding = true;
        _bLeftStep = true;
        _bRightStep = true;
        PlayEffect();
    }
    private void PlayEffect()
    {
        var effect = _bLanding ? _stempEffect : _footstepEffect;
        if (!effect)
        {
            Debug.LogWarning($"Effect is null. {DM_ERROR.REFERENCES_NULL} in {name}.");
            return;
        }
        if (_bLeftStep && _leftFoot == null)
        {
            Debug.LogWarning($"Left Foot is null. {DM_ERROR.REFERENCES_NULL} in {name}.");
            return;
        }
        if (_bRightStep && _rightFoot == null)
        {
            Debug.LogWarning($"Right Foot is null. {DM_ERROR.REFERENCES_NULL} in {name}.");
            return;
        }

        Vector3 effectPosition;
        if (_bLeftStep && _bRightStep)
        {
            effectPosition = Vector3.Lerp(_leftFoot.position, _rightFoot.position, 0.5f);
        }
        else if (_bLeftStep)
        {
            effectPosition = _leftFoot.position;
        }
        else
        {
            effectPosition = _rightFoot.position;
        }

        ParticlePlayer.PlayParticleAt(effect, effectPosition);
        _bLanding = _bRightStep = _bLeftStep = false;
    }
}