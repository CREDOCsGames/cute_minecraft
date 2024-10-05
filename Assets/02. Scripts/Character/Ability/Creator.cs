using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Creator")]
    public class Creator : Ability
    {
        public GameObject Prefab;

        public void CreateObject(Transform transform)
        {
            CreateObject(Prefab, transform);
        }

        public static void CreateObject(GameObject prefab)
        {
            GameObject.Instantiate(prefab);
        }

        public static void CreateObject(GameObject prefab, Transform transform)
        {
            var obj = GameObject.Instantiate(prefab);

            obj.transform.SetPositionAndRotation(transform.position + Vector3.right + Vector3.up + Vector3.back + (Vector3.up * 1.5f), Quaternion.identity);
        }

        public override void UseAbility(AbilityCollision collision)
        {
            CreateObject(Prefab);
        }
    }

}
