using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Destroy")]
    public class Destroy : Ability
    {
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
            character.DoAction("State(Die)".GetHashCode());
            var destroyDelay = 3f;
            GameObject.Destroy(character.gameObject, destroyDelay);
        }

        public static void DestroyTo(Transform gameObject)
        {
            GameObject.Destroy(gameObject.gameObject);
        }
    }
}