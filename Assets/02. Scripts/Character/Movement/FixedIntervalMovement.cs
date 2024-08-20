using System.Collections;
using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/FixedIntervalMovement")]
    public class FixedIntervalMovement : TransformBaseMovement
    {
        [SerializeField] float mTime = 1f;

        public override IEnumerator Move(Transform start, Transform end, bool repeat = false)
        {
            var duration = 0f;
            var s = start.position;
            var e = end.position;
            start.SetParent(null);
            start.LookAt(e);
            while(true)
            {
                duration += Time.fixedDeltaTime;
                var t = Mathf.Clamp(duration / mTime, 0, 1);
                start.transform.position = Vector3.Slerp(s, e, t);
                if(t == 1)
                {
                    break;
                }
                yield return new WaitForFixedUpdate();

            }
        }

        
    }

}
