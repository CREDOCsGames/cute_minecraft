using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/ReverseAbility")]
    public class ReverseAbility : Ability
    {
        public Ability AbilityAction;

        public override void UseAbility(AbilityCollision collision)
        {
            var abilityCollision = new AbilityCollision(collision.Victim, collision.Caster, collision.Ability);
            AbilityAction.UseAbility(abilityCollision);
        }

    }
}