using PlatformGame.Character;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Character mCharacter;
    static Vector3 m3DVelocity;

    [SerializeField] bool mbLookAtMoveDir = true;
    [SerializeField] Transform mTarget;

    static void LookAtMovingDirection(Transform transform, Vector3 dir)
    {
        m3DVelocity = dir;
        m3DVelocity.y = 0;
        if (m3DVelocity == Vector3.zero)
        {
            return;
        }

        transform.forward = m3DVelocity;
    }
    public void SetTarget(Transform character)
    {
        mTarget = character;
    }

    void LookAtMoveDir()
    {
        if (mCharacter.State is not (CharacterState.Walk or CharacterState.Running))
        {
            return;
        }

        var velocity = mCharacter.Rigid.velocity;
        velocity.y = 0f;
        if (Mathf.Abs(velocity.magnitude) < 1f)
        {
            return;
        }

        LookAtMovingDirection(transform, velocity);
    }

    void LookAtTarget()
    {
        var viewPos = mTarget.position; ;
        viewPos.y = transform.position.y;
        transform.LookAt(viewPos);
    }

    void Update()
    {
        if (mbLookAtMoveDir)
        {
            LookAtMoveDir();
        }
        else
        {
            LookAtTarget();
        }
    }

}
