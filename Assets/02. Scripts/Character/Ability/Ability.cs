using PlatformGame.Character.Collision;
using PlatformGame.Pipeline;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    public abstract class Ability : ScriptableObject
    {
        Pipeline<AbilityCollision> mPipeline;

        public void DoActivation(HitBoxCollision collision)
        {
            CreatePipeline();
            var caster = collision.Subject.Actor;
            var victim = collision.Victim == caster ? collision.Attacker : collision.Victim;
            var abilityCollision = new AbilityCollision(caster, victim, this);
            mPipeline.Invoke(abilityCollision);
        }

        public abstract void UseAbility(AbilityCollision collision);

        // TODO : 검토
        void CreatePipeline()
        {
            mPipeline = Pipelines.Instance.AbilityPipeline;
            mPipeline.InsertPipe((collision) => UseAbility(collision));
        }
        //
    }
}