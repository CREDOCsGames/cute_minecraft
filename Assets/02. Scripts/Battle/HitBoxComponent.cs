using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class HitBoxComponent : MonoBehaviour, IHitBox
    {
        [Header("[Refer]")]
        [SerializeField] private UnityEvent<HitBoxCollision> _onHit;
        [Header("[Options]")]
        [SerializeField] protected Transform _actor;
        private HitBox _hitBox;
        public HitBox HitBox
        {
            get
            {
                Debug.Assert(_actor != null, $"Specify an Actor : {name}");
                _hitBox ??= CreateHitBox();
                return _hitBox;
            }
        }
        public static HitBoxComponent AddComponent(GameObject obj, HitBox hitBox)
        {
            var component = obj.AddComponent<HitBoxComponent>();
            component._actor = hitBox.Actor;
            component._hitBox = hitBox;
            return component;
        }

        private HitBox CreateHitBox()
        {
            var hitBox = new HitBox(_actor);
            hitBox.OnCollision += _onHit.Invoke;
            return hitBox;
        }

    }
}