using PlatformGame.Character;
using System.Linq;
using UnityEngine;

namespace PlatformGame
{
    public class FormationRadar : MonoBehaviour
    {
        public string PetName;

        public void SetPetTransform(Transform transform)
        {
            var pet = FindPet(PetName);
            pet.SetTransform(transform);
        }

        public void CombackPet()
        {
            var formation = FindFormation();
            var pet = FindPet(PetName);
            formation.Comback(pet);
        }

        public void ChatText(string text)
        {
            var pet = FindPet(PetName);
            pet.Chat.SetText(text);
            pet.Chat.Show(true);
        }

        public void HideChatBalloon()
        {
            var pet = FindPet(PetName);
            pet.Chat.Show(false);
        }

        static FormationManager FindFormation()
        {
            var pc = GameManager.Instance.JoinCharacters.First();
            var formation = pc.transform.GetComponentInChildren<FormationManager>();
            return formation;
        }

        static Role FindPet(string petName)
        {
            var formation = FindFormation();
            var roles = formation.Roles;
            Debug.Assert(roles.Any(x => x.ID.Name.Equals(petName)), $"Not found in Player's FormationManager : {petName}");
            var pet = roles.Where(x => x.ID.Name.Equals(petName)).First();
            return pet;

        }

    }
}