using System.Collections;
using UnityEngine;

namespace Controller
{
    public class MonsterComponent : MonoBehaviour
    {
        public Character _character { get; private set; }
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            _character = new Character(_rigidbody, _animator);
        }

        public void Exit(Vector3 dir)
        {
            _character.Rigidbody.excludeLayers = -1;
            _character.ChangeController(new CanNotControl());
            _character.Rigidbody.AddForce(dir * 7f, ForceMode.Impulse);
        }
        public void Hit()
        {
            _character?.Hit();
        }
        private void Update()
        {
            _character?.Update();
        }
    }

}
