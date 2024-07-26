using PlatformGame.Character.Collision;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Destroy")]
    public class Destroy : Ability
    {
        const uint STATE_DIE = 4290000010;
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
            character.DoAction(STATE_DIE);
            var destroyDelay = 3f;
            GameObject.Destroy(character.gameObject, destroyDelay);
        }

        public static void DestroyTo(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }
}