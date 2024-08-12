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
            var victim = collision.Victim.GetComponent<Character>();
            if (victim == null)
            {
                DestroyTo(collision.Victim);
            }
            else
            {

                DestroyTo(victim);
            }
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

        public static void DestroyTo(Transform gameObject)
        {
            GameObject.Destroy(gameObject.gameObject);
        }
    }
}