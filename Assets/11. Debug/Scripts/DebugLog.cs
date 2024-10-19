using Battle;
using Character;
using UnityEngine;
using Conditional = System.Diagnostics.ConditionalAttribute;


namespace PlatformGame.Debugger
{
    [CreateAssetMenu(menuName = "Custom/Log")]
    public class DebugLog : ScriptableObject
    {
        public static readonly string ID_ABILITY = "00";
        public static readonly string ID_CONTROLLER = "01";
        public static readonly string ID_HITBOXCOLLIDER = "02";
        public static readonly string ID_CHARACTERSTATE = "03";

        [Conditional("DEVELOPMENT")]
        public void PrintLog(HitBoxCollision collision)
        {
            Debug.Log($"{collision.Attacker.name} -> {collision.Victim}: Hit");
        }

        [Conditional("DEVELOPMENT")]
        public static void PrintLog(Transform who, CharacterState state)
        {
            Debug.Log($"{who.name}: {state}");
        }

        [Conditional("DEVELOPMENT")]
        public static void PrintLog(string text)
        {
            Debug.Log(text);
        }

        [Conditional("DEVELOPMENT")]
        public static void PrintLog(Character.CharacterComponent character)
        {
            Debug.Log(character.State);
        }
    }
}
