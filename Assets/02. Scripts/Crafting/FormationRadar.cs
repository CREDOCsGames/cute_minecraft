using PlatformGame.Character;
using UnityEngine;

namespace PlatformGame
{
    public class FormationRadar : MonoBehaviour
    {
        Transform mHome;
        Role pet;
        public void GoTo(Transform transform)
        {
            if (pet != null)
            {
                Debug.Assert(mHome != null);
                pet.SetTransform(mHome);
            }
            pet = FindObjectOfType<FormationManager>().Roles[0];
            mHome = pet.OriginTransform;
            pet.SetTransform(transform);
        }

        public void ReturnHome()
        {
            if (pet == null)
            {
                return;
            }
            pet.SetTransform(mHome);
            pet = null;
        }
    }

}