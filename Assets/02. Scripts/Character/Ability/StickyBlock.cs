using UnityEngine;


namespace PlatformGame.Character.Collision
{
    [CreateAssetMenu(menuName = "Custom/Pipeline/StickyBlock")]
    public class StickyBlock : ScriptableObject
    {
        public void OnHit(HitBoxCollision collision)
        {
            if (collision.Subject.IsAttacker)
            {
                return;
            }

            var victim = collision.Victim.GetComponent<Character>();
            if(victim == null)
            {
                return;
            }

            if (!victim.Attribute.IsInclude(AttributeFlags.NonStatic))
            {
                return;
            }

            var stickyBlock = victim.GetComponent<StickyComponent>();
            if (stickyBlock == null)
            {
                return;
            }

            if (stickyBlock.IsStuck)
            {
                stickyBlock.DetachFromOther();
            }
            else
            {
                stickyBlock.StickAround();
            }
        }
    }
}