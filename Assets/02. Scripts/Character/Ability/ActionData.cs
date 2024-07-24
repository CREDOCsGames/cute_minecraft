using PlatformGame.Character.Movement;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Action/ActionData")]
    public class ActionData : ScriptableObject
    {
        public uint ID;
        public string Name;
        public float ActionDelay;
        public CharacterState BeState;
        public CharacterStateFlags AllowedState;
        public MovementAction Movement;
    }
}