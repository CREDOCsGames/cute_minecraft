using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Character.Collision
{
    [Flags, Serializable]
    public enum AttributeFlags
    {
        None = 0,
        NonStatic = 1 << 0,
        Destructibility = 1 << 1
    }

    [Serializable]
    public class AttributeFlag
    {
        [SerializeField] AttributeFlags mFlags;

        public AttributeFlags Flags
        {
            get => mFlags;
            private set => mFlags = value;
        }

        public void SetFlag(AttributeFlags flags, Character character)
        {
            Flags = flags;
            character.Rigid.isKinematic = !IsInclude(AttributeFlags.NonStatic);
        }

        public bool Equals(AttributeFlags flags)
        {
            return flags == Flags;
        }

        public bool IsInclude(AttributeFlags flag)
        {
            Debug.Assert((flag & (flag - 1)) == 0 && flag != 0, $"Only one flag should be used : {flag}");
            return (Flags & flag) == flag;
        }

        public static List<HitBoxCollider> GetCollidersAs(Dictionary<string, HitBoxCollider> list,
            List<string> filterColliderNames)
        {
            var colliders = new List<HitBoxCollider>();

            if (filterColliderNames.Any(x => x.Equals("*")))
            {
                colliders = list.Values.ToList();
            }
            else
            {
                foreach (var colliderName in filterColliderNames)
                {
                    Debug.Assert(list.ContainsKey(colliderName), $"{colliderName} is not found in HitBoxColliders.");
                    colliders.Add(list[colliderName]);
                }
            }

            return colliders;
        }
    }
}