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
        public static void PrintLog(string text)
        {
            Debug.Log(text);
        }

        public static string GetStrings(byte[] bytes)
        {
            string s = "";
            foreach (var data in bytes)
            {
                s += data.ToString() + " ";
            }
            return s;
        }
    }
}
