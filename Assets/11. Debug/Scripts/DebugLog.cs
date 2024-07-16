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
            var subject = collision.Subject;
            var instanceID = subject.transform.GetInstanceID();
            var hitLog = subject.IsAttacker ? "Attack" : "Hit";
            DebugWrapper.LogMessage(instanceID, $"{ID_HITBOXCOLLIDER}{subject.name}, {hitLog}");
        }

        [Conditional("DEVELOPMENT")]
        public static void PrintLog(Transform who, CharacterState state)
        {
            DebugWrapper.LogMessage(who.GetInstanceID(), $"{ID_CHARACTERSTATE}{state}");
        }

        public static void PrintLog(string text)
        {
            Debug.Log(text);
        }
    }
}
