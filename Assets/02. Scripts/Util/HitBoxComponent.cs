using System.Linq;
using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class HitBoxComponent : MonoBehaviour, IHitBox
    {
        [SerializeField] HitBox mHitBox;
        public HitBox HitBox => mHitBox;

        void OnTriggerStay(Collider other)
        {
            if (!HitBox.IsAttacker)
            {
                return;
            }
            HitBox.CheckHit(other);
        }

    }
}