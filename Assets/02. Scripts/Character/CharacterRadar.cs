using PlatformGame.Character.Combat;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Character
{
    public class CharacterRadar : MonoBehaviour
    {
        public string CharacterName;
        public void Rest()
        {
            GameManager.Instance.JoinCharacters.First().Rest();
        }

        public void ReleaseRest()
        {
            GameManager.Instance.JoinCharacters.First().ReleaseRest();
        }

        public void DoAction(ActionData action)
        {
            GameManager.Instance.JoinCharacters.First().DoAction(action.ID);
        }

        public void PlayerAnim(string anim)
        {
            GameManager.Instance.JoinCharacters.First().Model.GetComponent<Animator>().Play(anim);
        }
        public void SelectAndDoAction(ActionData action)
        {
            Debug.Assert(!string.IsNullOrEmpty(CharacterName));
            var character = Character.Instances.Where(x => x.ID.Name.Equals(CharacterName)).First();
            character.DoAction(action.ID);
        }
    }

}
