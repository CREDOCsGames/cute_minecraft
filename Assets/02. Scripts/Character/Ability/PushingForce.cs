using UnityEngine;
using static PlatformGame.Character.Collision.AttributeFlags;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/PushingForce")]
    public class PushingForce : Ability
    {
        public float PowerMultiply = 300f;
        public float UpperPower = 3f;

        public override void UseAbility(AbilityCollision collision)
        {
            var victim = collision.Victim.GetComponent<Character>();
            if (victim == null)
            {
                return;
            }
            var rigid = victim.Rigid;
            if (rigid == null)
            {
                return;
            }

            var attacker = collision.Caster.GetComponent<Character>();
            if (attacker == null)
            {
                return;
            }

            var force = attacker.Model.forward;
            force.y = UpperPower;
            force *= PowerMultiply;
            PushingTo(victim, force);
        }

        public static void PushingTo(Character victim, Vector3 force)
        {
            if (!victim.Attribute.IsInclude(NonStatic))
            {
                return;
            }

            if (victim.transform.parent != null)
            {
                victim.transform.SetParent(null, true);
            }

            victim.Movement.RemoveMovement();
            victim.Rigid.AddForce(force);
        }

    }
}