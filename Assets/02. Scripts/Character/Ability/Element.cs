using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Element")]
    public class Element : Ability
    {
        public string TargetTag = "Fire";
        public UnityEvent<Transform> BurnEvent;

        public override void UseAbility(AbilityCollision collision)
        {
            var attacker = collision.Caster;
            if (!attacker.CompareTag(TargetTag))
            {
                return;
            }

            var victim = collision.Victim;
            Burn(victim, attacker);
            BurnEvent.Invoke(victim);
        }

        public static void Burn(Transform victim, Transform attacker)
        {
            var obj = Instantiate(attacker);
            obj.transform.position = victim.transform.position;
        }

    }
}