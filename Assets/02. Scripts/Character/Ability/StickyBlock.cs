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

            var victim = collision.Victim;
            if (!victim.Attribute.IsInclude(AttributeFlags.Stickiness) ||
                !victim.Attribute.IsInclude(AttributeFlags.NonStatic))
            {
                return;
            }

            var stickyBlock = victim.GetComponent<StickyComponent>();
            Debug.Assert(stickyBlock != null, $"Component not found : {victim.name}");

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