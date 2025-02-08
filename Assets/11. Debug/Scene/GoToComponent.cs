using UnityEngine;

namespace Controller
{
    public class GoToComponent : MonoBehaviour
    {
        [SerializeField] private CharacterComponent _character;

        private void Update()
        {
            if (_character == null) return;
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    _character._character.ChangeController(new GoToState(hit.point));
                }
            }
            if (Input.GetMouseButton(1))
            {
                _character._character.Jump();
                _character._character.ChangeController(new JumpState());
            }
        }
    }

}
