using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [CreateAssetMenu(menuName = "Custom/Ability/Creater")]
    public class Creater : Ability
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
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        public override void UseAbility(AbilityCollision collision)
        {
            CreateObject(Prefab);
        }
    }

}
