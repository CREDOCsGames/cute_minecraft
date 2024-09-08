using PlatformGame.Character;
using PlatformGame.Character.Collision;
using PlatformGame.Character.Combat;
using PlatformGame.Character.Controller;
using PlatformGame.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public void PrintLog(AbilityCollision collision)
        {
            var caster = collision.Caster;
            var abilityName = collision.Ability.name;
            DebugWrapper.LogMessage(caster.transform.GetInstanceID(), $"{ID_ABILITY}{abilityName}");
        }

        [Conditional("DEVELOPMENT")]
        public static void PrintLog(ControllerInputData inputData)
        {
            var instanceID = inputData.Controller.transform.GetInstanceID();
            DebugWrapper.LogMessage(instanceID, $"{ID_CONTROLLER}{inputData.Key}");
        }

        [Conditional("DEVELOPMENT")]
        public void ReloadScene(ControllerInputData inputData)
        {
            if (inputData.Key == ActionKey.KEY_Debug)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

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
        public static void PrintLog(Character.Character character)
        {
            Debug.Log(character.State);
        }
    }
}
