using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementComponent : MonoBehaviour
    {
        private Rigidbody _rigid;

        public Rigidbody Rigid
        {
            get
            {
                if (_rigid == null)
                {
                    _rigid = GetComponent<Rigidbody>();
                }

                return _rigid;
            }
        }

        private MovementAction _beforeAction;
        [SerializeField] private MovementAction _defaultActionOrNull;

        public void PlayMovement(MovementAction movement)
        {
            RemoveMovement();
            _beforeAction = movement;
            movement.PlayAction(Rigid, this);
        }

        public void RemoveMovement()
        {
            StopAllCoroutines();
            if (!_beforeAction)
            {
                return;
            }

            _beforeAction.StopAction(_rigid, this);
        }

        private void Awake()
        {
            if (_defaultActionOrNull != null)
            {
                PlayMovement(_defaultActionOrNull);
            }
        }
    }
}