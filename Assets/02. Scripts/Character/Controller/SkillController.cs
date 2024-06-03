using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Character.Controller
{
    public class Skill
    {

    }
    public class SkillList
    {
        static Dictionary<uint, Skill> list;
        public static Skill FindSkill(uint skillID)
        {
            return list[skillID];
        }
    }

    public interface KeySlot
    {
        public void ConvertToSkill(string key, out Skill skill);
    }

    public class KeyboardPreset : KeySlot
    {
        Dictionary<string, uint> mKeyMap;

        public void ConvertToSkill(string key, out Skill skill)
        {
            var skillID = mKeyMap[key];
            skill = SkillList.FindSkill(skillID);
        }

    }

    public class SkillController : ScriptableObject
    {
        KeySlot mKeySlot;

        public void OnKeyDown(string key)
        {
            ConvertToSkill(key);
            if (!CanUseSkill()) return;
            InvokeSkill();
            UpdateState();
        }

        void ConvertToSkill(string key)
        {
            mKeySlot.ConvertToSkill(key, out Skill skill);
        }

        bool CanUseSkill()
        {
            return false;
        }

        void InvokeSkill()
        {

        }

        void UpdateState()
        {

        }

    }
}
