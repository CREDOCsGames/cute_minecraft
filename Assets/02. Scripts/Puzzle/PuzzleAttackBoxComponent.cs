using Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleAttackBoxComponent : AttackBoxComponent
    {
        [SerializeField][Range(0, 255)] private byte _type;
        public byte Type => _type;

        [SerializeField] private GameObject _cursorPrefab;
        private GameObject _cursorInstance;
        private Collider _focus;
        private List<Collider> _hitTargets = new();


        protected override void Awake()
        {
            base.Awake();
            _cursorInstance = Instantiate(_cursorPrefab);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _focus = null;
            if (0 < _hitTargets.Count)
            {
                _focus = _hitTargets[0];
                _cursorInstance.transform.position = _focus.transform.position + (Vector3.up * 2f);
                AttackBox.CheckCollision(_focus);
                _hitTargets.Clear();
            }
            _cursorInstance.SetActive(_focus != null);

        }

        protected override void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<IHitBox>(out var hitBox))
            {
                _hitTargets.Add(other);
            }
        }

    }
}
