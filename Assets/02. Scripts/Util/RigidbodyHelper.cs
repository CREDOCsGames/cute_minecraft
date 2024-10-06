using System.Linq;
using UnityEngine;

namespace Util
{
    public static class RigidbodyHelper
    {
        public static bool IsGrounded(Rigidbody rigid)
        {
            var transform = rigid.transform;
            var origin = transform.position;
            origin.y += 0.5f;
            var hitAll = Physics.BoxCastAll(origin, Vector3.one * 0.25f, Vector3.down, transform.rotation, 0.5f);
            var hitsExceptForMyself = hitAll.Where(x => !x.transform.Equals(rigid.transform)).ToList();

#if DEVELOPMENT
            if (hitsExceptForMyself.Any())
            {
                Debug.DrawRay(origin, Vector3.down, Color.yellow, hitsExceptForMyself.Min(x => x.distance));
                DebugGizmos.DrawWireCube(origin + Vector3.down * hitsExceptForMyself.Min(x => x.distance),
                    Vector3.one * 0.25f, rigid.GetHashCode());
            }
            else
            {
                Debug.DrawRay(origin, Vector3.down, Color.red, 0.3f);
                DebugGizmos.DrawWireCube(origin + Vector3.down * 0.3f, Vector3.one * 0.25f, rigid.GetHashCode());
            }
#endif
            return hitsExceptForMyself.Any();
        }
    }
}