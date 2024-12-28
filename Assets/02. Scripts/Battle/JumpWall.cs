using Controller;
using Movement;
using Puzzle;
using UnityEngine;

namespace Battle
{
    public class JumpWall : MonoBehaviour
    {
        private static float _updateTime;
        private MovementComponent movement;
        public MovementAction Action;
        private void OnTriggerEnter(Collider other)
        {
            if (Time.time < _updateTime + 1.5f)
            {
                return;
            }

            if (other.transform.TryGetComponent<CharacterComponent>(out var con))
            {
                movement = GameObject.FindAnyObjectByType<CubePuzzleComponent>()?.GetComponent<MovementComponent>();
                con._character.Idle();
                con._character.Jump();
                con._character.ChangeController(new JumpState());
                if (Action != null && movement != null)
                {
                    movement.PlayMovement(Action);
                    _updateTime = Time.time;
                }

            }
        }
    }

}
