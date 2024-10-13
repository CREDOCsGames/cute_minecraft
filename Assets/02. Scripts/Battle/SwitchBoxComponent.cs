using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

namespace Battle
{
    public class SwitchBoxComponent : MonoBehaviour
    {
        HitBox mHitBox;
        AttackBox mAttackBox;
        GameObject HitBoxInstance;
        GameObject AttackBoxInstance;
        [Header("[Refer]")]
        [SerializeField] Transform Actor;
        [SerializeField] List<Bounds> HitColliders;
        [SerializeField] List<Bounds> AttackColliders;
        [Range(0, 10000)][SerializeField] float AttackWindow;
        [Header("[Options]")]
        [SerializeField] protected UnityEvent<HitBoxCollision> OnHit;
        [SerializeField] protected UnityEvent<HitBoxCollision> OnAttack;

        void SwitchToAttackBox()
        {
            HitBoxInstance.SetActive(false);
            AttackBoxInstance.SetActive(true);
            mAttackBox.OpenAttackWindow();
        }

        void SwitchToHitBox()
        {
            HitBoxInstance.SetActive(true);
            AttackBoxInstance.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            SwitchToHitBox();
        }

        protected virtual void Awake()
        {
            mHitBox = new HitBox(Actor);
            mHitBox.OnCollision += OnHit.Invoke;
            mHitBox.OnCollision += (c) => SwitchToAttackBox();
            mHitBox.OnCollision += (c) => StartCoroutine(Disable());
            HitBoxInstance = CreateColliderBoxes(HitColliders);
            HitBoxComponent.AddComponent(HitBoxInstance, mHitBox);
            HitColliders.Clear();
            HitBoxInstance.SetActive(true);

            mAttackBox = new AttackBox(Actor, AttackWindow);
            mAttackBox.OnCollision += OnAttack.Invoke;
            AttackBoxInstance = CreateColliderBoxes(AttackColliders);
            AttackBoxComponent.AddComponent(AttackBoxInstance, mAttackBox);
            AttackColliders.Clear();
            AttackBoxInstance.SetActive(false);
        }

        IEnumerator Disable()
        {
            yield return new WaitForSeconds(AttackWindow);
            gameObject.SetActive(false);
        }

        GameObject CreateColliderBoxes(List<Bounds> boxes)
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
            foreach (var hitBox in HitColliders)
            {
                Gizmos.DrawWireCube(transform.position + hitBox.center, Vector3.Scale(transform.lossyScale, hitBox.extents));
            }

            foreach (var attackBox in AttackColliders)
            {
                Gizmos.DrawWireCube(transform.position + attackBox.center, Vector3.Scale(transform.lossyScale, attackBox.extents));
            }
            Gizmos.color = originGizmosColor;
        }
#endif

    }
}
