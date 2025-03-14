using Controller;
using Sound;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class Throw : MonoBehaviour
    {
        [SerializeField] private Collider Trigger;
        public Vector3 PowerMulti = Vector3.one;
        private bool enable;
        [SerializeField] private UnityEvent<Collider> _onThrow;

        public bool Enable
        {
            get => enable;
            set
            {
                Trigger.enabled = false;
                Trigger.enabled = true;
                enable = value;
            }
        }
        [NonSerialized] public Vector3 Dir = Vector3.one;

        private void OnTriggerEnter(Collider collision)
        {
            if (!Enable)
            {
                return;
            }
            if (collision.TryGetComponent<MonsterComponent>(out var monster))
            {
                var rigid = monster.GetComponent<Rigidbody>();
                rigid.velocity = Vector3.zero;
                monster.Exit(Vector3.Scale(Dir, PowerMulti));
                SoundManagerComponent.Instance.PlaySound("Cannon");
            }
        }
    }

}
