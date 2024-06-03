using PlatformGame.Character.Movement;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Satellite")]
    public class Satellite : Ability
    {
        [SerializeField] Character mSatellite;
        [SerializeField] SatelliteMovement mMovement;

        public override void UseAbility(AbilityCollision collision)
        {
            Debug.Assert(mSatellite, $"Satellite reference not found. : {name}");
            Debug.Assert(mMovement, $"Movement reference not found : {name}");

            var attacker = collision.Caster;
            var owner = attacker.transform;
            SpawnSatellite(mSatellite, owner, mMovement);
        }

        public static void SpawnSatellite(Character satellitePrefab, Transform owner, SatelliteMovement movement)
        {
            var satellite = Instantiate(satellitePrefab, owner) as Character;
            satellite.Movement.PlayMovement(movement);
        }
    }

}