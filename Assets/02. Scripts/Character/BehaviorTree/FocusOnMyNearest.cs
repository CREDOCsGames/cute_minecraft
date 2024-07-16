using System.Linq;
using UnityEngine;

namespace PlatformGame.Character.BehaviorTree
{
    public class FocusOnMyNearest : Focusing
    {
        public override void OnAwake()
        {
            base.OnAwake();
        }

        protected override Transform GetTargetInFocus()
        {
            return GameManager.Instance.JoinCharacters.OrderBy(x => Vector3.Distance(x.transform.position, mMe.transform.position))
                                                      .First()
                                                      .transform;
        }

    }
}