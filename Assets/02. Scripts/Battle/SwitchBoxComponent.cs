using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class SwitchBoxComponent : MonoBehaviour
    {
        private HitBox _hitBox;
        private AttackBox _attackBox;
        private GameObject _hitBoxInstance;
        private GameObject _attackBoxInstance;
        [Header("[Refer]")]
        [SerializeField] private Transform _actor;
        [SerializeField] private List<Bounds> _hitColliders;
        [SerializeField] private List<Bounds> _attackColliders;
        [Range(0, 10000), SerializeField] private float _attackWindow;
        [Header("[Options]")]
        [SerializeField] protected UnityEvent<HitBoxCollision> _onHit;
        [SerializeField] protected UnityEvent<HitBoxCollision> _onAttack;

        private void SwitchToAttackBox()
        {
            _hitBoxInstance.SetActive(false);
            _attackBoxInstance.SetActive(true);
            _attackBox.OpenAttackWindow();
        }

        private void SwitchToHitBox()
        {
            _hitBoxInstance.SetActive(true);
            _attackBoxInstance.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            SwitchToHitBox();
        }

        protected virtual void Awake()
        {
            _hitBox = new HitBox(_actor);
            _hitBox.OnCollision += _onHit.Invoke;
            _hitBox.OnCollision += (c) => SwitchToAttackBox();
            _hitBox.OnCollision += (c) => StartCoroutine(Disable());
            _hitBoxInstance = CreateColliderBoxes(_hitColliders);
            HitBoxComponent.AddComponent(_hitBoxInstance, _hitBox);
            _hitColliders.Clear();
            _hitBoxInstance.SetActive(true);

            _attackBox = new AttackBox(_actor, _attackWindow);
            _attackBox.OnCollision += _onAttack.Invoke;
            _attackBoxInstance = CreateColliderBoxes(_attackColliders);
            AttackBoxComponent.AddComponent(_attackBoxInstance, _attackBox);
            _attackColliders.Clear();
            _attackBoxInstance.SetActive(false);
        }

        private IEnumerator Disable()
        {
            yield return new WaitForSeconds(_attackWindow);
            gameObject.SetActive(false);
        }

        private GameObject CreateColliderBoxes(List<Bounds> boxes)
        {
            var obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            var rigid = obj.AddComponent<Rigidbody>();
            rigid.isKinematic = true;
            foreach (var coll in boxes)
            {
                var box = obj.AddComponent<BoxCollider>();
                box.size = coll.size;
                box.center = coll.center;
                box.isTrigger = true;
            }
            return obj;
        }

#if DEVELOPMENT
        private void OnDrawGizmosSelected()
        {
            var originGizmosColor = Gizmos.color;
            Gizmos.color = new Color(145, 244, 139, 210) / 255f;
            foreach (var hitBox in _hitColliders)
            {
                Gizmos.DrawWireCube(transform.position + hitBox.center, Vector3.Scale(transform.lossyScale, hitBox.extents));
            }

            foreach (var attackBox in _attackColliders)
            {
                Gizmos.DrawWireCube(transform.position + attackBox.center, Vector3.Scale(transform.lossyScale, attackBox.extents));
            }
            Gizmos.color = originGizmosColor;
        }
#endif

    }
}
