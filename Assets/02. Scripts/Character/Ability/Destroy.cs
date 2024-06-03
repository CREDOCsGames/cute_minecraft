using PlatformGame.Character.Collision;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Destroy")]
    public class Destroy : Ability
    {
        public override void UseAbility(AbilityCollision collision)
        {
            var victim = collision.Victim;
            DestroyTo(victim);
        }

        public static void DestroyTo(Character character)
        {
            if (!character.Attribute.IsInclude(AttributeFlags.Destructibility))
            {
                return;
            }
            GameObject.Destroy(character.gameObject);
        }
    }
}