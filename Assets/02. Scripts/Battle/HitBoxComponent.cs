using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class HitBoxComponent : MonoBehaviour, IHitBox
    {
        [Header("[Refer]")]
        [SerializeField] UnityEvent<HitBoxCollision> OnHit;
        [Header("[Options]")]
        [SerializeField] protected Transform Actor;
        HitBox mHitBox;
        public HitBox HitBox
        {
            get
            {
                Debug.Assert(Actor != null, $"Specify an Actor : {name}");
                mHitBox ??= CreateHitBox();
                return mHitBox;
            }
        }
        public static HitBoxComponent AddComponent(GameObject obj, HitBox hitBox)
        {
            var component = obj.AddComponent<HitBoxComponent>();
            component.Actor = hitBox.Actor;
            component.mHitBox = hitBox;
            return component;
        }

        HitBox CreateHitBox()
        {
            var hitBox = new HitBox(Actor);
            hitBox.OnCollision += OnHit.Invoke;
            return hitBox;
        }

    }
}