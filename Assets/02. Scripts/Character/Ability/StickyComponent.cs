using UnityEngine;

namespace PlatformGame.Character
{
    public class StickyComponent : MonoBehaviour
    {
        static readonly Vector3[] mDirs =
        {
            Vector3.up,
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
            Vector3.down
        };

        StickyComponent mRoot;
        StickyComponent mPrev;
        StickyComponent mNext;
        public bool IsStuck { get; private set; }

        int mCurrentDir;

        int CurrentDir
        {
            get => mCurrentDir;
            set => mCurrentDir = Mathf.Clamp(value < mDirs.Length ? value : 0, 0, mDirs.Length - 1);
        }

        Character mCharacter;

        public void DetachFromOther()
        {
            if (!IsStuck)
            {
                return;
            }

            if (mPrev == mRoot)
            {
                mRoot.DetachFromOther();
            }

            mPrev = null;
            mNext = null;
            mRoot = null;
            IsStuck = false;

            mCharacter.Rigid.isKinematic = false;
        }

        public void StickAround()
        {
            var pos = Vector3.zero;
            foreach (var i in mDirs)
            {
                if (pos != Vector3.zero)
                {
                    break;
                }

                CurrentDir++;
                pos = FindStickyPos(mDirs[CurrentDir]);
            }

            if (pos == Vector3.zero)
            {
                return;
            }

            mPrev.mRoot = mPrev.mRoot ? mPrev.mRoot : mPrev;
            mPrev.mNext = this;
            mPrev.IsStuck = true;

            mRoot = mPrev.mRoot;
            IsStuck = true;

            transform.position = pos;
            mCharacter.Rigid.isKinematic = true;
            mPrev.mCharacter.Rigid.isKinematic = true;
        }

        Vector3 FindStickyPos(Vector3 dir)
        {
            var near = Vector3.zero;

            var hitTarget = FindHandShakeTarget(transform.position, transform, dir);
            if (hitTarget == null)
            {
                return near;
            }

            var hitMe = FindHandShakeTarget(hitTarget.Value.point, hitTarget.Value.transform, -dir);
            if (hitMe == null)
            {
                return near;
            }

            mPrev = hitTarget.Value.transform.GetComponent<StickyComponent>();
            if (mPrev.mNext != null)
            {
                mPrev = null;
                return near;
            }

#if DEVELOPMENT
            line.SetPosition(0, hitMe.Value.point);
            line.SetPosition(1, hitTarget.Value.point);
#endif

            var hitTargetPos = mPrev.transform.position;
            var one = hitTargetPos - transform.position;
            var three = hitTargetPos - hitMe.Value.point;
            near = hitTarget.Value.point - (one - three);
            return near;
        }

        static RaycastHit? FindHandShakeTarget(Vector3 offset, Transform A, Vector3 dir)
        {
            if (!Physics.Raycast(offset, dir, out var hit, 5f))
            {
                return null;
            }

            var character = hit.transform.GetComponent<Character>();
            if (character == null || character.transform == A)
            {
                return null;
            }

            if (character.GetComponent<StickyComponent>() == null)
            {
                return null;
            }

            return hit;
        }

        void Awake()
        {
            mCharacter = GetComponent<Character>();

#if DEVELOPMENT
            if (line != null)
            {
                return;
            }

            line = gameObject.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.material = Resources.Load<Material>("M_Yellow");
            line.widthMultiplier = 0.1f;
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
#endif
        }

#if DEVELOPMENT
        LineRenderer line;
        void Update()
        {
            line.enabled = false;

            if (IsStuck)
            {
                return;
            }

            var dir = CurrentDir;
            foreach (var i in mDirs)
            {
                var pos = FindStickyPos(mDirs[dir]);
                if (pos != Vector3.zero)
                {
                    line.enabled = true;
                    return;
                }

                dir = dir + 1 < mDirs.Length ? dir + 1 : 0;
            }
        }
#endif
    }
}