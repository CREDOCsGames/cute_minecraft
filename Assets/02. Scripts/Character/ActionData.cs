using Movement;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(menuName = "Action/ActionData")]
    public class ActionData : ScriptableObject
    {
        public int ID => name.GetHashCode();
        public string Name => name;
        public float ActionDelay;
        public CharacterState BeState;
        public CharacterStateFlags AllowedState;
        public MovementAction Movement;
    }
}